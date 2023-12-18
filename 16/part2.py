file = open("input.txt")
lines = [ [ [c, []] for c in line.strip() ] for line in file.read().split("\n") ]
file.close()

def walk(x, y, direction):
    if x < 0 or y < 0:
        return
    if x >= len(lines[0]) or y >= len(lines):
        return
    if direction in lines[y][x][1]:
        return

    lines[y][x][1].append(direction)

    if direction == "R":
        if lines[y][x][0] in "\\|":
            yield (x, y + 1, "D")
        if lines[y][x][0] in "/|":
            yield (x, y - 1, "U")
        if lines[y][x][0] not in "/|\\":
            yield (x + 1, y, "R")

    elif direction == "L":
        if lines[y][x][0] in "/|":
            yield (x, y + 1, "D")
        if lines[y][x][0] in "\\|":
            yield (x, y - 1, "U")
        if lines[y][x][0] not in "/|\\":
            yield (x - 1, y, "L")
        
    elif direction == "U":
        if lines[y][x][0] in "/-":
            yield (x + 1, y, "R")
        if lines[y][x][0] in "\\-":
            yield (x - 1, y, "L")
        if lines[y][x][0] not in "/-\\":
            yield (x, y - 1, "U")
        
    elif direction == "D":
        if lines[y][x][0] in "\\-":
            yield (x + 1, y, "R")
        if lines[y][x][0] in "/-":
            yield (x - 1, y, "L")
        if lines[y][x][0] not in "/-\\":
            yield (x, y + 1, "D")
        
def test(x, y, d):
    for row in lines:
        for c in row:
            c[1] = []

    beams = [(x, y, d)]
    while beams:
        new_beams = []
        for b in beams:
            for nb in walk(b[0], b[1], b[2]):
                new_beams.append(nb)
        beams = new_beams

    return(sum([ sum([ 1 if len(a[1]) > 0 else 0 for a in row ]) for row in lines ]))

best = 0

w = len(lines[0])
h = len(lines)

for x in range(w):
    best = max(best, test(x, 0, "D"))
    best = max(best, test(x, h - 1, "U"))

for y in range(h):
    best = max(best, test(0, y, "R"))
    best = max(best, test(w - 1, y, "L"))


print(best)