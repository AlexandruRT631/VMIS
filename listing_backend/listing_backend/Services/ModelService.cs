using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class ModelService(IModelRepository modelRepository, IMakeRepository makeRepository) : IModelService
{
    public List<Model> GetAllModels()
    {
        return modelRepository.GetAllModels();
    }

    public Model? GetModelById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!modelRepository.DoesModelExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
        }
        
        return modelRepository.GetModelById(id);
    }

    public Model CreateModel(Model model)
    {
        if (model == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
        }
        if (model.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (modelRepository.DoesModelExist(model.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.ModelAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (modelRepository.DoesModelExist(model.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.ModelAlreadyExists);
        }
        if (model.Make == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredMake);
        }
        if (model.Make.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (!makeRepository.DoesMakeExist(model.Make.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        var make = makeRepository.GetMakeById(model.Make.Id);
        if (!string.IsNullOrWhiteSpace(model.Make.Name) && model.Make.Name != make!.Name)
        {
            throw new InvalidArgumentException(ExceptionMessages.MakeNameConflict);
        }

        model.Make = make;
        return modelRepository.CreateModel(model);
    }

    public Model UpdateModel(Model model)
    {
        if (model == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidModel);
        }
        if (model.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!modelRepository.DoesModelExist(model.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
        }
        var existingModel = modelRepository.GetModelById(model.Id);
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            if (modelRepository.DoesModelExist(model.Name))
            {
                throw new ObjectAlreadyExistsException(ExceptionMessages.ModelAlreadyExists);
            }
            existingModel!.Name = model.Name;
        }
        if (model.Make != null)
        {
            if (model.Make.Id <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
            }
            if (!makeRepository.DoesMakeExist(model.Make.Id))
            {
                throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
            }
            var make = makeRepository.GetMakeById(model.Make.Id);
            if (!string.IsNullOrWhiteSpace(model.Make.Name) && model.Make.Name != make!.Name)
            {
                throw new InvalidArgumentException(ExceptionMessages.MakeNameConflict);
            }
            existingModel!.Make = make;
        }
        return modelRepository.UpdateModel(existingModel!);
    }

    public bool DeleteModel(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!modelRepository.DoesModelExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ModelNotFound);
        }
        
        var model = modelRepository.GetModelById(id);
        return modelRepository.DeleteModel(model!);
    }
    
    public List<Model> GetModelsByMakeId(int makeId)
    {
        if (makeId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!makeRepository.DoesMakeExist(makeId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        
        return modelRepository.GetModelsByMakeId(makeId);
    }
}