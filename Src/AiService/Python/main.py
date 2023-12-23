# forces tf to not log anything
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
from tensorflow import _logging
_logging.disable()

import sys
from ModelWrappers.DoModelWrapper import D0ModelWrapper
from MediaHandlers.ImageHandler import ImageHandler
from FrameHandlers.ObjectDetector import ObjectDetector
from MediaHandlers.VideoHandler import VideoHandler

if len(sys.argv) < 6:
    raise Exception("Insufficient number of cli arguments.")

# env('PROD', True)

mime = sys.argv[1]
inputFile = sys.argv[2]
outputFile = sys.argv[3]
modelPath = sys.argv[4]
treshold = float(sys.argv[5])

modelWrapper = D0ModelWrapper(modelPath)
frameHandler = ObjectDetector(modelWrapper, treshold)
mediaHandler = ImageHandler(frameHandler) if mime == "image" else VideoHandler(frameHandler)
mediaHandler.handle(inputFile, outputFile)