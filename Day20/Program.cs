using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day20
{
    class Tile
    {
        public int Id { get; set; }

        public List<int> Sides { get; set; }

        public bool[,] Data { get; set; }

        public List<Tile> Neighbours { get; set; }

        public bool InGrid { get; set; }

        public bool HasPattern(bool[,] pattern, int x, int y)
        {
            var sizeX = pattern.GetLength(0);
            var sizeY = pattern.GetLength(1);

            for (int patternX = 0; patternX < sizeX; patternX++)
            {
                for (int patternY = 0; patternY < sizeY; patternY++)
                {
                    if (pattern[patternX, patternY] && !Data[x + patternX, y + patternY])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int CountPattern(bool[,] pattern)
        {
            var maxX = Data.GetLength(0) - pattern.GetLength(0);
            var maxY = Data.GetLength(1) - pattern.GetLength(1);

            var result = 0;

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (HasPattern(pattern, x, y))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public Tile RemoveBorder()
        {
            var sizeX = Data.GetLength(0) - 2;
            var sizeY = Data.GetLength(1) - 2;

            var data = new bool[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[x, y] = Data[x + 1, y + 1];
                }
            }

            var result = MemberwiseClone() as Tile;
            result.Data = data;

            return result;
        }

        public Tile Flip()
        {
            var sizeX = Data.GetLength(0);
            var sizeY = Data.GetLength(1);

            var data = new bool[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[x, y] = Data[sizeX - x - 1, y];
                }
            }

            var result = MemberwiseClone() as Tile;
            result.Data = data;

            return result;
        }

        public Tile RotateLeft()
        {
            var sizeX = Data.GetLength(1);
            var sizeY = Data.GetLength(0);

            var data = new bool[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[x, y] = Data[sizeY - y - 1, x];
                }
            }

            var result = MemberwiseClone() as Tile;
            result.Data = data;

            return result;
        }

        public int CalculateTopValue()
        {
            var sizeX = Data.GetLength(0);

            var bitString = new StringBuilder(sizeX);

            for (int x = 0; x < sizeX; x++)
            {
                bitString.Append(Data[x, 0] ? "1" : "0");
            }

            return Convert.ToInt32(bitString.ToString(), 2);
        }

        public int CalculateRightValue()
        {
            return RotateLeft().CalculateTopValue();
        }

        public int CalculateBottomValue()
        {
            return RotateLeft().RotateLeft().CalculateTopValue();
        }

        public int CalculateLeftValue()
        {
            return RotateLeft().RotateLeft().RotateLeft().CalculateTopValue();
        }

        public Tile Orientate(Tile top, Tile right, Tile bottom, Tile left)
        {
            foreach (var tile in Orientations())
            {
                if (top != null && !top.Sides.Contains(tile.CalculateTopValue()))
                {
                    continue;
                }

                if (right != null && !right.Sides.Contains(tile.CalculateRightValue()))
                {
                    continue;
                }

                if (bottom != null && !bottom.Sides.Contains(tile.CalculateBottomValue()))
                {
                    continue;
                }

                if (left != null && !left.Sides.Contains(tile.CalculateLeftValue()))
                {
                    continue;
                }

                return tile;
            }

            return null;
        }

        public IEnumerable<Tile> Orientations()
        {
            var tile = this;

            for (int i = 0; i < 4; i++)
            {
                yield return tile;
                yield return tile.Flip();

                tile = tile.RotateLeft();
            }
        }

        public void CalculateSides()
        {
            Sides = Orientations().Select(t => t.CalculateTopValue()).ToList();
        }

        public void FindNeighbours(List<Tile> tiles)
        {
            Neighbours = tiles.Where(t => t != this && Sides.Any(s => t.Sides.Contains(s))).ToList();
        }

        public void Print()
        {
            Console.WriteLine($"Tile {Id}:");

            var sizeX = Data.GetLength(0);
            var sizeY = Data.GetLength(1);

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Console.Write(Data[x, y] ? '#' : '.');

                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static Tile Parse(List<string> tile)
        {
            var headerParts = tile[0].Split(' ', ':');
            var id = int.Parse(headerParts[1]);

            var sizeX = tile[1].Length;
            var sizeY = tile.Count - 1;

            var data = new bool[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[x, y] = tile[y + 1][x] == '#';
                }
            }

            var result = new Tile
            {
                Id = id,
                Data = data
            };

            result.CalculateSides();

            return result;
        }
    }

    class Program
    {
        public static List<List<string>> GroupInput(string[] input)
        {
            var result = new List<List<string>>();

            var tile = new List<string>();

            foreach (var line in input)
            {
                if (line == "")
                {
                    result.Add(tile);
                    tile = new List<string>();
                    continue;
                }

                tile.Add(line);
            }

            if (tile.Count > 0)
            {
                result.Add(tile);
            }

            return result;
        }

        static long Multiply(List<long> values)
        {
            return values.Aggregate((a, b) => a * b);
        }

        static bool InGrid(Tile[,] grid, int x, int y)
        {
            var sizeX = grid.GetLength(0);
            var sizeY = grid.GetLength(1);

            return (0 <= x && x < sizeX && 0 <= y && y < sizeY);
        }

        static Tile GetTile(Tile[,] grid, int x, int y)
        {
            if (InGrid(grid, x, y))
            {
                return grid[x, y];
            }

            return null;
        }

        static int CountNeighbours(Tile[,] grid, int x, int y)
        {
            var neighbours = new List<(int x, int y)>
            {
                (x - 1, y),
                (x + 1, y),
                (x, y - 1),
                (x, y + 1)
            };

            return neighbours.Count(nb => InGrid(grid, nb.x, nb.y));
        }

        static Tile[,] FillGrid(List<Tile> tiles)
        {
            var corners = tiles.Where(t => t.Neighbours.Count == 2).ToList();
            var sides = tiles.Where(t => t.Neighbours.Count == 3).ToList();

            var size = sides.Count / 4 + corners.Count / 2;
            var grid = new Tile[size, size];

            // top row
            grid[0, 0] = corners[0];
            grid[0, 0].InGrid = true;

            for (int x = 1; x < size; x++)
            {
                var neighbourCount = CountNeighbours(grid, x, 0);

                grid[x, 0] = tiles.First(t => !t.InGrid && t.Neighbours.Count == neighbourCount && t.Neighbours.Contains(grid[x - 1, 0]));
                grid[x, 0].InGrid = true;
            }

            // remaining rows
            for (int y = 1; y < size; y++)
            {
                var neighbourCount = CountNeighbours(grid, 0, y);

                grid[0, y] = tiles.First(t => !t.InGrid && t.Neighbours.Count == neighbourCount && t.Neighbours.Contains(grid[0, y - 1]));
                grid[0, y].InGrid = true;

                for (int x = 1; x < size; x++)
                {
                    neighbourCount = CountNeighbours(grid, x, y);

                    grid[x, y] = tiles.First(t => !t.InGrid && t.Neighbours.Count == neighbourCount && t.Neighbours.Contains(grid[x - 1, y]) && t.Neighbours.Contains(grid[x, y - 1]));
                    grid[x, y].InGrid = true;
                }
            }

            // fix orientation
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var left = GetTile(grid, x - 1, y);
                    var right = GetTile(grid, x + 1, y);
                    var top = GetTile(grid, x, y - 1);
                    var bottom = GetTile(grid, x, y + 1);

                    grid[x, y] = grid[x, y].Orientate(top, right, bottom, left);
                }
            }

            return grid;
        }

        static Tile Combine(Tile[,] grid)
        {
            var gridSizeX = grid.GetLength(0);
            var gridSizeY = grid.GetLength(1);

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    grid[x, y] = grid[x, y].RemoveBorder();
                }
            }

            var tileSizeX = grid[0, 0].Data.GetLength(0);
            var tileSizeY = grid[0, 0].Data.GetLength(1);

            var dataSizeX = gridSizeX * tileSizeX;
            var dataSizeY = gridSizeY * tileSizeY;

            var data = new bool[dataSizeX, dataSizeY];

            for (int x = 0; x < dataSizeX; x++)
            {
                for (int y = 0; y < dataSizeY; y++)
                {
                    var gridX = x / tileSizeX;
                    var gridY = y / tileSizeY;

                    var tileX = x % tileSizeX;
                    var tileY = y % tileSizeY;

                    data[x, y] = grid[gridX, gridY].Data[tileX, tileY];
                }
            }

            return new Tile
            {
                Data = data
            };
        }

        static bool[,] ParsePattern(string[] raw)
        {
            var sizeX = raw[0].Length;
            var sizeY = raw.Length;

            var data = new bool[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[x, y] = raw[y][x] == '#';
                }
            }

            return data;
        }

        static int CountTrue(bool[,] data)
        {
            return data.Cast<bool>().Count(x => x);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var grouped = GroupInput(input);

            var tiles = grouped.Select(Tile.Parse).ToList();

            foreach (var tile in tiles)
            {
                tile.FindNeighbours(tiles);
            }

            var corners = tiles.Where(t => t.Neighbours.Count == 2).ToList();

            var answer1 = Multiply(corners.Select(t => (long)t.Id).ToList());

            Console.WriteLine($"Answer 1: {answer1}");


            var grid = FillGrid(tiles);
            var combined = Combine(grid);

            var patternRaw = new[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };
            
            var pattern = ParsePattern(patternRaw);

            var monsters = 0;

            foreach (var orientation in combined.Orientations())
            {
                monsters = orientation.CountPattern(pattern);

                if (monsters > 0)
                {
                    break;
                }
            }

            var answer2 = CountTrue(combined.Data) - monsters * CountTrue(pattern);

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
