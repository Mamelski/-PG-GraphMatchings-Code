using System;
using System.Collections.Generic;
using System.Globalization;

namespace GraphMatchings.Core
{
    public static class BruteForceMatchingAlgorithm
    {
        private static int lastRow;
        private static List<int> availableRows;
        private static List<int> availableColumns;

        private static Stack<Tuple<int, int>> matchingEdges;
        
        public static void Run()
        {
            
        }
        
        private static void Step(int currentRow)
        {

            if (currentRow == lastRow)
            {
                return;
            }
            
            // 
            foreach (var column in availableColumns)
            {   
                matchingEdges.Push(new Tuple<int, int>(currentRow, column));
                Step(currentRow+1);
                matchingEdges.Pop();
            }
        }
        
    }
}