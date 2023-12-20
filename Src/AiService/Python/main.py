import sys
from ModelWrappers.DoModelWrapper import D0ModelWrapper
from MediaHandlers.ImageHandler import ImageHandler
from FrameHandlers.ObjectDetector import ObjectDetector
from MediaHandlers.VideoHandler import VideoHandler

if len(sys.argv) < 4:
    raise Exception("Insufficient number of cli arguments.")

mime = sys.argv[1]
inputFile = sys.argv[2]
outputFile = sys.argv[3]

print("START")
modelPath = "./Python/Models/D0"
modelWrapper = D0ModelWrapper(modelPath)
frameHandler = ObjectDetector(modelWrapper, 0.5)
mediaHandler = ImageHandler(frameHandler) if mime == "image" else VideoHandler(frameHandler)
mediaHandler.handle(inputFile, outputFile)
print("END")