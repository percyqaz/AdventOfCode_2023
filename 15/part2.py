import functools;hash=lambda w:functools.reduce(lambda a,b:((a+b)*17)%256,[ord(c)for c in w],0);boxes=[[]for i in range(256)];eq=lambda x:boxes[hash(x[0])].append(x)if x[0]not in[y[0]for y in boxes[hash(x[0])]]else[(y.pop(1),y.append(x[1]))if y[0]==x[0]else None for y in boxes[hash(x[0])]];ms=lambda x:[[box.remove(y)if y[0]==x and len(box)==l else 0for y in box]for box,l in[(boxes[hash(x)],len(boxes[hash(x)]))]];[eq(c.split("="))if"="in c else ms(c.replace("-",""))for c in open("input.txt").read().strip().split(",")];print(sum([sum([(i+1)*(j+1)*int(l[1])for j,l in enumerate(box)])for i,box in enumerate(boxes)]))