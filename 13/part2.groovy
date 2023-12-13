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
	for (int i = 0; i < inp.size() - 1; i++) { // test if i is a line of sym
		def differences = 0
		
		for (int j = 0; i + j + 1 < inp.size() && i - j >= 0; j++) { // check other lines
			for (int k = 0; k < inp[0].length(); k++) { // count differences
				if (inp[i + j + 1][k] != (inp[i - j][k])) {
					differences++
					if (differences > 1) {
						continue outer_loop
					}
				}
			}
		}
		if (differences == 1) {
			return i + 1
		}
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