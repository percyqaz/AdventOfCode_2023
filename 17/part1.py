file = open("input.txt")
lines = [ [ int(c) for c in line.strip() ] for line in file.read().strip().split("\n") ]
file.close()

# x * y * m

ms = [
    ("U", 1),
    ("U", 2),
    ("U", 3),
    ("L", 1),
    ("L", 2),
    ("L", 3),
    ("D", 1),
    ("D", 2),
    ("D", 3),
    ("R", 1),
    ("R", 2),
    ("R", 3),
]
w = len(lines[0])
h = len(lines)

def dijkstra():
    distances = {}
    visited = {}
    for x in range(w):
        for y in range(h):
            for m in ms:
                visited[(x, y, m)] = False
                distances[(x, y, m)] = 9999999

    distances[0, 0, ('U', 1)] = 0
    distances[0, 0, ('L', 1)] = 0

    explore_next = [(0, 0, ('U', 1)), (0, 0, ('L', 1))]

    def options(x, y, m):
        if m[0] != "R":
            if x > 0 and m[0] != "L":
                yield(x - 1, y, ("L", 1))
            elif x > 0 and m[1] < 3:
                yield(x - 1, y, ("L", m[1] + 1))

        if m[0] != "D":
            if y > 0 and m[0] != "U":
                yield(x, y - 1, ("U", 1))
            elif y > 0 and m[1] < 3:
                yield(x, y - 1, ("U", m[1] + 1))

        if m[0] != "L":
            if x + 1 < w and m[0] != "R":
                yield(x + 1, y, ("R", 1))
            elif x + 1 < w and m[1] < 3:
                yield(x + 1, y, ("R", m[1] + 1))

        if m[0] != "U":
            if y + 1 < h and m[0] != "D":
                yield(x, y + 1, ("D", 1))
            elif y + 1 < h and m[1] < 3:
                yield(x, y + 1, ("D", m[1] + 1))

    def alts(x, y, m):
        yield (x, y, m)
        if m[1] < 2:
            yield (x, y, (m[0], 2))
        if m[1] > 3:
            yield (x, y, (m[0], 3))

    def explore(x, y, m):
        current = (x, y, m)
        for node in options(x, y, m):
            new_dist = distances[current] + lines[node[1]][node[0]]
            if new_dist < distances[node]:
                distances[node] = new_dist
                explore_next.append(node)

    while explore_next:
        next = explore_next.pop(0)
        explore(*next)

    return distances

result = dijkstra()
x = w - 1
y = h - 1
while x != 0 or y != 0:
    best_m = None
    best = 99999999
    for m in ms:
        r = result[ (x, y, m) ]
        if r < best:
            best = r
            best_m = m
    print(x, y, lines[y][x], best, best_m)
    if best_m[0] == "U":
        y += 1
    elif best_m[0] == "D":
        y -= 1
    elif best_m[0] == "L":
        x += 1
    elif best_m[0] == "R":
        x -= 1
    