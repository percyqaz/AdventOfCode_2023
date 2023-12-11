package main

import (
    "bufio"
    "fmt"
    "os"
)

type Position struct {
	X, Y int
}

type Tile struct {
	Value byte
	Distance int
	Outside bool
}

func (t *Tile) Remove() {
	if (t.Value != 'S') {
		t.Value = '.'
	}
}

func (t *Tile) Update(distance int) {
	if (t.Distance > distance) {
		t.Distance = distance
	}
}

func (t *Tile) Up() bool {
	return (t.Value == '|' || t.Value == 'J' || t.Value == 'L' || t.Value == 'S')
}

func (t *Tile) Down() bool {
	return (t.Value == '|' || t.Value == 'F' || t.Value == '7' || t.Value == 'S')
}

func (t *Tile) Left() bool {
	return (t.Value == '-' || t.Value == 'J' || t.Value == '7' || t.Value == 'S')
}

func (t *Tile) Right() bool {
	return (t.Value == '-' || t.Value == 'F' || t.Value == 'L' || t.Value == 'S')
}

func min(a int, b int) int {
	if a < b {
		return a
	} else {
		return b
	}
}

func load_lines() (lines []string) {
	lines = []string{}
	
    file, _ := os.Open("input.txt")
    defer file.Close()
	
    scanner := bufio.NewScanner(file)
    for scanner.Scan() {
        lines = append(lines, scanner.Text())
    }
	return
}

func load_grid(lines []string) (grid [][]Tile, start Position) {
	
	grid = make([][]Tile, len(lines))
	for y := range grid {
		grid[y] = make([]Tile, len(lines[0]))
	}
	
	w := len(lines[0])
	h := len(lines)
	
	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			c := lines[y][x]
			grid[y][x] = Tile{c, 99999, x == 0 || y == 0 || x + 1 == w || y + 1 == h}
			if c == 'S' {
				start = Position{x, y}
				grid[y][x].Distance = 0
			}
		}
	}
	
	return
}

func pass(grid [][]Tile) {
	w := len(grid[0])
	h := len(grid)
	
	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			tile := grid[y][x]
			if tile.Up() {
				if y == 0 || !grid[y - 1][x].Down() {
					grid[y][x].Remove()
				} else {
					grid[y - 1][x].Update(tile.Distance + 1)
				}
			}
			if tile.Left() {
				if x == 0 || !grid[y][x - 1].Right() {
					grid[y][x].Remove()
				} else {
					grid[y][x - 1].Update(tile.Distance + 1)
				}
			}
			if tile.Down() {
				if y + 1 == h || !grid[y + 1][x].Up() {
					grid[y][x].Remove()
				} else {
					grid[y + 1][x].Update(tile.Distance + 1)
				}
			}
			if tile.Right() {
				if x + 1 == h || !grid[y][x + 1].Left() {
					grid[y][x].Remove()
				} else {
					grid[y][x + 1].Update(tile.Distance + 1)
				}
			}
		}
	}
}

func remove_small_pipe_circuits(grid [][]Tile) {
	w := len(grid[0])
	h := len(grid)
	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			if grid[y][x].Distance == 99999 {
				grid[y][x].Remove()
			}
		}
	}
}

func pass2(grid [][]Tile) {
	w := len(grid[0])
	h := len(grid)
	
	for y := 1; y < h - 1; y++ {
		for x := 1; x < w - 1; x++ {
			tile := grid[y][x]
			if !tile.Outside {
				if grid[y][x - 1].Outside && !grid[y][x - 1].Up() {
					grid[y][x].Outside = true
				}
				if grid[y][x + 1].Outside && !grid[y][x].Up() {
					grid[y][x].Outside = true
				}
				if grid[y - 1][x].Outside && !grid[y - 1][x].Left() {
					grid[y][x].Outside = true
				}
				if grid[y + 1][x].Outside && !grid[y][x].Left() {
					grid[y][x].Outside = true
				}
			}
		}
	}
}

func count_inside(grid [][]Tile) (count int) {
	w := len(grid[0])
	h := len(grid)
	
	count = 0

	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			if !grid[y][x].Outside && grid[y][x].Value == '.' {
				count++
			}
		}
	}
	
	return
}

func display(grid [][]Tile) (result string) {
	result = ""
	
	w := len(grid[0])
	h := len(grid)
	
	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			tile := grid[y][x]
			if tile.Value == '.' && tile.Outside {
				result = result + "O"
			} else {
				result = result + string(rune(tile.Value))
			}
		}
		result = result + "\n"
	}
	
	return
}

func main() {

	lines := load_lines()
	grid, _ := load_grid(lines)
	
	for i := 0; i < 6927; i++ {
		pass(grid)
	}
	
	remove_small_pipe_circuits(grid)
	
	for i := 0; i < 10000; i++ {
		pass2(grid)
	}
	
	fmt.Printf("%s", display(grid))
	fmt.Printf("There are %i cells inside", count_inside(grid))
}