import socket
import sqlite3
import json
import threading

# Create a new SQLite database and leaderboard table
conn = sqlite3.connect('leaderboard.db')
c = conn.cursor()
c.execute('''CREATE TABLE IF NOT EXISTS leaderboard
             (playername text, score int)''')
conn.commit()

# Bind the server to a specific IP address and port
HOST = '0.0.0.0'
PORT = 1234
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))

# Listen for incoming connections
s.listen(5)
print(f"Server started and listening on {HOST}:{PORT}")

def handle_client(conn, addr):
    # Create a new connection and cursor for this thread
    thread_conn = sqlite3.connect('leaderboard.db')
    c = thread_conn.cursor()

    print(f"Connected by {addr}")

    while True:
        data = conn.recv(1024)
        if data:
            # Convert the received data to a dictionary
            print(data.decode())

            # Insert the playername and score data into the leaderboard table
            if data.decode()!='leaderboard':
                data_dict = json.loads(data.decode())
                playername = data_dict['playername']
                score = data_dict['score']
                c.execute('INSERT INTO leaderboard VALUES (?, ?)', (playername, score))
                thread_conn.commit()
                print(f"Added score {score} for player {playername}")

            # Retrieve the top 5 players with the highest scores and send them back to the client
            elif data.decode()=='leaderboard':
                print("sending the game the leaderboard")
                c.execute('SELECT * FROM leaderboard ORDER BY score DESC LIMIT 5')
                rows = c.fetchall()
                leaderboard_data = [{'playername': row[0], 'score': row[1]} for row in rows]
                print(leaderboard_data)
                conn.sendall(json.dumps(leaderboard_data).encode())
                print("Sent leaderboard data to client")

    # Close the client connection and thread connection when the thread is finished
    conn.close()
    thread_conn.close()

while True:
    # Accept incoming connections
    conn, addr = s.accept()
    # Create a new thread to handle the client connection
    threading.Thread(target=handle_client, args=(conn, addr)).start()
