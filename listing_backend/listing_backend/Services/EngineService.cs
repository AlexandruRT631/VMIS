using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class EngineService(IEngineRepository engineRepository, IMakeRepository makeRepository, IFuelRepository fuelRepository) : IEngineService
{
    public List<Engine> GetAllEngines()
    {
        return engineRepository.GetAllEngines();
    }

    public Engine? GetEngineById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!engineRepository.DoesEngineExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound);
        }
        
        return engineRepository.GetEngineById(id);
    }

    public Engine CreateEngine(Engine engine)
    {
        if (engine == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidEngine);
        }
        if (engine.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (engineRepository.DoesEngineExist(engine.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.EngineAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(engine.EngineCode))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredEngineCode);
        }
        if (engineRepository.DoesEngineExist(engine.EngineCode))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.EngineAlreadyExists);
        }
        if (engine.Displacement <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidDisplacement);
        }
        if (engine.Power <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPower);
        }
        if (engine.Torque <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTorque);
        }
        if (engine.Make == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (engine.Make.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (!makeRepository.DoesMakeExist(engine.Make.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        var make = makeRepository.GetMakeById(engine.Make.Id);
        if (!string.IsNullOrWhiteSpace(engine.Make.Name) && engine.Make.Name != make!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.MakeNameConflict);
        }
        if (engine.Fuel == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFuel);
        }
        if (engine.Fuel.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFuel);
        }
        if (!fuelRepository.DoesFuelExist(engine.Fuel.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound);
        }
        var fuel = fuelRepository.GetFuelById(engine.Fuel.Id);
        if (!string.IsNullOrWhiteSpace(engine.Fuel.Name) && engine.Fuel.Name != fuel!.Name) 
        {
            throw new InvalidArgumentException(ExceptionMessages.FuelNameConflict);
        }
        
        engine.Make = make!;
        engine.Fuel = fuel!;
        return engineRepository.CreateEngine(engine);
    }

    public Engine UpdateEngine(Engine engine)
    {
        if (engine == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidEngine);
        }
        if (engine.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!engineRepository.DoesEngineExist(engine.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound);
        }
        var existingEngine = engineRepository.GetEngineById(engine.Id);
        if (engine.Make != null)
        {
            if (engine.Make.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
            }
            if (!makeRepository.DoesMakeExist(engine.Make.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
            }
            var make = makeRepository.GetMakeById(engine.Make.Id);
            if (!string.IsNullOrWhiteSpace(engine.Make.Name) && engine.Make.Name != make!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.MakeNameConflict);
            }
            existingEngine!.Make = make;
        }
        if (engine.Fuel != null)
        {
            if (engine.Fuel.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidFuel);
            }
            if (!fuelRepository.DoesFuelExist(engine.Fuel.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound);
            }
            var fuel = fuelRepository.GetFuelById(engine.Fuel.Id);
            if (!string.IsNullOrWhiteSpace(engine.Fuel.Name) && engine.Fuel.Name != fuel!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.FuelNotFound);
            }
            existingEngine!.Fuel = fuel;
        }
        if (!string.IsNullOrWhiteSpace(engine.EngineCode))
        {
            existingEngine!.EngineCode = engine.EngineCode;
        }
        if (engine.Displacement > 0)
        {
            existingEngine!.Displacement = engine.Displacement;
        }
        if (engine.Power > 0)
        {
            existingEngine!.Power = engine.Power;
        }
        if (engine.Torque > 0)
        {
            existingEngine!.Torque = engine.Torque;
        }

        return engineRepository.UpdateEngine(existingEngine!);
    }

    public bool DeleteEngine(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!engineRepository.DoesEngineExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.EngineNotFound);
        }
        
        var engine = engineRepository.GetEngineById(id);
        return engineRepository.DeleteEngine(engine!);
    }
}