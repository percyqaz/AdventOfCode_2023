var fs = require("fs");
let data = fs.readFileSync("input.txt", "utf-8").trim().split("\n");

var total = 0;
for (var i = 0; i < data.length; i++) {
	var splits = data[i].split(" ");
	var numbers = JSON.stringify(splits[1].split(",").map(x => parseInt(x)));
	var chars = splits[0];
	total += crack(chars, numbers);
}
console.log(total);

function crack(chars, numbers) {
	if (chars.includes('?')) {
		return crack(chars.replace('?', '.'), numbers) + crack(chars.replace('?', '#'), numbers);
	} else if (JSON.stringify(chars.split(".").filter(truthy => truthy).map(x => x.length)) == numbers) {
		return 1;
	} else {
		return 0;
	}
}