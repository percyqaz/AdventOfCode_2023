const std = @import("std");
const data = @embedFile("input.txt");
const split = std.mem.split;

pub fn main() !void {
    var splits = split(u8, data, "\n");
    var lines: [100][100]u8 = undefined;
    var y: u8 = 0;
    while (splits.next()) |line| {
        var x: u8 = 0;
        for (line) |char| {
            lines[x][y] = char;
            x = x + 1;
        }
        y = y + 1;
    }
    var s: u64 = 0;
    for (lines) |line| {
        var p: u64 = 0;
        for (0.., line) |i, char| {
            if (char == 'O') {
                s = s + (100 - p);
                p = p + 1;
            } else if (char == '#') {
                p = i + 1;
            }
        }
    }
    std.debug.print("{}\n", .{s});
}