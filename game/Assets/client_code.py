
import intel_jtag_uart
import keyboard
from time import sleep
import os

dir_path = os.path.dirname(os.path.realpath(__file__))
os.chdir(dir_path)

jtag = intel_jtag_uart.intel_jtag_uart()

old_game_score = "x"

file = open("score_data.txt" , "w")
file.write("e")
file.close()

while (True):

    if keyboard.is_pressed("q"):
        break

    file = open("score_data.txt" , "r")
    game_score = file.read()
    if (game_score != old_game_score and game_score != "" and game_score!="0"):
        print("Score: " + game_score)
        old_game_score = game_score
        jtag.write(game_score.encode())
    file.close()

    data = jtag.read()
   
    if (data == b'1'):
        file = open("jump_data.txt" , "w")
        print("Jump")
        file.write("1")
        file.close()
        sleep(0.12)
    else:
        file = open("jump_data.txt" , "w")
        file.write("0")
        file.close()