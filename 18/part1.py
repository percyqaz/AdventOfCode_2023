file = open("input.txt")
lines = [ line.strip().split() for line in file.read().strip().split("\n") ]
file.close()

dig = {}
right = {}
down = {}
x = 0
y = 0

dig[(x, y)] = "."

for line in lines:
    d = line[0]
    l = int(line[1])
    if d == "R":
        down[(x, y)] = None
        for i in range(l):
            x += 1
            dig[(x, y)] = d
            down[(x, y)] = None
    elif d == "U":
        right[(x, y)] = None
        for i in range(l):
            y -= 1
            dig[(x, y)] = d
            right[(x, y)] = None
    elif d == "D":
        for i in range(l):
            y += 1
            dig[(x, y)] = d
    elif d == "L":
        for i in range(l):
            x -= 1
            dig[(x, y)] = d

for x, y in down:
    while True:
        y += 1
        if (x, y) in dig: break
        dig[(x,y)] = "v"

view_x = -50
view_y = -50
for y in range(view_y,view_y + 100):
    print("".join([ dig[(x, y)] if (x, y) in dig else "." for x in range(view_x,view_x + 100)]))
print(len(dig))