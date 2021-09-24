using System;
using System.Collections.Generic;
using System.Linq;

namespace PurpleVsBrowns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to 'Purple VS Browns'");
            Console.WriteLine("Please enter the board dimensions (e.g. 1,1)");
            string boardDimensions = Console.ReadLine();
            int x = int.Parse(boardDimensions.Split(',')[0]);
            int y = int.Parse(boardDimensions.Split(',')[0]);

            var board = new Board(x, y);

            IList<string> generationZero = GetGenerationZeroInput(board);

            board.SetGenerationZero(Board.GetGridFromCollectionOfStrings(generationZero));

            Console.WriteLine("Please enter the cell and the number of generations to cound for the cell been Purple (e.g. 1,1,10)");
            var parameters = Console.ReadLine().Split(',');
            var col = int.Parse(parameters[0]);
            var row = int.Parse(parameters[1]);
            var generations = int.Parse(parameters[2]);


            var countOfPurples = 0;
            var cellCurrentValue = board.GetCellValue(col, row);

            for (int i = 0; i < generations; i++)
            {
                board.GenerateNewGeneration();

                var cellValue = board.GetCellValue(col, row);
                // is purple
                if (cellValue == cellCurrentValue)
                    countOfPurples += 1;
            }

            Console.WriteLine("The result is " + countOfPurples);
        }

        private static IList<string> GetGenerationZeroInput(Board board)
        {
            var result = new List<string>();
            for (int i = 0; i < board.Y; i++)
            {
                Console.WriteLine("Please enter the zero generation for row number:" + i);
                string boardRow = Console.ReadLine();
                result.Add(boardRow);
            }

            return result;
        }
    }

    public class Board
    {
        int[,] _grid;
        int generationsCount = 0;

        public Board(int x, int y)
        {
            if (x < 1 || y < 1)
                throw new ArgumentException("Board X and Y can only be numbers bigger than 1!");
            if (x > y)
                throw new ArgumentException("The X dimension on the board cannot be bigger than the Y!");
            if (y >= 1000)
                throw new ArgumentException("The Y dimension on the board should be smaller than 1000!");

            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int GetCellValue(int x, int y)
        {
            return _grid[x, y];
        }

        public void SetGenerationZero(int[,] zeroGrid)
        {
            // validate generation zero if correct is the same dimmension, has correct characters
            _grid = zeroGrid;
        }

        public void GenerateNewGeneration()
        {
            var newGrid = ApplyGenerationForBrowns(_grid);
            _grid = ApplyGenerationRuleForPurples(newGrid);

            generationsCount += 1;
        }

        int[,] ApplyGenerationForBrowns(int[,] sourceGrid)
        {
            // clone otherwise this will be by reference. If by refenence you will endup changing the original object instead of just the variable
            int[,] newGrid = sourceGrid.Clone() as int[,];
            for (int row = 0; row < Y; row++)
            {
                for (int col = 0; col < X; col++)
                {
                    var cel = _grid[row, col];
                    
                    //Is Brown
                    if(cel == 0)
                    {
                        var surroundingPurples = GetNumberOfPurplesAroundCell(_grid,row, col);

                        if (surroundingPurples == 3 || surroundingPurples == 6)
                        {
                            newGrid[row, col] = 1;
                        }
                    }
                }
            }

            return newGrid;
        }

        int[,] ApplyGenerationRuleForPurples(int[,] sourceGrid)
        {
            // clone otherwise this will be by reference. If by refenence you will endup changing the original object instead of just the variable
            int[,] newGrid = sourceGrid.Clone() as int[,];
            for (int row = 0; row < Y; row++)
            {
                for (int col = 0; col < X; col++)
                {
                    var cel = _grid[row, col];

                    //Is Purple
                    if (cel == 1)
                    {
                        var surroundingPurples = GetNumberOfBrownsAroundCell(_grid, row, col);

                        if (surroundingPurples == 0 || surroundingPurples == 1 || 
                            surroundingPurples == 4 || surroundingPurples == 5 || surroundingPurples == 7 || 
                            surroundingPurples == 8)
                        {
                            newGrid[row, col] = 0;
                        }
                    }
                }
            }

            return newGrid;
        }

        static int GetNumberOfPurplesAroundCell(int[,] sourceGrid,int x, int y)
        {
            var hasTop = x > 0;
            var hasLeft = y > 0;
            var hasBottom = x < sourceGrid.GetLength(0) - 1;
            var hasRight = y < sourceGrid.GetLength(1) - 1;

            var topLeftCount = hasTop && hasLeft ? sourceGrid[x - 1, y - 1] == 1 ? 1 : 0 : 0;
            var topRightCount = hasTop && hasRight ? sourceGrid[x - 1, y + 1] == 1 ? 1 : 0 : 0;
            var topCenterCount = hasTop ? sourceGrid[x - 1, y] == 1 ? 1 : 0 : 0;

            var centerLeftCount = hasLeft ? sourceGrid[x, y - 1] == 1 ? 1 : 0 : 0;
            var centerRightCount = hasRight ? sourceGrid[x, y + 1] == 1 ? 1 : 0 : 0;

            var bottomLeftCount = hasLeft && hasBottom ? sourceGrid[x + 1, y - 1] == 1 ? 1 : 0 : 0;
            var bottomCenterCount = hasBottom ? sourceGrid[x + 1, y] == 1 ? 1 : 0 : 0;
            var bottomRightCount = hasRight && hasBottom ? sourceGrid[x + 1, y + 1] == 1 ? 1 : 0 : 0;

            return topLeftCount + topRightCount + topCenterCount + centerLeftCount + centerRightCount + bottomLeftCount + bottomCenterCount + bottomRightCount;
        }

        static int GetNumberOfBrownsAroundCell(int[,] sourceGrid,int x, int y)
        {
            var hasTop = x > 0;
            var hasLeft = y > 0;
            var hasBottom = x < sourceGrid.GetLength(0) - 1;
            var hasRight = y < sourceGrid.GetLength(1) - 1;

            var topLeftCount = hasTop && hasLeft ? sourceGrid[x-1,y-1] == 0 ? 1 : 0 : 0;
            var topRightCount = hasTop && hasRight ? sourceGrid[x - 1, y + 1] == 0 ? 1 : 0 : 0;
            var topCenterCount = hasTop ? sourceGrid[x - 1, y ] == 0 ? 1 : 0 : 0;

            var centerLeftCount = hasLeft ? sourceGrid[x, y -1 ] == 0 ? 1 : 0 : 0;
            var centerRightCount = hasRight ? sourceGrid[x, y + 1] == 0 ? 1 : 0 : 0;

            var bottomLeftCount = hasLeft && hasBottom ? sourceGrid[x + 1, y - 1] == 0 ? 1 : 0 : 0;
            var bottomCenterCount = hasBottom ? sourceGrid[x + 1, y] == 0 ? 1 : 0 : 0;
            var bottomRightCount = hasRight && hasBottom ? sourceGrid[x + 1, y + 1] == 0 ? 1 : 0 : 0;

            return topLeftCount + topRightCount + topCenterCount + centerLeftCount + centerRightCount + bottomLeftCount + bottomCenterCount + bottomRightCount;
        }

        public static int[,] GetGridFromCollectionOfStrings(IList<string> rowsOfGridAsStrings)
        {
            var rows = rowsOfGridAsStrings.Count();
            var cols = rowsOfGridAsStrings.Count();
            var result = new int[rows, cols];

            for (int row = 0; row < rowsOfGridAsStrings.Count(); row++)
            {
                for (int col = 0; col < rowsOfGridAsStrings[row].Count(); col++)
                {
                    string cell = rowsOfGridAsStrings[row][col].ToString();
                    
                    if(int.TryParse(cell,out int cellValue))
                    {
                        //validation here whether it is Brown or Purple if not error the grid has invalid characters

                        result[row,col] = cellValue;
                    }
                    else
                    {
                        //error the grid has invalid characters
                    }
                }
            }

            return result;
        }
    }
}
