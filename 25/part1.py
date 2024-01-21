file = open("input.txt")
lines = file.readlines()
file.close()

graph = {}

for line in lines:
    split = line.split(":")
    if split[0] not in graph: graph[split[0]] = []
    for item in split[1].strip().split():
        if item not in graph: graph[item] = []
        graph[split[0]].append(item)
        graph[item].append(split[0])
        
def generate_vault():
    # todo: wipe vault
    for filename in graph:
        
        file = open("obsidianvault/" + filename + ".md", "w+")
        file.write("\n".join(["[[" + item + "]]" for item in graph[filename]]))
        file.write("\n")
        file.close()
        
def dfs(start, visited):
    for p in graph[start]:
        if p not in visited:
            visited[p] = ""
            dfs(p, visited)
        
def break_links(a, b, c):
    s = a.split("/")
    graph[s[0]].remove(s[1])
    graph[s[1]].remove(s[0])
    s = b.split("/")
    graph[s[0]].remove(s[1])
    graph[s[1]].remove(s[0])
    s = c.split("/")
    graph[s[0]].remove(s[1])
    graph[s[1]].remove(s[0])
    
    total = len(graph)
    dfs_found = {}
    dfs(s[0], dfs_found)
    part = len(dfs_found)
    
    print(part * (total - part))
   
#generate_vault()
break_links("fqr/bqp", "fhv/zsp", "hcd/cnr")