import os
import shutil
from math import ceil

# Define paths
source_dir = 'data'
target_dir = os.path.join('car_data')
train_dir = os.path.join(target_dir, 'train')
test_dir = os.path.join(target_dir, 'test')

# Ensure target directories exist
os.makedirs(train_dir, exist_ok=True)
os.makedirs(test_dir, exist_ok=True)
class_names = []

# Traverse the source directory
for brand in os.listdir(source_dir):
    brand_path = os.path.join(source_dir, brand)
    if os.path.isdir(brand_path):
        for model in os.listdir(brand_path):
            model_path = os.path.join(brand_path, model)
            if os.path.isdir(model_path):
                for year in os.listdir(model_path):
                    year_path = os.path.join(model_path, year)
                    if os.path.isdir(year_path):
                        # Create the new folder name
                        new_folder_name = f"{brand} {model} {year}"
                        class_names.append(new_folder_name)
                        train_folder_path = os.path.join(train_dir, new_folder_name)
                        test_folder_path = os.path.join(test_dir, new_folder_name)

                        # Ensure target class directories exist
                        os.makedirs(train_folder_path, exist_ok=True)
                        os.makedirs(test_folder_path, exist_ok=True)

                        # Collect all image paths
                        images = [f for f in os.listdir(year_path) if os.path.isfile(os.path.join(year_path, f))]

                        # Split images into train and test sets
                        split_index = ceil(0.8 * len(images))
                        train_images = images[:split_index]
                        test_images = images[split_index:]

                        # Copy images to the respective directories
                        for img in train_images:
                            src_img_path = os.path.join(year_path, img)
                            dst_img_path = os.path.join(train_folder_path, img)
                            shutil.copy2(src_img_path, dst_img_path)

                        for img in test_images:
                            src_img_path = os.path.join(year_path, img)
                            dst_img_path = os.path.join(test_folder_path, img)
                            shutil.copy2(src_img_path, dst_img_path)


# Function to verify that images are correctly split and copied
def verify_split(source_dir, train_dir, test_dir):
    for brand in os.listdir(source_dir):
        brand_path = os.path.join(source_dir, brand)
        if os.path.isdir(brand_path):
            for model in os.listdir(brand_path):
                model_path = os.path.join(brand_path, model)
                if os.path.isdir(model_path):
                    for year in os.listdir(model_path):
                        year_path = os.path.join(model_path, year)
                        if os.path.isdir(year_path):
                            new_folder_name = f"{brand} {model} {year}"
                            train_folder_path = os.path.join(train_dir, new_folder_name)
                            test_folder_path = os.path.join(test_dir, new_folder_name)
                            original_images = set(os.listdir(year_path))
                            train_images = set(os.listdir(train_folder_path))
                            test_images = set(os.listdir(test_folder_path))
                            assert len(train_images) + len(test_images) == len(
                                original_images), f"Split mismatch in {new_folder_name}"
                            assert train_images | test_images == original_images, f"Image set mismatch in {new_folder_name}"


# Run verification
verify_split(source_dir, train_dir, test_dir)

print(f'class_names = {class_names}')
