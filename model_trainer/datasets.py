from torchvision import datasets, transforms
from torch.utils.data import DataLoader

# Constants.
TRAIN_DIR = 'car_data/train/'
VALID_DIR = 'car_data/test/'
IMAGE_SIZE = 224
BATCH_SIZE = 32
NUM_WORKERS = 4

# Common normalization for both train and validation datasets
normalize = transforms.Normalize(
    mean=[0.485, 0.456, 0.406],
    std=[0.229, 0.224, 0.225]
)

# Training transforms
train_transform = transforms.Compose([
    transforms.Resize((IMAGE_SIZE, IMAGE_SIZE)),
    transforms.RandomHorizontalFlip(p=0.5),
    transforms.RandomRotation(35),
    transforms.RandomAdjustSharpness(sharpness_factor=2, p=0.5),
    transforms.RandomGrayscale(p=0.5),
    transforms.RandomPerspective(distortion_scale=0.5, p=0.5),
    transforms.RandomPosterize(bits=2, p=0.5),
    transforms.ToTensor(),
    normalize
])

# Validation transforms
test_transform = transforms.Compose([
    transforms.Resize((IMAGE_SIZE, IMAGE_SIZE)),
    transforms.ToTensor(),
    normalize
])


def get_datasets():
    """Creates the training and validation datasets."""
    return (
        datasets.ImageFolder(TRAIN_DIR, transform=train_transform),
        datasets.ImageFolder(VALID_DIR, transform=test_transform),
    )


def get_data_loaders(dataset_train, dataset_test):
    """Creates the training and validation data loaders."""
    return (
        DataLoader(dataset_train, batch_size=BATCH_SIZE, shuffle=True, num_workers=NUM_WORKERS),
        DataLoader(dataset_test, batch_size=BATCH_SIZE, num_workers=NUM_WORKERS)
    )
