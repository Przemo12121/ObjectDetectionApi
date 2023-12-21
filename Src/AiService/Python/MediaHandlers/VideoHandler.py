from MediaHandlers.MediaHandler import MediaHandler
from FrameHandlers.FrameHandler import FrameHandler
from cv2 import VideoCapture, VideoWriter, VideoWriter_fourcc, CAP_PROP_FPS, CAP_PROP_FRAME_WIDTH, CAP_PROP_FRAME_HEIGHT

class VideoHandler(MediaHandler):
    def __init__(self, frameHandler: FrameHandler) -> None:
        self.__frameHandler = frameHandler

    def handle(self, inputFilePath: str, outputFilePath: str) -> None:
        inputFile = VideoCapture(inputFilePath)
        fps = int(inputFile.get(CAP_PROP_FPS))
        frameSize = (int(inputFile.get(CAP_PROP_FRAME_WIDTH)), int(inputFile.get(CAP_PROP_FRAME_HEIGHT)))
        
        outputFile = VideoWriter( outputFilePath, VideoWriter_fourcc(*'mp4v'), fps, frameSize)

        hasNext, frame = inputFile.read()
        while hasNext:
            frame = self.__frameHandler.handle(frame)
            outputFile.write(frame)
            hasNext, frame = inputFile.read()

        outputFile.release()