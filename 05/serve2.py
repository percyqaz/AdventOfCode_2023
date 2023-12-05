import sys
id = int(sys.argv[1])

file = open("input.txt")
data = file.read()
file.close()
rules = []
interesting = []
for line in data.split(":\n")[id].split('\n'):
    if line.strip() == "":
        break
    dest, start, count = line.split(" ")
    rules.append((int(dest), int(start), int(count)))
    interesting.append(int(start))
    interesting.append(int(start) + int(count))

def rule(x):
    for (dest, start, count) in rules:
        if x >= start and x < start + count:
            return x - start + dest
    return x

def part2(lo, hi):
    for poi in interesting:
        if poi > lo and poi <= hi:
            yield (rule(lo), rule(poi - 1))
            lo = poi
    yield (rule(lo), rule(hi))
    
import socket

def get_next(lo, hi):
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect(('localhost', 6400 + id + 1))
        s.send(bytes(str(lo)+','+str(hi), "utf-8"))
        data = s.recv(1024).decode("utf-8")
        s.close()
        return int(data)
    except Exception as exn:
        print(exn)
        return lo

serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serversocket.bind(('localhost', 6400 + id))
serversocket.listen(2)

while True:
    connection, _ = serversocket.accept()
    request = connection.recv(1024).decode("utf-8").split(",")
    lo = int(request[0])
    hi = int(request[1])

    min = None
    for (nlo, nhi) in part2(lo, hi):
        print(f"[{lo},{hi}] ~-> [{nlo}, {nhi}]")
        b = get_next(nlo, nhi)
        if min is None or b < min: min = b
    connection.send(bytes(str(min), "utf-8"))
    connection.close()