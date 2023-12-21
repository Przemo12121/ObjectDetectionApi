from abc import ABC, abstractmethod, abstractproperty
from typeAliases import RgbImage

class ModelWrapper(ABC):
    @abstractproperty
    def labels(self) -> list[str]:
        pass
    
    @abstractmethod
    def predict(self, data: RgbImage) -> dict:
        pass

    @abstractmethod
    def extractClasses(self, output) -> list[int]:
        pass
    
    @abstractmethod
    def extractScores(self, output) -> list[float]:
        pass

    @abstractmethod
    def extractBoxes(self, output) -> list[float]:
        pass

    @abstractmethod
    def extractNumberOfDetections(self, output) -> int:
        pass