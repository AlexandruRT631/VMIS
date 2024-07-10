import torch
import argparse
import torch.nn as nn
import torch.optim as optim
import time
import copy
from tqdm.auto import tqdm

from class_names import class_names
from model import build_model
from datasets import get_datasets, get_data_loaders
from utils import save_trained_model, save_plots

# Global Constants
SEED = 42
DEVICE = ('cuda' if torch.cuda.is_available() else 'cpu')

# Set Seed for Reproducibility
torch.manual_seed(SEED)
torch.cuda.manual_seed(SEED)
torch.backends.cudnn.deterministic = True
torch.backends.cudnn.benchmark = True


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('-e', '--epochs', type=int, default=10, help='Number of epochs to train our network for')
    parser.add_argument('-lr', '--learning-rate', type=float, dest='learning_rate', default=0.001,
                        help='Learning rate for training the model')
    parser.add_argument('-es', '--early-stopping', type=int, dest='early_stopping', default=5,
                        help='Early stopping patience')
    return vars(parser.parse_args())


def epoch_run(loader, model, criterion, optimizer=None):
    running_loss = 0.0
    running_correct = 0

    model.train() if optimizer else model.eval()

    for i, data in tqdm(enumerate(loader), total=len(loader)):
        images, labels = data
        images, labels = images.to(DEVICE), labels.to(DEVICE)
        outputs = model(images)
        loss = criterion(outputs, labels)
        running_loss += loss.item()
        _, predictions = torch.max(outputs.data, 1)
        running_correct += (predictions == labels).sum().item()

        if optimizer:
            optimizer.zero_grad()
            loss.backward()
            optimizer.step()

    epoch_loss = running_loss / len(loader)
    epoch_acc = 100. * (running_correct / len(loader.dataset))

    return epoch_loss, epoch_acc


def main():
    args = parse_args()
    dataset_train, dataset_test = get_datasets()
    print(f"[INFO]: Number of training images: {len(dataset_train)}")
    print(f"[INFO]: Number of test images: {len(dataset_test)}")

    train_loader, test_loader = get_data_loaders(dataset_train, dataset_test)
    lr = args['learning_rate']
    epochs = args['epochs']
    patience = args['early_stopping']

    print(f"Computation device: {DEVICE}")
    print(f"Learning rate: {lr}")
    print(f"Epochs to train for: {epochs}\n")

    model = build_model(weights=True, fine_tune=True, num_classes=len(class_names)).to(DEVICE)
    total_params = sum(p.numel() for p in model.parameters())
    print(f"{total_params:,} total parameters.")
    total_trainable_params = sum(p.numel() for p in model.parameters() if p.requires_grad)
    print(f"{total_trainable_params:,} training parameters.")

    optimizer = optim.Adam(model.parameters(), lr=lr)
    scheduler = optim.lr_scheduler.ReduceLROnPlateau(optimizer, mode='min', patience=3, verbose=True)
    criterion = nn.CrossEntropyLoss()

    train_loss, test_loss = [], []
    train_acc, test_acc = [], []
    best_acc = 0.0
    best_model_wts = copy.deepcopy(model.state_dict())
    early_stopping_counter = 0

    for epoch in range(epochs):
        print(f"[INFO]: Epoch {epoch + 1} of {epochs}")
        train_epoch_loss, train_epoch_acc = epoch_run(train_loader, model, criterion, optimizer)
        test_epoch_loss, test_epoch_acc = epoch_run(test_loader, model, criterion)

        train_loss.append(train_epoch_loss)
        test_loss.append(test_epoch_loss)
        train_acc.append(train_epoch_acc)
        test_acc.append(test_epoch_acc)

        print(f"Training loss: {train_epoch_loss:.3f}, training acc: {train_epoch_acc:.3f}")
        print(f"Test loss: {test_epoch_loss:.3f}, test acc: {test_epoch_acc:.3f}")
        print('-' * 50)

        scheduler.step(test_epoch_loss)

        if test_epoch_acc > best_acc:
            best_acc = test_epoch_acc
            best_model_wts = copy.deepcopy(model.state_dict())
            early_stopping_counter = 0
        else:
            early_stopping_counter += 1

        if early_stopping_counter >= patience:
            print("Early stopping triggered")
            break

        time.sleep(1)

    model.load_state_dict(best_model_wts)
    save_trained_model(epochs, model, optimizer, criterion, 'outputs/model.pth')
    save_plots(train_acc, test_acc, train_loss, test_loss)

    print('TRAINING COMPLETE')


if __name__ == '__main__':
    main()
