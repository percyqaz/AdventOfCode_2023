star_map = Array.new

File.readlines('input.txt', chomp: true).each do |line|
    star_map.append(line)
end

stars = Array.new

for y in 0..(star_map.length - 1) do
	for x in 0..(star_map[0].length - 1) do
		if (star_map[y][x] == '#')
			stars.append([x, y])
		end
	end
end

expand_y = Array.new
for y in 0..(star_map.length - 1) do
	if (!(stars.any? { |x, y2| y2 == y }))
		expand_y.append(y)
	end
end

expand_x = Array.new
for x in 0..(star_map[0].length - 1) do
	if (!(stars.any? { |x2, y| x2 == x }))
		expand_x.append(x)
	end
end

stars = stars.map { |x, y| [x + expand_x.count { |x2| x2 < x }, y + expand_y.count { |y2| y2 < y }]}

total = 0
for i in 0..(stars.length - 1) do
	for j in (i + 1)..(stars.length - 1) do
		x1, y1 = stars[i]
		x2, y2 = stars[j]
		total = total + (x1 - x2).abs + (y1 - y2).abs
	end
end

puts total