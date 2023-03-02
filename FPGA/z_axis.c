#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>
#include <stdbool.h>

int main() {

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
        	alt_printf("1");
        	read_data = false;
           }

		if(axis_read < 0xfffff000){
			read_data = true;
			}
    }

    return 0;
}
