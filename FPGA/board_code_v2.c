#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>
#include <stdbool.h>
#include <sys/alt_stdio.h>
#include <stdio.h>
#include <altera_avalon_jtag_uart_regs.h>
#include <string.h>

int dec_to_7seg(char number){
	switch(number){
	case 0:
		return 0b1000000;
	case 1:
		return 0b1111001;
	case 2:
		return 0b0100100;
	case 3:
		return 0b0110000;
	case 4:
		return 0b0011001;
	case 5:
		return 0b0010010;
	case 6:
		return 0b0000010;
	case 7:
		return 0b1111000;
	case 8:
		return 0b0000000;
	case 9:
		return 0b0010000;
	default:
		return 0b1111111;
	}
}

void led_blink(int state){
	if (state == 1){
		IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b1111111111);
	}
	else{
		IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000000000);
	}
}

//void update_score(int score){
//
//	int unit_digit = 0;
//	int tenth_digit = 0;
//
//	if (score < 10){
//		IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(score));
//	}
//	else{
//		score = score - 1;
//		unit_digit = score % 10;
//		tenth_digit = score / 10;
//		IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(unit_digit));
//		IOWR_ALTERA_AVALON_PIO_DATA(HEX_1_BASE, dec_to_7seg(tenth_digit));
//	}
//}



int main() {
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(99));
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_1_BASE, dec_to_7seg(99));
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_2_BASE, dec_to_7seg(99));
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_3_BASE, dec_to_7seg(99));
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_4_BASE, dec_to_7seg(99));
	IOWR_ALTERA_AVALON_PIO_DATA(HEX_5_BASE, dec_to_7seg(99));
    led_blink(0);

    alt_32 axis_read;
    alt_up_accelerometer_spi_dev * acc_dev;
    acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
    if (acc_dev == NULL) {
        return 1;
    }

    bool read_data = true;

    while (1) {

    	alt_up_accelerometer_spi_read_z_axis(acc_dev, & axis_read);
        if (axis_read > 0xfffff000 && read_data) {
        	printf("1");
        	led_blink(1);
        	read_data = false;
           }

		if(axis_read < 0xfffff000){
			led_blink(0);
			read_data = true;
			}
    }

    return 0;
}
