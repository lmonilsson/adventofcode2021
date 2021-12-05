#!/usr/bin/env python3

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

class Line:
    def __init__(self, p1, p2):
        self.p1 = p1
        self.p2 = p2

    def x_range(self):
        if self.p1.x < self.p2.x:
            return range(self.p1.x, self.p2.x + 1)
        else:
            return range(self.p1.x, self.p2.x - 1, -1)

    def y_range(self):
        if self.p1.y < self.p2.y:
            return range(self.p1.y, self.p2.y + 1)
        else:
            return range(self.p1.y, self.p2.y - 1, -1)

def parse_line(line):
    xy1, xy2 = line.strip().split(' -> ')
    x1, y1 = xy1.split(',')
    x1, y1 = int(x1), int(y1)
    x2, y2 = xy2.split(',')
    x2, y2 = int(x2), int(y2)
    return Line(Point(x1, y1), Point(x2, y2))

def load_input():
    # lines = [
    #     '0,9 -> 5,9',
    #     '8,0 -> 0,8',
    #     '9,4 -> 3,4',
    #     '2,2 -> 2,1',
    #     '7,0 -> 7,4',
    #     '6,4 -> 2,0',
    #     '0,9 -> 2,9',
    #     '3,4 -> 1,4',
    #     '0,0 -> 8,8',
    #     '5,5 -> 8,2'
    # ]
    with open('input.txt') as f:
        lines = f.readlines()

    return list(map(parse_line, lines))

def part1():
    vent_lines = load_input()
    straight_lines = [line for line in vent_lines if line.p1.x == line.p2.x or line.p1.y == line.p2.y]
    counts = {}

    for line in straight_lines:
        if line.p1.x == line.p2.x:
            x = line.p1.x
            for y in line.y_range():
                counts[(x, y)] = counts.get((x, y), 0) + 1
        else:
            y = line.p1.y
            for x in line.x_range():
                counts[(x, y)] = counts.get((x, y), 0) + 1

    overlapping = len([x for x in counts.values() if x > 1])
    print('Part 1: {}'.format(overlapping))

def part2():
    vent_lines = load_input()
    counts = {}

    for line in vent_lines:
        if line.p1.x == line.p2.x:
            x = line.p1.x
            for y in line.y_range():
                counts[(x, y)] = counts.get((x, y), 0) + 1
        elif line.p1.y == line.p2.y:
            y = line.p1.y
            for x in line.x_range():
                counts[(x, y)] = counts.get((x, y), 0) + 1
        else:
            for x, y in zip(line.x_range(), line.y_range()):
                counts[(x, y)] = counts.get((x, y), 0) + 1

    overlapping = len([x for x in counts.values() if x > 1])
    print('Part 2: {}'.format(overlapping))


if __name__ == '__main__':
    part1()
    part2()
