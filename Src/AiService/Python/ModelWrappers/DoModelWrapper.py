from tensorflow import saved_model, expand_dims, image, get_logger
from ModelWrappers.ModelWrapper import ModelWrapper
from os import path
from numpy import array
from typeAliases import RgbImage

class D0ModelWrapper(ModelWrapper):
    def __init__(self, pathToModel: str) -> None:
        self.__model = saved_model.load(pathToModel)
        self.__labels = D0ModelWrapper.__loadLabels(path.join(pathToModel, "labels.txt"))
        self.inputShape = (640, 480)

    @property
    def labels(self) -> list[str]:
        return self.__labels

    @staticmethod
    def __loadLabels(path: str) -> list[str]:
        with open(path, "r") as file:
            return [
                l.strip().split(": ")[1] 
                for l in file.readlines(-1)
            ]
    
    def predict(self, data: RgbImage) -> dict:
        preprocessedData = array(
            image.resize(data, size=(480, 640)), 
            dtype="uint8"
        )
        return self.__model(expand_dims(preprocessedData, axis=0))
    
    def extractClasses(self, output) -> list[int]:
        return [int(c) for c in output["detection_classes"][0]]
    
    def extractScores(self, output) -> list[float]:
        return output["detection_scores"][0]

    def extractBoxes(self, output) -> list[float]:
        return output["detection_boxes"][0]

    def extractNumberOfDetections(self, output) -> list[float]:
        return int(output["num_detections"][0].numpy())