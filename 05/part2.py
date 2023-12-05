import socket

def get(lo, hi):
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect(('localhost', 6401))
        s.send(bytes(str(lo)+","+str(hi), "utf-8"))
        data = s.recv(1024).decode("utf-8")
        s.close()
        return int(data)
    except Exception as exn:
        print(exn)
        return lo

file = open("input.txt")
seeds = file.readline().split(':')[1].strip().split(' ')
file.close()

least = -1
for i in range(0, len(seeds), 2):
    lo = int(seeds[i])
    hi = int(seeds[i+1]) - 1 + lo
    result = get(lo, hi)
    if least < 0 or least > result:
        least = result
print("Final output: ", least)