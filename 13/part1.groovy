def list = new File("input.txt").text.readLines()
def inputs = []
def temp = []
for (l in list) {
	if (l == "") {
		inputs.add(temp)
		temp = []
	} else {
		temp.add(l);
	}
}
inputs.add(temp)

def symmetry(inp) {
	outer_loop:
	for (int i = 0; i < inp.size() - 1; i++) {
		for (int j = 0; i + j + 1 < inp.size() && i - j >= 0; j++) {
			if (!inp[i + j + 1].equals(inp[i - j])) {
				continue outer_loop;
			}
		}
		return i + 1
	}
	return 0
}

def transpose(inp) {
	def result = []
	for (int i = 0; i < inp[0].length(); i++) {
		def line = ""
		for (int j = 0; j < inp.size(); j++) {
			line += inp[j][i]
		}
		result.add(line)
	}
	return result
}

total = 0

for (horizontal in inputs) {

	sym = symmetry(horizontal)
	total += 100 * sym
	
	def vertical = transpose(horizontal)
	
	sym = symmetry(vertical)
	total += sym
}

println total