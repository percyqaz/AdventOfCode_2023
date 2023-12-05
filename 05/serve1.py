import sys
id = int(sys.argv[1])

file = open("input.txt")
data = file.read()
file.close()
rules = []
for line in data.split(":\n")[id].split('\n'):
    if line.strip() == "":
        break
    dest, start, count = line.split(" ")
    rules.append((int(dest), int(start), int(count)))

def rule(x):
    for (dest, start, count) in rules:
        if x >= start and x < start + count:
            return x - start + dest
    return x
    
import socket

def get_next(x):
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect(('localhost', 3200 + id + 1))
        s.send(bytes(str(x), "utf-8"))
        data = s.recv(1024).decode("utf-8")
        s.close()
        return int(data)
    except Exception as exn:
        print(exn)
        return x

serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serversocket.bind(('localhost', 3200 + id))
serversocket.listen(2)

while True:
    connection, _ = serversocket.accept()
    request = int(connection.recv(1024).decode("utf-8"))
    result = rule(request)
    print(str(request) + " -> ", str(result))
    connection.send(bytes(str(get_next(result)), "utf-8"))
    connection.close()