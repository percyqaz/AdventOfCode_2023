const std = @import("std");
const data = @embedFile("input.txt");
const split = std.mem.split;

fn load(lines: [100][100]u8) u64 {
    var s: u64 = 0;
    for (lines) |line| {
        for (0.., line) |i, char| {
            if (char == 'O') {
                s = s + (100 - i);
            }
        }
    }
    return s;
}

fn north(l: [100][100]u8) [100][100]u8 {
    var lines = l;
    for (0..100) |x| {
        var p: u64 = 0;
        for (0..100) |y| {
            if (lines[x][y] == 'O') {
                lines[x][y] = '.';
                lines[x][p] = 'O';
                p = p + 1;
            } else if (lines[x][y] == '#') {
                p = y + 1;
            }
        }
    }
    return lines;
}

fn west(l: [100][100]u8) [100][100]u8 {
    var lines = l;
    for (0..100) |y| {
        var p: u64 = 0;
        for (0..100) |x| {
            if (lines[x][y] == 'O') {
                lines[x][y] = '.';
                lines[p][y] = 'O';
                p = p + 1;
            } else if (lines[x][y] == '#') {
                p = x + 1;
            }
        }
    }
    return lines;
}

fn south(l: [100][100]u8) [100][100]u8 {
    var lines = l;
    var x: i64 = 0;
    while (x < 100) : (x += 1) {
        var y: i64 = 99;
        var p: i64 = 99;
        while (y > -1) : (y -= 1) {
            if (lines[@bitCast(x)][@bitCast(y)] == 'O') {
                lines[@bitCast(x)][@bitCast(y)] = '.';
                lines[@bitCast(x)][@bitCast(p)] = 'O';
                p = p - 1;
            } else if (lines[@bitCast(x)][@bitCast(y)] == '#') {
                p = y - 1;
            }
        }
    }
    return lines;
}

fn east(l: [100][100]u8) [100][100]u8 {
    var lines = l;
    var y: i64 = 0;
    while (y < 100) : (y += 1) {
        var x: i64 = 99;
        var p: i64 = 99;
        while (x >= 0) : (x -= 1) {
            if (lines[@bitCast(x)][@bitCast(y)] == 'O') {
                lines[@bitCast(x)][@bitCast(y)] = '.';
                lines[@bitCast(p)][@bitCast(y)] = 'O';
                p = p - 1;
            } else if (lines[@bitCast(x)][@bitCast(y)] == '#') {
                p = x - 1;
            }
        }
    }
    return lines;
}

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
        for (lines) |line| {
            std.debug.print("{s}\n", .{line});
        }
        for (0..1000) |i| {
            lines = north(lines);
            lines = west(lines);
            lines = south(lines);
            lines = east(lines);
            std.debug.print("{}: {}\n", .{i + 1, load(lines)});
        }
}