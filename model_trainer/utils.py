import torch
import matplotlib
import matplotlib.pyplot as plt

matplotlib.style.use('ggplot')


def save_trained_model(epoch, model, optimizer, loss, file_path):
    """
    Saves the trained model state along with the optimizer state and loss to a file.

    Args:
    epoch (int): The number of training epochs
    model (torch.nn.Module): The trained model
    optimizer (torch.optim.Optimizer): The optimizer used in training the model
    loss (torch.nn.modules.loss): The loss function used in training the model
    file_path (str): The path of the file to save the model state
    """
    torch.save({
        'epoch': epoch,
        'model_state_dict': model.state_dict(),
        'optimizer_state_dict': optimizer.state_dict(),
        'loss': loss,
    }, file_path)


def plot_metric(metric_values, metric_name, color='blue', linestyle='-'):
    """
    Plots the given metric values with respect to the epochs.

    Args:
    metric_values (list of float): A list containing metric values
    metric_name (str): The name of the metric being plotted
    color (str, optional): The color of the plot. Defaults to 'blue'.
    linestyle (str, optional): The style of the line in the plot. Defaults to '-'.
    """
    plt.plot(metric_values, color=color, linestyle=linestyle, label=f'train {metric_name}')


def save_plots(train_acc, test_acc, train_loss, test_loss):
    """
    Saves the accuracy and loss plots to disk.

    Args:
    train_acc (list of float): A list containing accuracy values of training data
    test_acc (list of float): A list containing accuracy values of test data
    train_loss (list of float): A list containing loss values of training data
    test_loss (list of float): A list containing loss values of test data
    """
    # Accuracy plots.
    plt.figure(figsize=(10, 7))
    plot_metric(train_acc, 'accuracy', color='green')
    plot_metric(test_acc, 'test accuracy', color='blue')
    plt.xlabel('Epochs')
    plt.ylabel('Accuracy')
    plt.legend()
    plt.savefig("outputs/accuracy.png")

    # Loss plots.
    plt.figure(figsize=(10, 7))
    plot_metric(train_loss, 'loss', color='orange')
    plot_metric(test_loss, 'test loss', color='red')
    plt.xlabel('Epochs')
    plt.ylabel('Loss')
    plt.legend()
    plt.savefig("outputs/loss.png")
