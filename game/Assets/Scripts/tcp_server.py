import socket
import sqlite3
import json

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
s.listen(1)
print(f"Server started and listening on {HOST}:{PORT}")

# Accept incoming connections
client_conn, addr = s.accept()
print(f"Connected by {addr}")

while True:
    data = client_conn.recv(1024)
    if not data:
        break
    # Convert the received data to a dictionary
    print(data.decode())

    # Insert the playername and score data into the leaderboard table
    if data.decode()!='leaderboard':
        data_dict = json.loads(data.decode())
        playername = data_dict['playername']
        score = data_dict['score']
        c.execute('INSERT INTO leaderboard VALUES (?, ?)', (playername, score))
        conn.commit()
        print(f"Added score {score} for player {playername}")

    # Retrieve the top 5 players with the highest scores and send them back to the client
    elif data.decode()=='leaderboard':
        print("sending the game the leaderboard")
        c.execute('SELECT * FROM leaderboard ORDER BY score DESC LIMIT 5')
        rows = c.fetchall()
        leaderboard_data = [{'playername': row[0], 'score': row[1]} for row in rows]
        print(leaderboard_data)
        client_conn.sendall(json.dumps(leaderboard_data).encode())
        print("Sent leaderboard data to client")

client_conn.close()
