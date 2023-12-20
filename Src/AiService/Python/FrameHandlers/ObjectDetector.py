from cv2 import rectangle, putText, getTextSize, FONT_HERSHEY_SIMPLEX
from typeAliases import RgbImage, BoundingBox, LabelName
from FrameHandlers.FrameHandler import FrameHandler
from ModelWrappers.ModelWrapper import ModelWrapper

class ObjectDetector(FrameHandler):
    def __init__(self, modelWrapper: ModelWrapper, treshold: float) -> None:
        self.__modelWrapper = modelWrapper
        self.__treshold = treshold
    
    def handle(self, frame: RgbImage) -> RgbImage:
        (originalHeight, originalWidth, _) = frame.shape

        output = self.__modelWrapper.predict(frame)
        classes = self.__modelWrapper.extractClasses(output)
        scores =  self.__modelWrapper.extractScores(output)
        boxes =  self.__modelWrapper.extractBoxes(output)
        detectionsCount = self.__modelWrapper.extractNumberOfDetections(output)

        for i in range(0, detectionsCount):
            if scores[i] < self.__treshold:
                continue

            label = self.__modelWrapper.labels[classes[i]]
            box = [
                int(boxes[i][0] * originalHeight), 
                int(boxes[i][1] * originalWidth), 
                int(boxes[i][2] * originalHeight), 
                int(boxes[i][3] * originalWidth)
            ]

            frame = self.__applyBox(frame, box)
            frame = self.__applyLabel(frame, label, [box[1], box[2]])

        return frame

    def __applyBox(self, frame: RgbImage, box: BoundingBox) -> RgbImage:
        return rectangle(
            frame, 
            (box[1], box[0]), 
            (box[3], box[2]), 
            thickness=3, 
            color=(255, 0, 0),
        )

    def __applyLabel(self, frame: RgbImage, label: LabelName, point: tuple[int, int]) -> RgbImage:
        (textWidth, textHeight), _ = getTextSize(label, FONT_HERSHEY_SIMPLEX, 1.4, 2)

        frame = rectangle(
            frame, 
            (point[0], point[1] - textHeight), 
            (point[0] + textWidth, point[1] + 8), 
            thickness=-1, 
            color=(255, 0, 0),
        )
        frame = putText(
            frame, 
            label, 
            (point[0], point[1] - 4),
            FONT_HERSHEY_SIMPLEX, 
            1.4, 
            (255, 255, 255), 
            2
        )

        return frame