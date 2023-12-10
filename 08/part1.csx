var lines = File.ReadAllLines("input.txt");

var movements = lines[0];

var left = new Dictionary<string, string>();
var right = new Dictionary<string, string>();

for (int i = 2; i < lines.Length; i++) {
	var node = lines[i].Substring(0, 3);
	var l = lines[i].Substring(7, 3);
	var r = lines[i].Substring(12, 3);
	left[node] = l;
	right[node] = r;
}

var current = "AAA";
var i = 0;
var steps = 0;
while (current != "ZZZ")
{
	if (movements[i] == 'L')
	{
		current = left[current];
	}
	else
	{
		current = right[current];
	}
	i = (i + 1) % movements.Length;
	steps = steps + 1;
}

Console.WriteLine(steps);