import os
import sys
import time

# mime = sys.argv[1]
# inputFile = sys.argv[2]
# outputFile = sys.argv[3]
# modelPath = sys.argv[4]
# treshold = float(sys.argv[5])
outputFile = sys.argv[1]

with open(outputFile, "w+") as output:
    for i in range(0, 60):
        output.write(str(i))
        output.write("\n")
        time.sleep(0.5)