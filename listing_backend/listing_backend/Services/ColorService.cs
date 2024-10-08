using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class ColorService(IColorRepository colorRepository) : IColorService
{
    public List<Color> GetAllColors()
    {
        return colorRepository.GetAllColors();
    }

    public Color? GetColorById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!colorRepository.DoesColorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound);
        }
        
        return colorRepository.GetColorById(id);
    }

    public Color CreateColor(Color color)
    {
        if (color == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidColor);
        }
        if (color.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (colorRepository.DoesColorExist(color.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.ColorAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(color.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (colorRepository.DoesColorExist(color.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.ColorAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(color.HexCode))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredHexCode);
        }
        
        return colorRepository.CreateColor(color);
    }

    public Color UpdateColor(Color color)
    {
        
        if (color == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidColor);
        }
        if (color.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!colorRepository.DoesColorExist(color.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound);
        }
        var existingColor = colorRepository.GetColorById(color.Id);
        if (!string.IsNullOrWhiteSpace(color.Name))
        {
            if (colorRepository.DoesColorExist(color.Name))
            {
                throw new ObjectAlreadyExistsException(ExceptionMessages.ColorAlreadyExists);
            }
            existingColor!.Name = color.Name;
        }
        if (!string.IsNullOrWhiteSpace(color.HexCode))
        {
            existingColor!.HexCode = color.HexCode;
        }
        
        return colorRepository.UpdateColor(existingColor!);
    }

    public bool DeleteColor(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!colorRepository.DoesColorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ColorNotFound);
        }
        
        var color = colorRepository.GetColorById(id);
        return colorRepository.DeleteColor(color!);
    }
}