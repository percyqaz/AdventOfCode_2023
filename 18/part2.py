file = open("input.txt")
lines = [ line.strip().split() for line in file.read().strip().split("\n") ]
file.close()

x = 0
y = 0
top = []
bottom = []
x_poi = []
total = 0
last = (None, None)

for line in lines:
    d = "RDLU"[int(line[2][-2])]
    l = int(line[2][2:-2], 16)
    if d == "R": 
        total += l + 1
        top.append((x, y, l))
        x += l
    elif d == "U":
        x_poi.append(x)
        y -= l
    elif d == "D": 
        if last[0] == "L": total += l - 1
        x_poi.append(x)
        y += l
    elif d == "L":
        if last[0] == "U": total += last[1] - 1
        total += l + 1
        x -= l
        bottom.append((x, y, l))
    last = (d, l)

if x != 0 or y != 0:
    print("Input data is wrong, you are at ", x, y)
    exit()

x_poi = sorted(list(set(x_poi)))
edges = sorted(bottom + top, key=lambda x:x[1])

def f(x, y):
    for x_e, y_e, l in edges:
        if y_e <= y: continue
        if x_e <= x and x_e + l >= x:
            return y_e - 1

def line(a, y1, y2):
    return y2 - y1 + 1
def box(x1, y1, x2, y2):
    return (x2 - x1 + 1) * (y2 - y1 + 1)

for x, y, l in top:
    lo = x 
    hi = x + l
    total += line(lo, y + 1, f(lo, y))
    for px in x_poi:
        if px <= lo: continue
        if px >= hi: continue
        if (px - 1) > lo:
            total += box(lo + 1, y + 1, px - 1, f(lo + 1, y))
        total += line(px, y + 1, f(px, y))
        lo = px
    if (hi - 1) > lo:
        total += box(lo + 1, y + 1, hi - 1, f(lo + 1, y))
    total += line(hi, y + 1, f(hi, y))
print(total)