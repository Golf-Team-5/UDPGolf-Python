# coding=utf-8
from sense_hat import SenseHat
from time import sleep
from socket import *

# socket information
serverName = '192.168.104.124'
serverPort = 6789
clientSocket = socket(AF_INET, SOCK_DGRAM)

# implementering af senseHat, samt reset af displayet
sense = SenseHat()
sense.clear()

# Denne metode tager farten fra senseHattens accelerometer's 3 akser
# og lægger dem sammen og sender den gennemsnitlige værdi videre til UDP serveren,
# hvis værdien er over 1 eller under -1
def SwingSpeedCalculation():
    print("Pi is running...")
    while True:
        swing_speed = sense.get_accelerometer_raw()
        x = swing_speed['x']
        y = swing_speed['y']
        z = swing_speed['z']

        average_swing_speed = abs(round((x + y + z / 3), 1))

        if average_swing_speed > 1.5:
            message = str(average_swing_speed)
            clientSocket.sendto(message.encode(), (serverName, serverPort))
            # print (average_swing_speed)
        sleep(0.2)

# her kalder vi vores funktion
SwingSpeedCalculation()
