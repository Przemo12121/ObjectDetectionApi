from tensorflow import io, keras
from numpy import array
from MediaHandlers.MediaHandler import MediaHandler
from FrameHandlers.FrameHandler import FrameHandler
from typeAliases import RgbImage

class ImageHandler(MediaHandler):
    def __init__(self, frameHandler: FrameHandler) -> None:
        self.__frameHandler = frameHandler

    def handle(self, inputFilePath: str, outputFilePath: str) -> None:
        frame = self.__loadImage(inputFilePath)
        frame = self.__frameHandler.handle(frame)
        self.__saveImage(outputFilePath, frame)

    def __loadImage(self, path: str) -> RgbImage:
        return array(
            io.decode_image(
                io.read_file(path), channels=3
            ), 
            dtype="uint8"
        )

    def __saveImage(self, path: str, img: list[list[list[int]]]) -> None:
        keras.utils.save_img(path, img)