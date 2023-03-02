import intel_jtag_uart
import keyboard
from time import sleep
import os

dir_path = os.path.dirname(os.path.realpath(__file__))
os.chdir(dir_path)

usb_port = intel_jtag_uart.intel_jtag_uart()



while (True):

    if keyboard.is_pressed("q"):
        break

    data = usb_port.read()

    if (data == b'1'):
        file = open("jump_data.txt" , "w")
        print(1)
        file.write("1")
        file.close()
        sleep(0.15)
    else:
        file = open("jump_data.txt" , "w")
        print("0")
        file.write("0")
        file.close()
