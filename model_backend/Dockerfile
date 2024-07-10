# Use an official Python runtime as a parent image
FROM python:3.11-slim-buster as builder

# Set environment variables
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

# Set work directory
WORKDIR /code

# Install dependencies
COPY requirements.txt /code/
RUN pip3 install --no-cache-dir -r requirements.txt

# Copy project
COPY . /code/

# Use multi-stage build to create a smaller image
FROM python:3.11-slim-buster as runner

WORKDIR /code

# Copy the dependencies from the builder stage
COPY --from=builder /usr/local /usr/local

# Copy the source code from the builder stage
COPY --from=builder /code /code

EXPOSE 5000

CMD ["gunicorn", "-t", "180", "app:app", "--bind", "0.0.0.0:5000"]