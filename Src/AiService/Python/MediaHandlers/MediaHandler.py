from abc import ABC, abstractmethod

class MediaHandler(ABC):
    @abstractmethod
    def handle(self, inputFilePath: str, outputFilePath: str) -> None:
        pass