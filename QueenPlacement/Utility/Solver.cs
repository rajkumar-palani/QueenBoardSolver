using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueenPlacement.Utility
{
    public interface IQueenSolver
    {
        void Execute(int square);
    }

    public class QueenSolver : IQueenSolver
    {
        public void Execute(int square)
        {
            var queenSolver = new Solver(square);

            var totalCount = 0;

            for (int i = 0; i < square; i++)
            {
                for (int j = 0; j < square; j++)
                {
                    totalCount++;

                    queenSolver.Reset();
                    queenSolver.SetValue(i, j);

                    queenSolver.Solve();

                    if (queenSolver.IsSolved())
                    {
                        queenSolver.WriteBoard();
                    }
                }
            }

            Console.WriteLine(totalCount);
            Console.ReadLine();
        }
    }

    public class Solver
    {
        public List<int[]> Board { get; private set; }

        public Solver(int boardSize)
        {
            if (boardSize <= 0)
                return;

            Board = new List<int[]>();

            for (int i = 0; i < boardSize; i++)
            {
                int[] curRow = new int[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    curRow[j] = 0;
                }

                Board.Add(curRow);
            }
        }

        public void SetValue(int row, int column)
        {
            if (Board == null)
                return;

            (Board[row])[column] = 1;
        }

        public void Reset()
        {
            if (Board == null)
                return;

            for (int i = 0; i < Board.Count(); i++)
            {
                int[] curRow = Board[i];
                for (int j = 0; j < curRow.Length; j++)
                {
                    curRow[j] = 0;
                }

                Board[i] = curRow;
            }
        }

        public void Solve()
        {
            //(Board[0])[0] = 1;

            List<IBoardSolver> boardSolverList = new List<IBoardSolver>();
            boardSolverList.Add(new RowSolver());
            boardSolverList.Add(new ColumnSolver());
            boardSolverList.Add(new DiagnolSolver());

            var rowIndex = 0;
            var colIndex = 0;
            var canPin = false;

            for (int i = 0; i < Board.Count; i++)
            {
                rowIndex = i;

                for (int j = 0; j < Board.Count; j++)
                {
                    colIndex = j;

                    if ((Board[i])[j] == 1)
                    {
                        continue;
                    }

                    canPin = false;
                    foreach (var boardSolver in boardSolverList)
                    {
                        // i is the List Index
                        canPin = boardSolver.Solve(Board, i, j);

                        if (!canPin)
                        {
                            break;
                        }
                    }

                    if (canPin)
                    {
                        var curArray = Board[i];
                        curArray[j] = 1;

                        Board[i] = curArray;
                    }
                }
            }
        }

        public bool IsSolved()
        {
            if (Board == null)
                return false;

            var totalCount = 0;

            for (int i = 0; i < Board.Count(); i++)
            {
                int[] curRow = Board[i];
                for (int j = 0; j < curRow.Length; j++)
                {
                    totalCount += curRow[j];
                }
            }

            return totalCount == Board.Count();
        }

        public void WriteBoard()
        {
            var strBuilder = new StringBuilder();
            foreach (var item in Board)
            {
                var curItem = item;

                for (int i = 0; i < curItem.Length; i++)
                {
                    if (i == 0)
                        strBuilder.Append(curItem[i]);
                    else
                        strBuilder.Append(" " + curItem[i]);
                }

                strBuilder.Append("\n");
            }

            Console.WriteLine(strBuilder.ToString());
        }
    }

    public interface IBoardSolver
    {
        bool Solve(List<int[]> board, int rowIndex, int columnIndex);
    }

    public class RowSolver : IBoardSolver
    {
        public bool Solve(List<int[]> board, int rowIndex, int columnIndex)
        {
            var boardSize = board.Count();

            var curRowArray = board[rowIndex];

            var canPinValue = true;

            for (int i = 0; i < curRowArray.Length; i++)
            {
                if (i == columnIndex)
                    continue;

                if (curRowArray[i] == 1)
                {
                    canPinValue = false;
                    break;
                }
            }

            return canPinValue;
        }
    }

    public class ColumnSolver : IBoardSolver
    {
        public bool Solve(List<int[]> board, int rowIndex, int columnIndex)
        {
            var boardSize = board.Count();

            var canPinValue = true;

            for (int i = 0; i < boardSize; i++)
            {
                if (i == rowIndex)
                    continue;

                var curRowArray = board[i];
                if (curRowArray[columnIndex] == 1)
                {
                    canPinValue = false;
                    break;
                }
            }

            return canPinValue;
        }
    }

    public class DiagnolSolver : IBoardSolver
    {
        public bool Solve(List<int[]> board, int rowIndex, int columnIndex)
        {
            var boardCount = board.Count();

            var isLeftUpCheck = IsValuePresentLeftUpDiagnol(board, rowIndex, columnIndex);
            var isLeftDownCheck = IsValuePresentLeftDownDiagnol(board, rowIndex, columnIndex);
            var isRightUpCheck = IsValuePresentRightUpDiagnol(board, rowIndex, columnIndex);
            var isRightDownCheck = IsValuePresentRightDownDiagnol(board, rowIndex, columnIndex);

            if (isLeftUpCheck && isLeftDownCheck && isRightUpCheck && isRightDownCheck)
                return true;
            else
                return false;
        }

        private bool IsValuePresentLeftUpDiagnol(List<int[]> board, int curRow, int curColumn)
        {
            var canPin = true;
            for (int i = curRow - 1; i >= 0; i--)
            {
                curColumn--;

                if (curColumn < 0)
                    break;

                if (i < 0)
                    break;

                var curArray = board[i];
                if (curArray[curColumn] == 1)
                {
                    canPin = false;
                }
            }

            return canPin;
        }

        private bool IsValuePresentLeftDownDiagnol(List<int[]> board, int curRow, int curColumn)
        {
            var canPin = true;
            for (int i = curRow + 1; i < board.Count; i++)
            {
                if (i >= board.Count)
                    break;

                curColumn--;

                if (curColumn < 0)
                    break;

                var curArray = board[i];
                if (curArray[curColumn] == 1)
                {
                    canPin = false;
                }
            }

            return canPin;
        }

        private bool IsValuePresentRightUpDiagnol(List<int[]> board, int curRow, int curColumn)
        {
            var canPin = true;
            for (int i = curRow - 1; i >= 0; i--)
            {
                if (i < 0)
                    break;

                curColumn++;

                if (curColumn >= board.Count)
                    break;

                var curArray = board[i];
                if (curArray[curColumn] == 1)
                {
                    canPin = false;
                }
            }

            return canPin;
        }

        private bool IsValuePresentRightDownDiagnol(List<int[]> board, int curRow, int curColumn)
        {
            var canPin = true;
            for (int i = curRow + 1; i < board.Count; i++)
            {
                if (i >= board.Count)
                    break;

                curColumn++;

                if (curColumn >= board.Count)
                    break;

                var curArray = board[i];
                if (curArray[curColumn] == 1)
                {
                    canPin = false;
                }
            }

            return canPin;
        }

    }
}
