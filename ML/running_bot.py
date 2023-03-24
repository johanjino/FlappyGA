import socket
import tensorflow as tf

last_prediction = False

def process_array(data):
    global last_prediction
    coordinates = list((float(data[1]), float(data[2]), float(data[3]), int(data[4])))
    prediction = model.predict([coordinates])
    print(prediction)   
    if prediction[0][0]>0.45 and not last_prediction:
        last_prediction = True
        return '1'.encode()
    last_prediction = False
    return '0'.encode()



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
                print(f"Connected by {addr}")

                data = conn.recv(1024)
                array = [x for x in data.decode().split(",")]
                out = process_array(array)
                print("sending : {}".format(out))
                conn.sendall(out)


if __name__ == "__main__":
    model = tf.keras.models.load_model('FlappyGA.h5')
    print("Bot Ready....")
    main()