# ObjectDetectionApi

## About
System for uploading image and video files to be processed by R-CNN for detecting [objects](#detected-objects). System is built in microservices architecture, using RabbitMQ as message bus. Communication with the system is possible using [Web API](#about). The system is built entirely with .net7 and C#.

## Diagrams

### System
![system_diagram](./docs/readme/System.drawio.svg)

### Use cases
![use_cases](./docs/readme/Use%20cases.drawio.svg)

## Detected objects
Currently the system uses pretrained Tensorflow object detection model. For detected objects see [reference](#about),

## What is yet to be done
- Web API
  - endpoints for managing files
  - OAuth2 (ex. Msc, Facebook, Google)
  - HTML exported documentation, endpoint for getting docs
- Microservices
    - RabbitMQ
    - R-CNN
    - Cleanup service
    - Notification service (emails)
- Docker containerization
- Tests
    - Unit
    - Integration