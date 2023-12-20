from tensorflow import io, keras
from numpy import array
from ModelWrappers.ModelWrapper import ModelWrapper
from MediaHandlers.MediaHandler import MediaHandler
from FrameHandlers.OpenCvRectanglesApplier import OpenCvRectanglesApplier

RgbImage = list[list[list[int]]]
ImageSize = tuple[int, int] #(width, height)
BoundingBox = tuple[int, int, int, int] # (yMin, xMin, yMax, yMax)
LabelName = str

class ImageHandler(MediaHandler):
    def __init__(
            self, 
            modelWrapper: ModelWrapper, 
            frameHandler: OpenCvRectanglesApplier
            # handleFrame: callable[[RgbImage, BoundingBox, LabelName], RgbImage]
        ) -> None:
        self.__modelWrapper = modelWrapper
        self.__frameHandler = frameHandler

    def handle(self, inputFilePath: str, outputFilePath: str) -> None:
        frame = self.__loadImage(inputFilePath)
        (originalHeight, originalWidth, _) = frame.shape

        frame = self.__handleFrame(frame, originalHeight, originalWidth)
        self.__saveImage(outputFilePath, frame)

    def __loadImage(self, path: str) -> RgbImage:
        return array(
            io.decode_image(
                io.read_file(path), channels=3
            ), 
            dtype="uint8"
        )

    def __handleFrame(self, frame: RgbImage, originalHeight: int, originalWidth: int) -> RgbImage:
        output = self.__modelWrapper.predict(frame)
        classes = self.__modelWrapper.extractClasses(output)
        scores =  self.__modelWrapper.extractScores(output)
        boxes =  self.__modelWrapper.extractBoxes(output)
        detectionsCount = self.__modelWrapper.extractNumberOfDetections(output)

        for i in range(0, detectionsCount):
            if scores[i] < 0.5:
                continue

            label = self.__modelWrapper.labels[classes[i]]
            box = [
                int(boxes[i][0] * originalHeight), 
                int(boxes[i][1] * originalWidth), 
                int(boxes[i][2] * originalHeight), 
                int(boxes[i][3] * originalWidth)
            ]

            # for handle in list(self.__frameHandlers):
            #     handle(frame, box, label)
            frame = self.__frameHandler.handle(frame, box, label)

        return frame

    def __saveImage(self, path: str, img: list[list[list[int]]]) -> None:
        keras.utils.save_img(path, img)
