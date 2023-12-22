file = open("input.txt")
lines = [ [ int(c) for c in line.strip() ] for line in file.read().strip().split("\n") ]
file.close()

# x * y * m

ms = \
    list([ ("U", i + 1) for i in range(3, 10) ]) + \
    list([ ("D", i + 1) for i in range(3, 10) ]) + \
    list([ ("L", i + 1) for i in range(3, 10) ]) + \
    list([ ("R", i + 1) for i in range(3, 10) ])
w = len(lines[0])
h = len(lines)

def dijkstra():
    distances = {}

    distances[0, 0, ('S', 10)] = 0

    explore_next = [(0, 0, ('S', 10))]

    def options(x, y, m):
        if m[0] != "R":
            if x > 3 and m[0] != "L":
                yield (x - 4, y, ("L", 4)), sum([ lines[y][x2] for x2 in [x-4, x-3, x-2, x-1] ])
            elif m[0] == "L" and x > 0 and m[1] < 10:
                yield(x - 1, y, ("L", m[1] + 1)), lines[y][x-1]

        if m[0] != "D":
            if y > 3 and m[0] != "U":
                yield(x, y - 4, ("U", 4)), sum([ lines[y2][x] for y2 in [y-4, y-3, y-2, y-1] ])
            elif m[0] == "U" and y > 0 and m[1] < 10:
                yield(x, y - 1, ("U", m[1] + 1)), lines[y-1][x]

        if m[0] != "L":
            if x + 4 < w and m[0] != "R":
                yield(x + 4, y, ("R", 4)), sum([ lines[y][x2] for x2 in [x+4, x+3, x+2, x+1] ])
            elif m[0] == "R" and x + 1 < w and m[1] < 10:
                yield(x + 1, y, ("R", m[1] + 1)), lines[y][x+1]

        if m[0] != "U":
            if y + 4 < h and m[0] != "D":
                yield(x, y + 4, ("D", 4)), sum([ lines[y2][x] for y2 in [y+4, y+3, y+2, y+1] ])
            elif m[0] == "D" and y + 1 < h and m[1] < 10:
                yield(x, y + 1, ("D", m[1] + 1)), lines[y+1][x]

    def alts(x, y, m):
        yield (x, y, m)
        if m[1] < 2:
            yield (x, y, (m[0], 2))
        if m[1] > 3:
            yield (x, y, (m[0], 3))

    def explore(x, y, m):
        current = (x, y, m)
        for node, d in options(x, y, m):
            new_dist = distances[current] + d
            if node not in distances or new_dist < distances[node]:
                distances[node] = new_dist
                explore_next.append(node)

    while explore_next:
        next = explore_next.pop(0)
        explore(*next)

    return distances

result = dijkstra()
x = w - 1
y = h - 1
#print(result)
while x != 0 or y != 0:
    best_m = None
    best = 99999999
    for m in ms:
        if (x, y, m) not in result: continue
        r = result[ (x, y, m) ]
        if r < best:
            best = r
            best_m = m
    print(x, y, lines[y][x], best, best_m)
    break
    if best_m[0] == "U":
        y += 1
    elif best_m[0] == "D":
        y -= 1
    elif best_m[0] == "L":
        x += 1
    elif best_m[0] == "R":
        x -= 1
    