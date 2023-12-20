# from cv2 import rectangle, putText, getTextSize, FONT_HERSHEY_SIMPLEX

# RgbImage: list[list[list[int]]]
# BoundingBox: tuple[int, int, int, int] # (yMin, xMin, yMax, yMax)
# LabelName: str

# def applyOpenCvRectangles(frame: RgbImage, box: BoundingBox, label: LabelName) -> RgbImage:
#     newFrame = rectangle(
#         frame, 
#         (box[1], box[0]), 
#         (box[3], box[2]), 
#         thickness=3, 
#         color=(255, 0, 0),
#     )

#     #adds label
#     (textWidth, textHeight), _ = getTextSize(label, FONT_HERSHEY_SIMPLEX, 1.4, 2)

#     newFrame = rectangle(
#         newFrame, 
#         (box[1], box[0] - textHeight), 
#         (box[1] + textWidth, box[0] + 8), 
#         thickness=-1, 
#         color=(255, 0, 0),
#     )
#     newFrame = putText(
#         newFrame, 
#         label, 
#         (box[1], box[0] - 4),
#         FONT_HERSHEY_SIMPLEX, 
#         1.4, 
#         (255, 255, 255), 
#         2
#     )

#     return newFrame