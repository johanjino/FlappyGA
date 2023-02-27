import intel_jtag_uart

import keyboard

ju = intel_jtag_uart.intel_jtag_uart()

while (True):
    data = ju.read()

    if keyboard.is_pressed("q"):
        break

    if (data == b'1'):
        print(1)