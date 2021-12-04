using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    public static class Program
    {
        const int BoardSize = 5;

        public static void Main()
        {
            Part1();
            Part2();
        }

        static (int[] Numbers, List<int[][]> Boards) LoadInput()
        {
            var lines = File.ReadLines("input.txt");

            var numbers = lines.First().Split(',').Select(int.Parse).ToArray();
            lines = lines.Skip(1);

            var boards = new List<int[][]>();
            while (lines.Any())
            {
                lines = lines.Skip(1);

                var boardLines = lines.Take(BoardSize).ToList();
                lines = lines.Skip(5);

                boards.Add(boardLines
                    .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse).ToArray())
                    .ToArray());
            }

            return (numbers, boards);
        }

        static void Part1()
        {
            var (numbers, boards) = LoadInput();
            var (winningBoard, lastNumber) = FindFirstWinningBoard(numbers, boards);

            var unmarkedSum = winningBoard
                .SelectMany(line => line)
                .Where(n => n != -1)
                .Sum();

            Console.WriteLine($"Part 1: {unmarkedSum * lastNumber}");
        }

        static void Part2()
        {
            var (numbers, boards) = LoadInput();
            var (winningBoard, lastNumber) = FindLastWinningBoard(numbers, boards);

            var unmarkedSum = winningBoard
                .SelectMany(line => line)
                .Where(n => n != -1)
                .Sum();

            Console.WriteLine($"Part 2: {unmarkedSum * lastNumber}");
        }

        static (int[][] Board, int LastNumber) FindFirstWinningBoard(int[] numbers, List<int[][]> boards)
        {
            foreach (var num in numbers)
            {
                BlotBoards(boards, num);

                var winningBoard = TryGetWinningBoard(boards);
                if (winningBoard != null)
                {
                    return (winningBoard, num);
                }
            }

            throw new Exception("No winner found");
        }

        static (int[][] Board, int LastNumber) FindLastWinningBoard(int[] numbers, List<int[][]> boards)
        {
            int[][] lastWinningBoard = null;
            int lastWinningNumber = -1;

            foreach (var num in numbers)
            {
                BlotBoards(boards, num);

                var winningBoard = TryGetWinningBoard(boards);
                while (winningBoard != null)
                {
                    lastWinningBoard = winningBoard;
                    lastWinningNumber = num;
                    boards.Remove(winningBoard);
                    winningBoard = TryGetWinningBoard(boards);
                }
            }

            return (lastWinningBoard, lastWinningNumber);
        }

        static void BlotBoards(List<int[][]> boards, int num)
        {
            foreach (var board in boards)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    for (int x = 0; x < BoardSize; x++)
                    {
                        if (board[y][x] == num)
                        {
                            board[y][x] = -1;
                        }
                    }
                }
            }
        }

        static int[][] TryGetWinningBoard(List<int[][]> boards)
        {
            foreach (var board in boards)
            {
                if (HasWon(board))
                {
                    return board;
                }
            }

            return null;
        }

        static bool HasWon(int[][] board)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                var count = 0;
                for (var x = 0; x < BoardSize; x++)
                {
                    if (board[y][x] == -1)
                    {
                        count++;
                    }
                }

                if (count == BoardSize)
                {
                    return true;
                }
            }

            for (var x = 0; x < BoardSize; x++)
            {
                var count = 0;
                for (var y = 0; y < BoardSize; y++)
                {
                    if (board[y][x] == -1)
                    {
                        count++;
                    }
                }

                if (count == BoardSize)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
