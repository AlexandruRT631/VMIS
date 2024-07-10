import json
import os

import cv2
import numpy as np
import torch
from flask import Flask, request, after_this_request
from flask_cors import CORS
from torch import topk
from torch.nn import functional
from torchvision.transforms import transforms

from class_names import class_names
from model import build_model

app = Flask(__name__)
CORS(app)

seed = 42
np.random.seed(seed)
torch.manual_seed(seed)
torch.cuda.manual_seed(seed)
torch.backends.cudnn.deterministic = True
torch.backends.cudnn.benchmark = True

# Define computation device and other configurations.
app.config['DEVICE'] = 'cuda' if torch.cuda.is_available() else 'cpu'
app.config['CLASS_NAMES'] = class_names
app.config['MODEL'] = build_model(
    weights=False,
    fine_tune=False,
    num_classes=len(class_names)
).to(app.config['DEVICE'])
app.config['MODEL'] = app.config['MODEL'].eval()
app.config['MODEL'].load_state_dict(torch.load('model.pth', map_location=app.config['DEVICE'])['model_state_dict'])
app.config['TRANSFORM'] = transforms.Compose(
    [transforms.ToPILImage(),
     transforms.Resize((224, 224)),
     transforms.ToTensor(),
     transforms.Normalize(
         mean=[0.485, 0.456, 0.406],
         std=[0.229, 0.224, 0.225]
     )
     ])


def preprocess_image(image_path):
    # Read and preprocess the image.
    image = cv2.imread(image_path)
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    image_tensor = app.config['TRANSFORM'](image)
    return image_tensor.unsqueeze(0)


def parse_label(full_label):
    split_label = full_label.split()
    make = split_label[0]
    model = ' '.join(split_label[1:len(split_label)-1])
    year = split_label[-1]
    return make, model, year


def predict_label(image_tensor):
    # Forward pass through model.
    outputs = app.config['MODEL'](image_tensor.to(app.config['DEVICE']))
    # Get the softmax probabilities and the class index of top probability.
    probs = functional.softmax(outputs, dim=1).data.squeeze()
    class_idx = topk(probs, 1)[1].int()
    full_label = str(app.config['CLASS_NAMES'][int(class_idx)])
    return parse_label(full_label)


@app.route('/classify', methods=['POST'])
def classify():
    if 'image' not in request.files:
        return 'No file part', 400
    image = request.files['image']
    img_path = os.path.join("static", image.filename)
    image.save(img_path)

    @after_this_request
    def delete_file(response):
        os.remove(img_path)
        return response

    image_tensor = preprocess_image(img_path)
    make, model, year = predict_label(image_tensor)
    return json.dumps({"make": make, "model": model, "year": year}), 200


if __name__ == '__main__':
    app.run(debug=False, host='0.0.0.0', port=5000)
