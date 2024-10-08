namespace listing_backend.Constants;

public static class ExceptionMessages
{
    public const string InvalidToken = "Invalid token.";
    public const string ForbiddenUser = "Forbidden user.";
    public const string InvalidId = "Invalid id.";
    public const string RequiredName = "Name is required.";
    public const string CategoryNotFound = "Category not found.";
    public const string InvalidCategory = "Invalid category.";
    public const string CategoryAlreadyExists = "Category already exists.";
    public const string ColorNotFound = "Color not found.";
    public const string InvalidColor = "Invalid color.";
    public const string ColorAlreadyExists = "Color already exists.";
    public const string DoorTypeNotFound = "Door type not found.";
    public const string InvalidDoorType = "Invalid door type.";
    public const string DoorTypeAlreadyExists = "Door type already exists.";
    public const string FeatureNotFound = "Feature not found.";
    public const string InvalidFeature = "Invalid feature.";
    public const string FeatureAlreadyExists = "Feature already exists.";
    public const string FuelNotFound = "Fuel not found.";
    public const string InvalidFuel = "Invalid fuel.";
    public const string FuelAlreadyExists = "Fuel already exists.";
    public const string MakeNotFound = "Make not found.";
    public const string InvalidMake = "Invalid make.";
    public const string MakeAlreadyExists = "Make already exists.";
    public const string TractionNotFound = "Tractions not found.";
    public const string InvalidTraction = "Invalid traction.";
    public const string TractionAlreadyExists = "Traction already exists.";
    public const string TransmissionNotFound = "Transmission not found.";
    public const string InvalidTransmission = "Invalid transmission.";
    public const string TransmissionAlreadyExists = "Transmission already exists.";
    public const string ModelNotFound = "Model not found.";
    public const string InvalidModel = "Invalid model.";
    public const string ModelAlreadyExists = "Model already exists.";
    public const string MakeNameConflict = "Name in the make database for this id differs.";
    public const string InvalidEngine = "Invalid engine.";
    public const string EngineNotFound = "Engine not found.";
    public const string EngineAlreadyExists = "Engine already exists.";
    public const string RequiredEngineCode = "Engine code is required.";
    public const string InvalidDisplacement = "Invalid displacement.";
    public const string InvalidPower = "Invalid power.";
    public const string InvalidTorque = "Invalid torque.";
    public const string FuelNameConflict = "Name in the fuel database for this id differs.";
    public const string InvalidCar = "Invalid car.";
    public const string CarNotFound = "Car not found.";
    public const string CarAlreadyExists = "Car already exists.";
    public const string InvalidStartYear = "Invalid start year.";
    public const string InvalidEndYear = "Invalid end year.";
    public const string RequiredPossibleCategories = "Possible categories are required.";
    public const string RequiredPossibleDoorTypes = "Possible door types are required.";
    public const string RequiredPossibleTransmissions = "Possible transmissions are required.";
    public const string RequiredPossibleTractions = "Possible tractions are required.";
    public const string RequiredPossibleEngines = "Possible engines are required.";
    public const string ModelNameConflict = "Name in the model database for this id differs.";
    public const string CategoryNameConflict = "Name in the category database for this id differs.";
    public const string DoorTypeNameConflict = "Name in the door type database for this id differs.";
    public const string TransmissionNameConflict = "Name in the transmission database for this id differs.";
    public const string TractionNameConflict = "Name in the traction database for this id differs.";
    public const string EngineCodeConflict = "Code in the engine database for this id differs.";
    public const string InvalidListing = "Invalid listing.";
    public const string ListingNotFound = "Listings not found.";
    public const string ListingAlreadyExists = "Listings already exists.";
    public const string InvalidSellerId = "Invalid seller id.";
    public const string InvalidPrice = "Invalid price.";
    public const string InvalidYear = "Invalid year.";
    public const string InvalidMileage = "Invalid mileage.";
    public const string ColorNameConflict = "Name in the color database for this id differs.";
    public const string RequiredFeatures = "Features are required.";
    public const string CategoryNotPossible = "Category is not possible for this car.";
    public const string DoorTypeNotPossible = "Door type is not possible for this car.";
    public const string RequiredModel = "Model is required.";
    public const string RequiredMake = "Make is required.";
    public const string RequiredFuel = "Fuel is required.";
    public const string RequiredCar = "Car is required.";
    public const string RequiredCategory = "Category is required.";
    public const string RequiredEngine = "Engine is required.";
    public const string RequiredDoorType = "Door type is required.";
    public const string RequiredTransmission = "Transmission is required.";
    public const string RequiredTraction = "Traction is required.";
    public const string RequiredColor = "Color is required.";
    public const string FeatureNameConflict = "Name in the feature database for this id differs.";
    public const string TooManyImages = "Too many images.";
    public const string InvalidImageType = "Invalid image type.";
    public const string ImageTooLarge = "Image too large.";
    public const string RequiredImages = "Images are required.";
    public const string RequiredTitle = "Title is required.";
    public const string RequiredHexCode = "Hex code is required.";
    public const string InvalidPageIndex = "Invalid page index.";
    public const string InvalidPageSize = "Invalid page size.";
    public const string RequiredDescription = "Description is required.";
    public const string ModelMakeConflict = "Make in the model database for this id differs.";
    public const string InvalidMinYear = "Invalid min year.";
    public const string InvalidMaxYear = "Invalid max year.";
    public const string InvalidYearRange = "Invalid year range.";
    public const string InvalidMinMileage = "Invalid min mileage.";
    public const string InvalidMaxMileage = "Invalid max mileage.";
    public const string InvalidMileageRange = "Invalid mileage range.";
    public const string InvalidMinPower = "Invalid min power.";
    public const string InvalidMaxPower = "Invalid max power.";
    public const string InvalidPowerRange = "Invalid power range.";
    public const string InvalidMinTorque = "Invalid min torque.";
    public const string InvalidMaxTorque = "Invalid max torque.";
    public const string InvalidTorqueRange = "Invalid torque range.";
    public const string InvalidMinDisplacement = "Invalid min displacement.";
    public const string InvalidMaxDisplacement = "Invalid max displacement.";
    public const string InvalidDisplacementRange = "Invalid displacement range.";
    public const string InvalidMinPrice = "Invalid min price.";
    public const string InvalidMaxPrice = "Invalid max price.";
    public const string InvalidPriceRange = "Invalid price range.";
    public const string SellerNotFound = "Seller not found.";
    public const string InvalidRole = "User with this role cannot complete action.";
    public const string FavouritesDeleteError = "Listing couldn't be deleted from favourites";
    public const string EmailNotSendNewListing = "Listing was created, but failed sending emails";
}