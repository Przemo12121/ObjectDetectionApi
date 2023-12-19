# import tensorflow as tf
import numpy as np
import cv2 as cv
import sys
import os
# os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3' 
import tensorflow as tf

if len(sys.argv) < 4:
    raise Exception("Insufficient number of cli arguments.")

mime = sys.argv[1]
inputFile = sys.argv[2]
outputFile = sys.argv[3]

labelPath = "./Python/labels.txt"
modelPath = "./Python/models/d0"
treshold = 0.5
(targetWidth, targetHeight) = (640, 480)

model =  tf.saved_model.load(modelPath)

image = tf.io.decode_image(tf.io.read_file(inputFile), channels=3)
image = np.array(image, dtype="uint8")
(originalHeight, originalWidth, _) = image.shape

imgTensor = np.array(
    tf.image.resize(image, size=(targetHeight, targetWidth)), 
    dtype="uint8"
)
imgTensor = tf.expand_dims(image, axis=0) 

output = model(imgTensor)
classes = output["detection_classes"][0]
scores = output["detection_scores"][0]
boxes = output["detection_boxes"][0]
num_detections = int(output["num_detections"][0].numpy())
print("START")
with open(labelPath, "r") as labelFile:
    labels = [l.strip().split(": ")[1] for l in labelFile.readlines(-1)]
    length = len(labels)
    for i in range(0, num_detections):
        if scores[i] < treshold:
            continue
        
        className = labels[int(classes[i])]
        (yMin, xMin, yMax, xMax) = boxes[i]

        # adds rectangle
        xUpperLeft = int(xMin * originalWidth)
        yUpperLeft = int(yMax * originalHeight)

        image = cv.rectangle(
            image, 
            (xUpperLeft, int(yMin * originalHeight)), 
            (int(xMax * originalWidth), yUpperLeft), 
            thickness=3, 
            color=(255, 0, 0),
        )

        #adds label
        (textWidth, textHeight), _ = cv.getTextSize(className, cv.FONT_HERSHEY_SIMPLEX, 1.4, 2)

        image = cv.rectangle(
            image, 
            (xUpperLeft, yUpperLeft - textHeight), 
            (xUpperLeft + textWidth, yUpperLeft + 8), 
            thickness=-1, 
            color=(255, 0, 0),
        )
        image = cv.putText(
            image, 
            className, 
            (xUpperLeft, yUpperLeft - 4),
            cv.FONT_HERSHEY_SIMPLEX, 
            1.4, 
            (255, 255, 255), 
            2
        )

tf.keras.utils.save_img(outputFile, image)
print("END")