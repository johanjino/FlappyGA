#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include <stdlib.h>
#include <stdbool.h>

int main() {

    alt_32 axis_read;
    alt_up_accelerometer_spi_dev * acc_dev;
    acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
    if (acc_dev == NULL) {
        return 1; // if return 1, check if the spi ip name is "accelerometer_spi"
    }

    bool read_data = true;
    while (1) {
        alt_up_accelerometer_spi_read_z_axis(acc_dev, & axis_read); // change function name to change axis

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
