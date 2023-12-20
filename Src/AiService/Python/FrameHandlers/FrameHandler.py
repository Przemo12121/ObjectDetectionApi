from abc import ABC, abstractclassmethod
from typeAliases import RgbImage

class FrameHandler(ABC):
    @abstractclassmethod
    def handle(self, frame: RgbImage) -> RgbImage:
        pass