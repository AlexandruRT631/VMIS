import torch.nn as nn
from torchvision import models
from torchvision.models import EfficientNet_B1_Weights


def build_model(weights=True, fine_tune=True, num_classes=10):
    """
    Constructs and configures the EfficientNet-B1 model.
    """
    # Load pre-trained weights if specified.
    if weights:
        print('[INFO]: Loading pre-trained weights')
        model = models.efficientnet_b1(weights=EfficientNet_B1_Weights.IMAGENET1K_V2)
    else:
        print('[INFO]: Not loading pre-trained weights')
        model = models.efficientnet_b1(weights=None)

    # Fine-tune all layers if specified, otherwise freeze hidden layers.
    for param in model.parameters():
        param.requires_grad = fine_tune

    print('[INFO]: Fine-tuning all layers...') if fine_tune else print('[INFO]: Freezing hidden layers...')

    # Customize the final classification layer.
    num_features = model.classifier[1].in_features
    model.classifier[1] = nn.Linear(num_features, num_classes)

    return model
