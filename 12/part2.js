var fs = require("fs");
let data = fs.readFileSync("input.txt", "utf-8").trim().split("\n");

var total = 0;
for (var i = 0; i < data.length; i++) {
	const splits = data[i].split(" ");
	var numbers = splits[1].split(",").map(x => parseInt(x));
	numbers = numbers.concat(numbers).concat(numbers).concat(numbers).concat(numbers);
	const chars = splits[0] + "?" + splits[0] + "?" + splits[0] + "?" + splits[0] + "?" + splits[0];
	console.log(chars + " | " + numbers);
	const c = crack(chars, numbers);
	console.log(c);
	total += c;
}
console.log(total);

function permutations(numbers, spaces_reserved, index, existing, chars, memo) {
	
	const memo_val =  memo.get(index.toString() + "|" + existing.length.toString());
	if (memo_val !== undefined) {
		return memo_val;
	}
	
	const remaining = chars.length - existing.length
	const space_reserved = spaces_reserved[index];
	const number = numbers[index];
	var choices = 1 + (remaining - space_reserved) - number;
	var choices_lo = 0;
	
	const idx = chars.indexOf('#', existing.length) - existing.length;
	if (idx >= 0 && idx <= number) { // this hashtag Has to be part of the next group
		choices = Math.min(choices, idx + 1);
		choices_lo = Math.max(choices_lo, idx - number + 1);
	}
	var count = 0;
	
outer_loop:
	for (var i = choices_lo; i < choices; i++) {
		if (chars[existing.length + i] === '.') {
			continue;
		}
		var candidate = existing + ".".repeat(i) + "#".repeat(number);
		if (index + 1 >= numbers.length) {
			candidate += ".".repeat(chars.length - candidate.length);
		} else {
			candidate += ".";
		}
		for (var j = existing.length; j < candidate.length; j++) {
			if (candidate[j] === '.' && chars[j] === '#') {
				continue outer_loop;
			} else if (candidate[j] === '#' && chars[j] === '.') {
				continue outer_loop;
			}
		}
		if (index + 1 >= numbers.length) {
			count += 1;
		} else {
			count += permutations(numbers, spaces_reserved, index + 1, candidate, chars, memo);
		}
	}
	memo.set(index.toString() + "|" + existing.length.toString(), count);
	return count;
}

function crack(chars, numbers) {
	const spaces_reserved = [0];
	var t = -1;
	for (var i = numbers.length - 1; i >= 1; i--) {
		t += 1 + numbers[i];
		spaces_reserved.unshift(t);
	}
	
	var memo = new Map();
	
	return permutations(numbers, spaces_reserved, 0, "", chars, memo);
}