var lines = File.ReadAllLines("input.txt");

var movements = lines[0];

public readonly struct Node
{
	public readonly byte a;
	public readonly byte b;
	public readonly byte c;
	
    public Node(string name)
    {
        a = (byte)(int)name[0];
        b = (byte)(int)name[1];
        c = (byte)(int)name[2];
    }
	
	public override bool Equals(object obj) => obj is Node other && this.Equals(other);

	public bool Equals(Node p) => a == p.a && b == p.b && c == p.c;

	public override int GetHashCode() => (a, b, c).GetHashCode();

	public static bool operator ==(Node lhs, Node rhs) => lhs.Equals(rhs);

	public static bool operator !=(Node lhs, Node rhs) => !(lhs == rhs);
	
	public override string ToString() => ((char)(int)a).ToString() + ((char)(int)b).ToString() + ((char)(int)c).ToString();
}

var left = new Dictionary<Node, Node>();
var right = new Dictionary<Node, Node>();

var currents = new List<Node>();
var A = (byte)(int)'A';
var Z = (byte)(int)'Z';

for (int i = 2; i < lines.Length; i++) {
	var node = new Node(lines[i].Substring(0, 3));
	var l = new Node(lines[i].Substring(7, 3));
	var r = new Node(lines[i].Substring(12, 3));
	left[node] = l;
	right[node] = r;
	
	if (node.c == A) 
	{ 
		currents.Add(node);
	}
}

foreach (var _c in currents) {
	var current = _c;
	var i = 0;
	var steps = 0;
	var seen = new Dictionary<Node, (int, int)>();

	while (!seen.ContainsKey(current) || seen[current].Item1 != i)
	{
		seen[current] = (i, steps);
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
	
	Console.WriteLine(current.ToString() + " at step " + steps.ToString() + ", last seen at step " + seen[current].Item2.ToString());
	Console.WriteLine("That's a period of " + (steps - seen[current].Item2).ToString());
}