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
}

func (t *Tile) Remove() {
	if (t.Value != 'S') {
		t.Value = '.'
	}
}

func (t *Tile) Update(distance int) {
	if (t.Distance > distance) {
		t.Distance = distance
		fmt.Printf("%A\n", distance)
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
	
	for y := 0; y < len(lines); y++ {
		for x := 0; x < len(lines[y]); x++ {
			c := lines[y][x]
			grid[y][x] = Tile{c, 99999}
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

func display(grid [][]Tile) (result string) {
	result = ""
	
	w := len(grid[0])
	h := len(grid)
	
	for y := 0; y < h; y++ {
		for x := 0; x < w; x++ {
			tile := grid[y][x]
			if tile.Distance < 99999 {
				result = result + "*"
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
	
	for {
		pass(grid)
		
		//fmt.Printf("%s", display(grid))
		
		//reader := bufio.NewReader(os.Stdin)
		//reader.ReadString('\n')
	}
}