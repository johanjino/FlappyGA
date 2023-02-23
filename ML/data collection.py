import socket
import pickle

def append_binary(append_data, filename):
    # Open the binary file for reading and load the existing list
    try:
        with open(filename, 'rb') as f:
            my_list = pickle.load(f)
    except FileNotFoundError:
        my_list = []

    # Append new data to the list
    my_list.append(append_data)

    # Open the binary file for writing and store the updated list
    with open(filename, 'wb') as f:
        pickle.dump(my_list, f)




def process_array(data):
    try:
        jump = int(data[0])
        coordinates = list((float(data[1]), float(data[2]), float(data[3]), int(data[4])))
        print("{} : {}".format(jump,coordinates))
        append_binary(coordinates, "gameData.bin")
        append_binary(jump, "gameData_ref.bin")
    except:
        print(data)



def main():
    host = "127.0.0.1"
    port = 8888

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((host, port))
        s.listen(1)

        print(f"Listening on {host}:{port}")

        while True:
            conn, addr = s.accept()

            with conn:
                #print(f"Connected by {addr}")

                data = conn.recv(1024)
                array = [x for x in data.decode().split(",")]

            process_array(array)

if __name__ == "__main__":
    main()