using System.Text.RegularExpressions;
using listing_backend.Constants;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class ListingService(
    IListingRepository listingRepository,
    ICarRepository carRepository,
    ICategoryRepository categoryRepository,
    IEngineRepository engineRepository,
    IDoorTypeRepository doorTypeRepository,
    ITransmissionRepository transmissionRepository,
    ITractionRepository tractionRepository,
    IColorRepository colorRepository,
    IFeatureRepository featureRepository,
    IMakeRepository makeRepository,
    IModelRepository modelRepository,
    IFuelRepository fuelRepository,
    IImageService imageService
) : IListingService
{
    private static readonly string[] AllowedFileTypes = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 5 MB
    private const int MaxFileCount = 30;
    
    public List<Listing> GetAllListings(int pageIndex, int pageSize)
    {
        if (pageIndex <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageIndex);
        }
        if (pageSize <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageSize);
        }
        
        return listingRepository.GetAllListings(pageIndex, pageSize);
    }

    public Listing? GetListingById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!listingRepository.DoesListingExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
        }
        
        return listingRepository.GetListingById(id);
    }

    public Listing CreateListing(Listing listing, List<IFormFile>? images = null)
    {
        if (listing == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
        }
        if (listing.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (listingRepository.DoesListingExist(listing.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.ListingAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(listing.Title))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredTitle);
        }
        if (listing.SellerId < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidSellerId);
        }
        //TODO: Seller not found
        if (listing.Price < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPrice);
        }
        if (string.IsNullOrWhiteSpace(listing.Description))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredDescription);
        }
        if (listing.Year < 1900)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidYear);
        }
        if (listing.Mileage < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMileage);
        }
        if (listing.Car == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredCar);
        }
        if (listing.Car.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCar);
        }
        if (!carRepository.DoesCarExist(listing.Car.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
        }
        var car = carRepository.GetCarById(listing.Car.Id);
        if (listing.Year < car!.StartYear || listing.Year > car!.EndYear)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidYear);
        }
        if (listing.Category == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredCategory);
        }
        if (listing.Category.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCategory);
        }
        if (!categoryRepository.DoesCategoryExist(listing.Category.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound);
        }
        var category = categoryRepository.GetCategoryById(listing.Category.Id);
        if (!string.IsNullOrWhiteSpace(listing.Category.Name) && listing.Category.Name != category!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.CategoryNameConflict);
        }
        if (!car.PossibleCategories!.Contains(category!))
        {
            throw new InvalidArgumentException(ExceptionMessages.CategoryNotPossible);
        }
        if (listing.Engine == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredEngine);
        }
        if (listing.Engine.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidEngine);
        }
        if (!engineRepository.DoesEngineExist(listing.Engine.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound);
        }
        var engine = engineRepository.GetEngineById(listing.Engine.Id);
        if (listing.Engine.EngineCode != null && listing.Engine.EngineCode != engine!.EngineCode)
        {
            throw new InvalidArgumentException(ExceptionMessages.EngineCodeConflict);
        }
        if (listing.DoorType == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredDoorType);
        }
        if (listing.DoorType.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType);
        }
        if (!doorTypeRepository.DoesDoorTypeExist(listing.DoorType.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound);
        }
        var doorType = doorTypeRepository.GetDoorTypeById(listing.DoorType.Id);
        if (!string.IsNullOrWhiteSpace(listing.DoorType.Name) && listing.DoorType.Name != doorType!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.DoorTypeNameConflict);
        }
        if (!car.PossibleDoorTypes!.Contains(doorType!))
        {
            throw new InvalidArgumentException(ExceptionMessages.DoorTypeNotPossible);
        }
        if (listing.Transmission == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredTransmission);
        }
        if (listing.Transmission.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission);
        }
        if (!transmissionRepository.DoesTransmissionExist(listing.Transmission.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound);
        }
        var transmission = transmissionRepository.GetTransmissionById(listing.Transmission.Id);
        if (!string.IsNullOrWhiteSpace(listing.Transmission.Name) && listing.Transmission.Name != transmission!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.TransmissionNameConflict);
        }
        if (listing.Traction == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredTraction);
        }
        if (listing.Traction.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTraction);
        }
        if (!tractionRepository.DoesTractionExist(listing.Traction.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound);
        }
        var traction = tractionRepository.GetTractionById(listing.Traction.Id);
        if (!string.IsNullOrWhiteSpace(listing.Traction.Name) && listing.Traction.Name != traction!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.TractionNameConflict);
        }
        if (listing.Color == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredColor);
        }
        if (listing.Color.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidColor);
        }
        if (!colorRepository.DoesColorExist(listing.Color.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound);
        }
        var color = colorRepository.GetColorById(listing.Color.Id);
        if (!string.IsNullOrWhiteSpace(listing.Color.Name) && listing.Color.Name != color!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.ColorNameConflict);
        }
        if (listing.Features == null || listing.Features.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredFeatures);
        }
        var features = new List<Feature>();
        foreach (var feature in listing.Features)
        {
            if (feature.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidFeature + " ID: " + feature.Id);
            }
            if (!featureRepository.DoesFeatureExist(feature.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound);
            }
            var featureEntity = featureRepository.GetFeatureById(feature.Id);
            if (!string.IsNullOrWhiteSpace(feature.Name) && feature.Name != featureEntity!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.FeatureNameConflict);
            }
            if (!features.Contains(featureEntity!))
            {
                features.Add(featureEntity!);
            }
        }
        if (images == null || images.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredImages);
        }
        if (images.Count > MaxFileCount)
        {
            throw new InvalidArgumentException(ExceptionMessages.TooManyImages);
        }
        var listingImages = new List<ListingImage>();

        for (int i = 0; i < images.Count; i++)
        {
            var extension = Path.GetExtension(images[i].FileName).ToLowerInvariant();
            if (!AllowedFileTypes.Contains(extension))
            { 
                throw new InvalidArgumentException(ExceptionMessages.InvalidImageType + " File: " + i);
            }
            
            if (images[i].Length > MaxFileSize)
            {
                throw new InvalidArgumentException(ExceptionMessages.ImageTooLarge + " File: " + i);
            }
                
            var url = imageService.SaveImage(images[i], car!);
            listingImages.Add(new ListingImage { Url = url });
        }
        

        listing.Car = car;
        listing.Category = category;
        listing.Engine = engine;
        listing.DoorType = doorType;
        listing.Color = color;
        listing.Transmission = transmission;
        listing.Traction = traction;
        listing.Features = features;
        listing.ListingImages = listingImages;
        return listingRepository.CreateListing(listing);
    }

    public Listing UpdateListing(Listing listing, List<IFormFile>? images = null)
    {
        if (listing == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
        }
        if (listing.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!listingRepository.DoesListingExist(listing.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
        }
        var existingListing = listingRepository.GetListingById(listing.Id);
        if (!string.IsNullOrWhiteSpace(listing.Title))
        {
            existingListing!.Title = listing.Title;
        }
        if (listing.SellerId > 0)
        {
            existingListing!.SellerId = listing.SellerId;
        }
        if (listing.Price > 0)
        {
            existingListing!.Price = listing.Price;
        }
        if (!string.IsNullOrWhiteSpace(listing.Description))
        {
            existingListing!.Description = listing.Description;
        }
        if (listing.Car != null)
        {
            if (listing.Car.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidCar);
            }
            if (!carRepository.DoesCarExist(listing.Car.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
            }
            var car = carRepository.GetCarById(listing.Car.Id);
            if ((listing.Year < car!.StartYear || listing.Year > car!.EndYear) && listing.Year < 1900)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidYear);
            }
            existingListing!.Car = car;
        }
        if (listing.Year >= 1900)
        {
            if (listing.Year < existingListing!.Car!.StartYear || listing.Year > existingListing!.Car!.EndYear)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidYear);
            }
            existingListing!.Year = listing.Year;
        }
        if (listing.Mileage > 0)
        {
            existingListing!.Mileage = listing.Mileage;
        }
        if (listing.Category != null)
        {
            if (listing.Category.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidCategory);
            }
            if (!categoryRepository.DoesCategoryExist(listing.Category.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound);
            }
            var category = categoryRepository.GetCategoryById(listing.Category.Id);
            if (!string.IsNullOrWhiteSpace(listing.Category.Name) && listing.Category.Name != category!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.CategoryNameConflict);
            }
            if (!existingListing!.Car!.PossibleCategories!.Contains(category!))
            {
                throw new InvalidArgumentException(ExceptionMessages.CategoryNotPossible);
            }
            existingListing!.Category = category;
        }
        if (listing.Engine != null)
        {
            if (listing.Engine.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidEngine);
            }
            if (!engineRepository.DoesEngineExist(listing.Engine.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound);
            }
            var engine = engineRepository.GetEngineById(listing.Engine.Id);
            if (!string.IsNullOrWhiteSpace(listing.Engine.EngineCode) && listing.Engine.EngineCode != engine!.EngineCode)
            {
                throw new InvalidArgumentException(ExceptionMessages.EngineCodeConflict);
            }
            existingListing!.Engine = engine;
        }
        if (listing.DoorType != null)
        {
            if (listing.DoorType.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType);
            }
            if (!doorTypeRepository.DoesDoorTypeExist(listing.DoorType.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound);
            }
            var doorType = doorTypeRepository.GetDoorTypeById(listing.DoorType.Id);
            if (!string.IsNullOrWhiteSpace(listing.DoorType.Name) && listing.DoorType.Name != doorType!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.DoorTypeNameConflict);
            }
            if (!existingListing!.Car!.PossibleDoorTypes!.Contains(doorType!))
            {
                throw new InvalidArgumentException(ExceptionMessages.DoorTypeNotPossible);
            }
            existingListing!.DoorType = doorType;
        }
        if (listing.Transmission != null)
        {
            if (listing.Transmission.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission);
            }
            if (!transmissionRepository.DoesTransmissionExist(listing.Transmission.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound);
            }
            var transmission = transmissionRepository.GetTransmissionById(listing.Transmission.Id);
            if (!string.IsNullOrWhiteSpace(listing.Transmission.Name) &&
                listing.Transmission.Name != transmission!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.TransmissionNameConflict);
            }
            existingListing!.Transmission = transmission;
        }
        if (listing.Traction != null)
        {
            if (listing.Traction.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidTraction);
            }
            if (!tractionRepository.DoesTractionExist(listing.Traction.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound);
            }
            var traction = tractionRepository.GetTractionById(listing.Traction.Id);
            if (!string.IsNullOrWhiteSpace(listing.Traction.Name) && listing.Traction.Name != traction!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.TractionNameConflict);
            }
            existingListing!.Traction = traction;
        }
        if (listing.Color != null)
        {
            if (listing.Color.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidColor);
            }
            if (!colorRepository.DoesColorExist(listing.Color.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound);
            }
            var color = colorRepository.GetColorById(listing.Color.Id);
            if (!string.IsNullOrWhiteSpace(listing.Color.Name) && listing.Color.Name != color!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.ColorNameConflict);
            }
            existingListing!.Color = color;
        }
        if (listing.Features != null && listing.Features.Count > 0)
        {
            var features = new List<Feature>();
            foreach (var feature in listing.Features)
            {
                if (feature.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidFeature + " ID: " + feature.Id);
                }
                if (!featureRepository.DoesFeatureExist(feature.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound);
                }
                var featureEntity = featureRepository.GetFeatureById(feature.Id);
                if (!string.IsNullOrWhiteSpace(feature.Name) && feature.Name != featureEntity!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.FeatureNameConflict);
                }
                if (!features.Contains(featureEntity!))
                {
                    features.Add(featureEntity!);
                }
            }
            existingListing!.Features = features;
        }
        if (images != null && images.Count > 0)
        {
            if (images.Count > MaxFileCount)
            {
                throw new InvalidArgumentException(ExceptionMessages.TooManyImages);
            }

            var listingImages = new List<ListingImage>();
            for (int i = 0; i < images.Count; i++)
            {
                var extension = Path.GetExtension(images[i].FileName).ToLowerInvariant();
                if (!AllowedFileTypes.Contains(extension))
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidImageType + " File: " + i);
                }

                if (images[i].Length > MaxFileSize)
                {
                    throw new InvalidArgumentException(ExceptionMessages.ImageTooLarge + " File: " + i);
                }
                
                var url = imageService.SaveImage(images[i], existingListing!.Car!);
                listingImages.Add(new ListingImage { Url = url });
            }
            
            if (existingListing!.ListingImages != null)
            {
                foreach (var listingImage in existingListing.ListingImages)
                {
                    imageService.DeleteImage(listingImage.Url!);
                }
            }
            
            existingListing.ListingImages = listingImages;
        }

        return listingRepository.UpdateListing(existingListing!);
    }

    public bool DeleteListing(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!listingRepository.DoesListingExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
        }
        
        var listing = listingRepository.GetListingById(id);
        foreach (var listingImage in listing!.ListingImages!)
        {
            imageService.DeleteImage(listingImage.Url!);
        }
        
        return listingRepository.DeleteListing(listing!);
    }

    public List<Listing> GetListingsBySearch(ListingSearchDto listingSearchDto, int pageIndex, int pageSize)
    {
        if (pageIndex <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageIndex);
        }
        if (pageSize <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageSize);
        }

        if (listingSearchDto.MakeId != null)
        {
            if (listingSearchDto.MakeId.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
            }
            if (!makeRepository.DoesMakeExist(listingSearchDto.MakeId.Value))
            {
                throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
            }
        }
        
        if (listingSearchDto.ModelId != null)
        {
            if (listingSearchDto.ModelId.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
            }
            if (!modelRepository.DoesModelExist(listingSearchDto.ModelId.Value))
            {
                throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
            }
            var model = modelRepository.GetModelById(listingSearchDto.ModelId.Value);
            if (listingSearchDto.MakeId != null && model!.Make!.Id != listingSearchDto.MakeId)
            {
                throw new InvalidArgumentException(ExceptionMessages.ModelMakeConflict);
            }
        }
        
        if (listingSearchDto.StartYear != null)
        {
            if (listingSearchDto.ModelId == null)
            {
                throw new InvalidArgumentException(ExceptionMessages.RequiredModel);
            }
            if (listingSearchDto.StartYear.Value < 1900)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidStartYear);
            }
        }
        
        if (listingSearchDto.MinYear != null)
        {
            if (listingSearchDto.MinYear.Value < 1900)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinYear);
            }
        }
        if (listingSearchDto.MaxYear != null)
        {
            if (listingSearchDto.MaxYear.Value < 1900)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxYear);
            }
            if (listingSearchDto.MinYear != null && listingSearchDto.MinYear > listingSearchDto.MaxYear)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidYearRange);
            }
        }
        
        if (listingSearchDto.MinMileage != null)
        {
            if (listingSearchDto.MinMileage.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinMileage);
            }
        }
        if (listingSearchDto.MaxMileage != null)
        {
            if (listingSearchDto.MaxMileage.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxMileage);
            }
            if (listingSearchDto.MinMileage != null && listingSearchDto.MinMileage > listingSearchDto.MaxMileage)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMileageRange);
            }
        }

        if (listingSearchDto.Categories is { Count: > 0 })
        {
            foreach (var categoryId in listingSearchDto.Categories)
            {
                if (categoryId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidCategory + " ID: " + categoryId);
                }
                if (!categoryRepository.DoesCategoryExist(categoryId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound + " ID: " + categoryId);
                }
            }
        }

        if (listingSearchDto.Fuels is { Count: > 0 })
        {
            foreach (var fuelId in listingSearchDto.Fuels)
            {
                if (fuelId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidFuel + " ID: " + fuelId);
                }
                if (!fuelRepository.DoesFuelExist(fuelId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound + " ID: " + fuelId);
                }
            }
        }
        
        if (listingSearchDto.MinPower != null)
        {
            if (listingSearchDto.MinPower.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinPower);
            }
        }
        if (listingSearchDto.MaxPower != null)
        {
            if (listingSearchDto.MaxPower.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxPower);
            }
            if (listingSearchDto.MinPower != null && listingSearchDto.MinPower > listingSearchDto.MaxPower)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidPowerRange);
            }
        }
        
        if (listingSearchDto.MinTorque != null)
        {
            if (listingSearchDto.MinTorque.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinTorque);
            }
        }
        if (listingSearchDto.MaxTorque != null)
        {
            if (listingSearchDto.MaxTorque.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxTorque);
            }
            if (listingSearchDto.MinTorque != null && listingSearchDto.MinTorque > listingSearchDto.MaxTorque)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidTorqueRange);
            }
        }
        
        if (listingSearchDto.MinDisplacement != null)
        {
            if (listingSearchDto.MinDisplacement.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinDisplacement);
            }
        }
        if (listingSearchDto.MaxDisplacement != null)
        {
            if (listingSearchDto.MaxDisplacement.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxDisplacement);
            }
            if (listingSearchDto.MinDisplacement != null && listingSearchDto.MinDisplacement > listingSearchDto.MaxDisplacement)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidDisplacementRange);
            }
        }
        
        if (listingSearchDto.DoorTypes is { Count: > 0 })
        {
            foreach (var doorTypeId in listingSearchDto.DoorTypes)
            {
                if (doorTypeId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType + " ID: " + doorTypeId);
                }
                if (!doorTypeRepository.DoesDoorTypeExist(doorTypeId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound + " ID: " + doorTypeId);
                }
            }
        }

        if (listingSearchDto.Transmissions is { Count: > 0 })
        {
            foreach (var transmissionId in listingSearchDto.Transmissions)
            {
                if (transmissionId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission + " ID: " +
                                                       transmissionId);
                }
                if (!transmissionRepository.DoesTransmissionExist(transmissionId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound + " ID: " +
                                                      transmissionId);
                }
            }
        }
        
        if (listingSearchDto.Tractions is { Count: > 0 })
        {
            foreach (var tractionId in listingSearchDto.Tractions)
            {
                if (tractionId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidTraction + " ID: " + tractionId);
                }
                if (!tractionRepository.DoesTractionExist(tractionId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound + " ID: " + tractionId);
                }
            }
        }
        
        if (listingSearchDto.Colors is { Count: > 0 })
        {
            foreach (var colorId in listingSearchDto.Colors)
            {
                if (colorId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidColor + " ID: " + colorId);
                }
                if (!colorRepository.DoesColorExist(colorId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound + " ID: " + colorId);
                }
            }
        }
        
        if (listingSearchDto.Features is { Count: > 0 })
        {
            foreach (var featureId in listingSearchDto.Features)
            {
                if (featureId < 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidFeature + " ID: " + featureId);
                }
                if (!featureRepository.DoesFeatureExist(featureId))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound + " ID: " + featureId);
                }
            }
        }
        
        if (listingSearchDto.MinPrice != null)
        {
            if (listingSearchDto.MinPrice.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMinPrice);
            }
        }
        if (listingSearchDto.MaxPrice != null)
        {
            if (listingSearchDto.MaxPrice.Value < 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMaxPrice);
            }
            if (listingSearchDto.MinPrice != null && listingSearchDto.MinPrice > listingSearchDto.MaxPrice)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidPriceRange);
            }
        }
        
        if (!string.IsNullOrWhiteSpace(listingSearchDto.Keywords))
        {
            listingSearchDto.Keywords = Regex.Replace(listingSearchDto.Keywords, @"\s+", " ");
        }

        return listingRepository.GetListingsBySearch(listingSearchDto, pageIndex, pageSize);
    }
}