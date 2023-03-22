#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_jtag_uart_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>
#include <stdio.h>

alt_32 z_read;

volatile alt_u8 RECV_CHAR;

volatile alt_u8 RECV_FLAG = 0;

void uart_read_isr() {
	if(IOADDR_ALTERA_AVALON_JTAG_UART_DATA(JTAG_UART_BASE + ALTERA_AVALON_JTAG_UART_DATA_RVALID_OFST)){
		RECV_CHAR = alt_getchar();
		if(RECV_CHAR != 0){
			RECV_FLAG = 1;
		}
	}
}

void uart_init(void * isr) {
    IOWR_ALTERA_AVALON_JTAG_UART_CONTROL(JTAG_UART_BASE,0xFFFF && ALTERA_AVALON_JTAG_UART_CONTROL_RE_MSK);
    alt_irq_register(JTAG_UART_IRQ, 0, isr);
}

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
	case '-':
		return 0b0111111;
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

void update_score(int score){
	int unit_digit = 0;
	int tenth_digit = 0;
	if (score < 10){
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(score));
	}
	else{
		unit_digit = score % 10;
		tenth_digit = score / 10;
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(unit_digit));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_1_BASE, dec_to_7seg(tenth_digit));
	}
}

int concat_integers(int a, int b) {
    char str_a[32];
    char str_b[32];
    sprintf(str_a, "%d", a);
    sprintf(str_b, "%d", b);
    strcat(str_a, str_b);
    return atoi(str_a);
}



int main() {

	while(1){
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_0_BASE, dec_to_7seg(0));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_1_BASE, dec_to_7seg('x'));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_2_BASE, dec_to_7seg('x'));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_3_BASE, dec_to_7seg('-'));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_4_BASE, dec_to_7seg('-'));
		IOWR_ALTERA_AVALON_PIO_DATA(HEX_5_BASE, dec_to_7seg('-'));
		led_blink(0);

		alt_up_accelerometer_spi_dev * acc_dev;
		acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
		if (acc_dev == NULL) {
			return 1;
		}

		char old_score = 'x';
		int read_data = 1;
		int tenth_value = 0;

		uart_init(uart_read_isr);

		while (1) {
			alt_up_accelerometer_spi_read_z_axis(acc_dev, & z_read);

			if (z_read > 0xffff0000 && read_data) {
				alt_printf("1");
				led_blink(1);
				read_data = 0;
			   }

			if(z_read < 0xffff0000){
				led_blink(0);
				read_data = 1;
				}

			if(RECV_FLAG){
				if(RECV_CHAR != old_score){
					if(RECV_CHAR == 'e'){
						exit(1);
					}
					if(RECV_CHAR == '0'){
						tenth_value++;
					}
					update_score( concat_integers(tenth_value, atoi(&RECV_CHAR)) );
					old_score = RECV_CHAR;
				}

				RECV_FLAG = 0;
			}
		}
	}

    return 0;
}
