import intel_jtag_uart
import keyboard
from time import sleep
import os

dir_path = os.path.dirname(os.path.realpath(__file__))
os.chdir(dir_path)

usb_port = intel_jtag_uart.intel_jtag_uart()

old_game_score = "-1"

while (True):

    if keyboard.is_pressed("q"):
        break

    file = open("score_data.txt" , "r")
    game_score = file.read()
    if (game_score != old_game_score and game_score != ""):
        print("Score: " + game_score)
        old_game_score = game_score
        usb_port.write(game_score.encode())
        sleep(0.5)
        data = usb_port.read()
        print(data)
    
    file.close()


    
    data = usb_port.read()

    if (data == b'1'):
        file = open("jump_data.txt" , "w")
        print(1)
        file.write("1")
        file.close()
        sleep(0.15)
    else:
        file = open("jump_data.txt" , "w")
        file.write("0")
        file.close()
