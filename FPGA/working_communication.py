import intel_jtag_uart
import keyboard
from time import sleep

ju = intel_jtag_uart.intel_jtag_uart()

while (True):

    if keyboard.is_pressed("q"):
        break

    data = ju.read()

    if (data == b'1'):
        file = open("H:\EIE2_Projects\IP_Labs\jump_data_temp.txt", "w")
        print(1)
        file.write("1")
        file.close()
        sleep(0.15)
    else:
        file = open("H:\EIE2_Projects\IP_Labs\jump_data_temp.txt", "w")
        print("0")
        file.write("0")
        file.close()
