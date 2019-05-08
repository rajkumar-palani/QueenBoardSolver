using QueenPlacement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueenPlacement
{
    class Program
    {
        static void Main(string[] args)
        {
            //8x8 Queen Problem.
            IQueenSolver queenSolver = new QueenSolver();
            queenSolver.Execute(8);
        }
    }
}
