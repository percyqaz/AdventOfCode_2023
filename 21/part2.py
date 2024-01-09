### PHASE 1: find some values by trying 65 + multiples of 131 steps

file = open("input.txt")
lines = [ line.strip() for line in file.read().split("\n") ]
file.close()

positions = dict()
rocks = dict()

w = len(lines[0])
h = len(lines)

for y in range(h):
    line = lines[y]
    for x in range(w):
        if line[x] == "S":
            positions[(x, y)] = "S"
        elif line[x] == "#":
            rocks[(x, y)] = True
            
def has_rock(x, y):
    return (x % w, y % h) in rocks
            
def step(positions):
    new_positions = dict()
    for x, y in positions:
        for rx, ry in [(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)]:
            if not(has_rock(rx, ry)):
                new_positions[(rx, ry)] = "O"
    return new_positions
    
reality = []
for i in range(65):
    positions = step(positions)
reality = [len(positions)]
for i in range(131):
    positions = step(positions)
reality.append(len(positions))
for i in range(131):
    positions = step(positions)
reality.append(len(positions))
print("PHASE 1 COMPLETE, Cracked these values: " + str(reality))

# PHASE 2: Find quadratic, reverse engineered from a little bit of number theory/spotting the "highway" in our input data

coefficient1 = reality[0]
coefficient3 = (reality[2] - reality[1]) // 2 - (reality[1] - reality[0]) // 2
coefficient2 = reality[1] - coefficient3 - coefficient1

print("PHASE 2 COMPLETE, Coefficients: " + str([coefficient1, coefficient2, coefficient3]))
    
# PHASE 3:

x = 26501365 // 131
answer = coefficient3 * x * x + coefficient2 * x + coefficient1
print("PHASE 3 COMPLETE, Solution: " + str(answer))