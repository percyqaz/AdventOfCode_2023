file = open("input.txt")
lines = [ line.strip() for line in file.read().split("\n") ]
file.close()

positions = dict()
rocks = dict()

for y in range(len(lines)):
    line = lines[y]
    for x in range(len(line)):
        if line[x] == "S":
            positions[(x, y)] = "S"
        elif line[x] == "#":
            rocks[(x, y)] = True
            
def step(positions):
    new_positions = dict()
    for x, y in positions:
        if (x - 1, y) not in rocks:
            new_positions[(x - 1, y)] = "O"
        if (x + 1, y) not in rocks:
            new_positions[(x + 1, y)] = "O"
        if (x, y - 1) not in rocks:
            new_positions[(x, y - 1)] = "O"
        if (x, y + 1) not in rocks:
            new_positions[(x, y + 1)] = "O"
    return new_positions
    
for i in range(64):
    positions = step(positions)
print(len(positions))