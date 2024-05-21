using listing_backend.Constants;
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
    IFeatureExteriorRepository featureExteriorRepository,
    IFeatureInteriorRepository featureInteriorRepository,
    IImageService imageService
) : IListingService
{
    private static readonly string[] AllowedFileTypes = [".jpg", ".jpeg", ".png"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
    private const int MaxFileCount = 30;
    
    public List<Listing> GetAllListings()
    {
        return listingRepository.GetAllListings();
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
        if (listing.InteriorColor == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredInteriorColor);
        }
        if (listing.InteriorColor.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidInteriorColor);
        }
        if (!colorRepository.DoesColorExist(listing.InteriorColor.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.InteriorColorNotFound);
        }
        var interiorColor = colorRepository.GetColorById(listing.InteriorColor.Id);
        if (!string.IsNullOrWhiteSpace(listing.InteriorColor.Name) && listing.InteriorColor.Name != interiorColor!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.InteriorColorNameConflict);
        }
        if (listing.ExteriorColor == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredExteriorColor);
        }
        if (listing.ExteriorColor.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidExteriorColor);
        }
        if (!colorRepository.DoesColorExist(listing.ExteriorColor.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ExteriorColorNotFound);
        }
        var exteriorColor = colorRepository.GetColorById(listing.ExteriorColor.Id);
        if (!string.IsNullOrWhiteSpace(listing.ExteriorColor.Name) && listing.ExteriorColor.Name != exteriorColor!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.ExteriorColorNameConflict);
        }
        if (listing.FeaturesExterior == null || listing.FeaturesExterior.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredFeaturesExterior);
        }
        var featuresExterior = new List<FeatureExterior>();
        foreach (var featureExterior in listing.FeaturesExterior)
        {
            if (featureExterior.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureExterior + " ID: " + featureExterior.Id);
            }
            if (!featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.FeatureExteriorNotFound);
            }
            var featureExteriorEntity = featureExteriorRepository.GetFeatureExteriorById(featureExterior.Id);
            if (!string.IsNullOrWhiteSpace(featureExterior.Name) && featureExterior.Name != featureExteriorEntity!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.FeatureExteriorNameConflict);
            }
            if (!featuresExterior.Contains(featureExteriorEntity!))
            {
                featuresExterior.Add(featureExteriorEntity!);
            }
        }
        if (listing.FeaturesInterior == null || listing.FeaturesInterior.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredFeaturesInterior);
        }
        var featuresInterior = new List<FeatureInterior>();
        foreach (var featureInterior in listing.FeaturesInterior)
        {
            if (featureInterior.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureInterior + " ID: " + featureInterior.Id);
            }
            if (!featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.FeatureInteriorNotFound);
            }
            var featureInteriorEntity = featureInteriorRepository.GetFeatureInteriorById(featureInterior.Id);
            if (!string.IsNullOrWhiteSpace(featureInterior.Name) && featureInterior.Name != featureInteriorEntity!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.FeatureInteriorNameConflict);
            }
            if (!featuresInterior.Contains(featureInteriorEntity!))
            {
                featuresInterior.Add(featureInteriorEntity!);
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
                
            var url = imageService.SaveImage(images[i]);
            listingImages.Add(new ListingImage { Url = url });
        }
        

        listing.Car = car;
        listing.Category = category;
        listing.Engine = engine;
        listing.DoorType = doorType;
        listing.InteriorColor = interiorColor;
        listing.ExteriorColor = exteriorColor;
        listing.Transmission = transmission;
        listing.Traction = traction;
        listing.FeaturesExterior = featuresExterior;
        listing.FeaturesInterior = featuresInterior;
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
        if (listing.InteriorColor != null)
        {
            if (listing.InteriorColor.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidInteriorColor);
            }
            if (!colorRepository.DoesColorExist(listing.InteriorColor.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.InteriorColorNotFound);
            }
            var interiorColor = colorRepository.GetColorById(listing.InteriorColor.Id);
            if (!string.IsNullOrWhiteSpace(listing.InteriorColor.Name) && listing.InteriorColor.Name != interiorColor!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.InteriorColorNameConflict);
            }
            existingListing!.InteriorColor = interiorColor;
        }
        if (listing.ExteriorColor != null)
        {
            if (listing.ExteriorColor.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidExteriorColor);
            }
            if (!colorRepository.DoesColorExist(listing.ExteriorColor.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.ExteriorColorNotFound);
            }
            var exteriorColor = colorRepository.GetColorById(listing.ExteriorColor.Id);
            if (!string.IsNullOrWhiteSpace(listing.ExteriorColor.Name) && listing.ExteriorColor.Name != exteriorColor!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.ExteriorColorNameConflict);
            }
            existingListing!.ExteriorColor = exteriorColor;
        }
        if (listing.FeaturesExterior != null && listing.FeaturesExterior.Count > 0)
        {
            var featuresExterior = new List<FeatureExterior>();
            foreach (var featureExterior in listing.FeaturesExterior)
            {
                if (featureExterior.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureExterior + " ID: " + featureExterior.Id);
                }
                if (!featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.FeatureExteriorNotFound);
                }
                var featureExteriorEntity = featureExteriorRepository.GetFeatureExteriorById(featureExterior.Id);
                if (!string.IsNullOrWhiteSpace(featureExterior.Name) && featureExterior.Name != featureExteriorEntity!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.FeatureExteriorNameConflict);
                }
                if (!featuresExterior.Contains(featureExteriorEntity!))
                {
                    featuresExterior.Add(featureExteriorEntity!);
                }
            }
            existingListing!.FeaturesExterior = featuresExterior;
        }
        if (listing.FeaturesInterior != null && listing.FeaturesInterior.Count > 0)
        {
            var featuresInterior = new List<FeatureInterior>();
            foreach (var featureInterior in listing.FeaturesInterior)
            {
                if (featureInterior.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureInterior + " ID: " + featureInterior.Id);
                }
                if (!featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.FeatureInteriorNotFound);
                }
                var featureInteriorEntity = featureInteriorRepository.GetFeatureInteriorById(featureInterior.Id);
                if (!string.IsNullOrWhiteSpace(featureInterior.Name) && featureInterior.Name != featureInteriorEntity!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.FeatureInteriorNameConflict);
                }
                if (!featuresInterior.Contains(featureInteriorEntity!))
                {
                    featuresInterior.Add(featureInteriorEntity!);
                }
            }
            existingListing!.FeaturesInterior = featuresInterior;
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
                
                var url = imageService.SaveImage(images[i]);
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
        return listingRepository.DeleteListing(listing!);
    }
}