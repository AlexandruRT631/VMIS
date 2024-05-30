using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class CarService(
    ICarRepository carRepository,
    IMakeRepository makeRepository,
    IModelRepository modelRepository,
    ICategoryRepository categoryRepository,
    IDoorTypeRepository doorTypeRepository,
    ITransmissionRepository transmissionRepository,
    ITractionRepository tractionRepository,
    IEngineRepository engineRepository
) : ICarService
{
    public List<Car> GetAllCars()
    {
        return carRepository.GetAllCars();
    }

    public Car? GetCarById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!carRepository.DoesCarExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
        }

        return carRepository.GetCarById(id);
    }

    public Car CreateCar(Car car)
    {
        if (car == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCar);
        }
        if (car.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (carRepository.DoesCarExist(car.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.CarAlreadyExists);
        }
        if (car.StartYear < 1900)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidStartYear);
        }
        if (car.EndYear != 0 && car.EndYear < car.StartYear)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidEndYear);
        }
        if (car.Model == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredModel);
        }
        if (car.Model.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
        }
        if (!modelRepository.DoesModelExist(car.Model.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
        }
        var model = modelRepository.GetModelById(car.Model.Id);
        if (!string.IsNullOrWhiteSpace(car.Model.Name) && car.Model.Name != model!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.ModelNameConflict);
        }
        car.Model = model!;
        
        if (car.PossibleCategories == null || car.PossibleCategories.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPossibleCategories);
        }
        var categories = new List<Category>();
        foreach (var possibleCategory in car.PossibleCategories)
        {
            if (possibleCategory.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidCategory + " ID: " + possibleCategory.Id);
            }
            if (!categoryRepository.DoesCategoryExist(possibleCategory.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound + " ID: " + possibleCategory.Id);
            }
            var category = categoryRepository.GetCategoryById(possibleCategory.Id);
            if (!string.IsNullOrWhiteSpace(possibleCategory.Name) && possibleCategory.Name != category!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.CategoryNameConflict + " ID: " + possibleCategory.Id);
            }
            if (!categories.Contains(category!))
            {
                categories.Add(category!);
            }
        }
        car.PossibleCategories = categories;
        
        if (car.PossibleDoorTypes == null || car.PossibleDoorTypes.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPossibleDoorTypes);
        }
        var doorTypes = new List<DoorType>();
        foreach (var possibleDoorType in car.PossibleDoorTypes)
        {
            if (possibleDoorType.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType + " ID: " + possibleDoorType.Id);
            }
            if (!doorTypeRepository.DoesDoorTypeExist(possibleDoorType.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound + " ID: " + possibleDoorType.Id);
            }
            var doorType = doorTypeRepository.GetDoorTypeById(possibleDoorType.Id);
            if (!string.IsNullOrWhiteSpace(possibleDoorType.Name) && possibleDoorType.Name != doorType!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.DoorTypeNameConflict + " ID: " + possibleDoorType.Id);
            }
            if (!doorTypes.Contains(doorType!))
            {
                doorTypes.Add(doorType!);
            }
        }
        car.PossibleDoorTypes = doorTypes;
        
        if (car.PossibleTransmissions == null || car.PossibleTransmissions.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPossibleTransmissions);
        }
        var transmissions = new List<Transmission>();
        foreach (var possibleTransmission in car.PossibleTransmissions)
        {
            if (possibleTransmission.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission + " ID: " + possibleTransmission.Id);
            }
            if (!transmissionRepository.DoesTransmissionExist(possibleTransmission.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound + " ID: " + possibleTransmission.Id);
            }
            var transmission = transmissionRepository.GetTransmissionById(possibleTransmission.Id);
            if (!string.IsNullOrWhiteSpace(possibleTransmission.Name) && possibleTransmission.Name != transmission!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.TransmissionNameConflict + " ID: " +
                                                   possibleTransmission.Id);
            }
            if (!transmissions.Contains(transmission!))
            {
                transmissions.Add(transmission!);
            }
        }
        car.PossibleTransmissions = transmissions;
        
        if (car.PossibleTractions == null || car.PossibleTractions.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPossibleTractions);
        }
        var tractions = new List<Traction>();
        foreach (var possibleTraction in car.PossibleTractions)
        {
            if (possibleTraction.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidTraction + " ID: " + possibleTraction.Id);
            }
            if (!tractionRepository.DoesTractionExist(possibleTraction.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound + " ID: " + possibleTraction.Id);
            }
            var traction = tractionRepository.GetTractionById(possibleTraction.Id);
            if (!string.IsNullOrWhiteSpace(possibleTraction.Name) && possibleTraction.Name != traction!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.TractionNameConflict + " ID: " + possibleTraction.Id);
            }
            if (!tractions.Contains(traction!))
            {
                tractions.Add(traction!);
            }
        }
        car.PossibleTractions = tractions;
        
        if (car.PossibleEngines == null || car.PossibleEngines.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPossibleEngines);
        }
        var engines = new List<Engine>();
        foreach (var possibleEngine in car.PossibleEngines)
        {
            if (possibleEngine.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidEngine + " ID: " + possibleEngine.Id);
            }
            if (!engineRepository.DoesEngineExist(possibleEngine.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound + " ID: " + possibleEngine.Id);
            }
            var engine = engineRepository.GetEngineById(possibleEngine.Id);
            if (!string.IsNullOrWhiteSpace(possibleEngine.EngineCode) && possibleEngine.EngineCode != engine!.EngineCode)
            {
                throw new InvalidArgumentException(ExceptionMessages.EngineCodeConflict + " ID: " + possibleEngine.Id);
            }
            if (!engines.Contains(engine!))
            {
                engines.Add(engine!);
            }
        }
        car.PossibleEngines = engines;

        return carRepository.CreateCar(car);
    }

    public Car UpdateCar(Car car)
    {
        if (car == null) 
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCar);
        }
        if (car.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!carRepository.DoesCarExist(car.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
        }
        
        var existingCar = carRepository.GetCarById(car.Id);
        if (car.StartYear >= 1900)
        {
            if (car.StartYear > existingCar!.EndYear && car.EndYear < 1900)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidStartYear);
            }
            existingCar!.StartYear = car.StartYear;
        }
        if (car.EndYear >= 1900)
        {
            if (car.EndYear < existingCar!.StartYear)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidEndYear);
            }
            existingCar!.EndYear = car.EndYear;
        }
        if (car.Model != null)
        {
            if (car.Model.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
            }
            if (!modelRepository.DoesModelExist(car.Model.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
            }
            var model = modelRepository.GetModelById(car.Model.Id);
            if (!string.IsNullOrWhiteSpace(car.Model.Name) && car.Model.Name != model!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.ModelNameConflict);
            }
            existingCar!.Model = model!;
        }
        if (car.PossibleCategories != null && car.PossibleCategories.Count != 0)
        {
            var categories = new List<Category>();
            foreach (var possibleCategory in car.PossibleCategories)
            {
                if (possibleCategory.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidCategory + " ID: " + possibleCategory.Id);
                }
                if (!categoryRepository.DoesCategoryExist(possibleCategory.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound + " ID: " + possibleCategory.Id);
                }
                var category = categoryRepository.GetCategoryById(possibleCategory.Id);
                if (!string.IsNullOrWhiteSpace(possibleCategory.Name) && possibleCategory.Name != category!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.CategoryNameConflict + " ID: " + possibleCategory.Id);
                }
                if (!categories.Contains(category!))
                {
                    categories.Add(category!);
                }
            }
            existingCar!.PossibleCategories = categories;
        }
        if (car.PossibleDoorTypes != null && car.PossibleDoorTypes.Count != 0)
        {
            var doorTypes = new List<DoorType>();
            foreach (var possibleDoorType in car.PossibleDoorTypes)
            {
                if (possibleDoorType.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType + " ID: " + possibleDoorType.Id);
                }
                if (!doorTypeRepository.DoesDoorTypeExist(possibleDoorType.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound + " ID: " + possibleDoorType.Id);
                }
                var doorType = doorTypeRepository.GetDoorTypeById(possibleDoorType.Id);
                if (!string.IsNullOrWhiteSpace(possibleDoorType.Name) && possibleDoorType.Name != doorType!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.DoorTypeNameConflict + " ID: " + possibleDoorType.Id);
                }
                if (!doorTypes.Contains(doorType!))
                {
                    doorTypes.Add(doorType!);
                }
            }
            existingCar!.PossibleDoorTypes = doorTypes;
        }
        if (car.PossibleTransmissions != null && car.PossibleTransmissions.Count != 0)
        {
            var transmissions = new List<Transmission>();
            foreach (var possibleTransmission in car.PossibleTransmissions)
            {
                if (possibleTransmission.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission + " ID: " + possibleTransmission.Id);
                }
                if (!transmissionRepository.DoesTransmissionExist(possibleTransmission.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound + " ID: " + possibleTransmission.Id);
                }
                var transmission = transmissionRepository.GetTransmissionById(possibleTransmission.Id);
                if (!string.IsNullOrWhiteSpace(possibleTransmission.Name) && possibleTransmission.Name != transmission!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.TransmissionNameConflict + " ID: " +
                                                       possibleTransmission.Id);
                }
                if (!transmissions.Contains(transmission!))
                {
                    transmissions.Add(transmission!);
                }
            }
            existingCar!.PossibleTransmissions = transmissions;
        }
        if (car.PossibleTractions != null && car.PossibleTractions.Count != 0)
        {
            var tractions = new List<Traction>();
            foreach (var possibleTraction in car.PossibleTractions)
            {
                if (possibleTraction.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidTraction + " ID: " + possibleTraction.Id);
                }
                if (!tractionRepository.DoesTractionExist(possibleTraction.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound + " ID: " + possibleTraction.Id);
                }
                var traction = tractionRepository.GetTractionById(possibleTraction.Id);
                if (!string.IsNullOrWhiteSpace(possibleTraction.Name) && possibleTraction.Name != traction!.Name)
                {
                    throw new InvalidArgumentException(ExceptionMessages.TractionNameConflict + " ID: " + possibleTraction.Id);
                }
                if (!tractions.Contains(traction!))
                {
                    tractions.Add(traction!);
                }
            }
            existingCar!.PossibleTractions = tractions;
        }
        if (car.PossibleEngines != null && car.PossibleEngines.Count != 0)
        {
            var engines = new List<Engine>();
            foreach (var possibleEngine in car.PossibleEngines)
            {
                if (possibleEngine.Id <= 0)
                {
                    throw new InvalidArgumentException(ExceptionMessages.InvalidEngine + " ID: " + possibleEngine.Id);
                }
                if (!engineRepository.DoesEngineExist(possibleEngine.Id))
                {
                    throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound + " ID: " + possibleEngine.Id);
                }
                var engine = engineRepository.GetEngineById(possibleEngine.Id);
                if (string.IsNullOrWhiteSpace(possibleEngine.EngineCode) && possibleEngine.EngineCode != engine!.EngineCode)
                {
                    throw new InvalidArgumentException(ExceptionMessages.EngineCodeConflict + " ID: " + possibleEngine.Id);
                }
                if (!engines.Contains(engine!))
                {
                    engines.Add(engine!);
                }
            }
            existingCar!.PossibleEngines = engines;
        }

        return carRepository.UpdateCar(existingCar!);
    }

    public bool DeleteCar(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!carRepository.DoesCarExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
        }

        var car = carRepository.GetCarById(id);
        return carRepository.DeleteCar(car!);
    }
    
    public List<Car> GetCarByMakeModelYear(int makeId, int modelId, int year)
    {
        if (makeId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (!makeRepository.DoesMakeExist(makeId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        if (modelId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
        }
        if (!modelRepository.DoesModelExist(modelId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
        }
        if (year < 1900)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidYear);
        }
        var possibleCars = carRepository.GetCarsByMakeModelYear(makeId, modelId, year);
        if (possibleCars.Count == 0)
        {
            throw new ObjectNotFoundException(ExceptionMessages.CarNotFound);
        }
        
        return possibleCars;
    }
}