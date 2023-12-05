import socket

def get(x):
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect(('localhost', 3201))
        s.send(bytes(str(x), "utf-8"))
        data = s.recv(1024).decode("utf-8")
        s.close()
        return int(data)
    except Exception as exn:
        print(exn)
        return x

file = open("input.txt")
seeds = file.readline().split(':')[1].strip().split(' ')
file.close()

least = -1
for seed in seeds:
    result = get(seed)
    print(seed, "->", result)
    if least < 0 or least > result:
        least = result
print("Final output: ", least)