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
            Step(0);
        }
        
        private static void Step(int row)
        {

            if (row == lastRow)
            {
                // TODO zapisz wynik
                return;
            }
            
            // Find all possible column values for given row
            foreach (var column in availableColumns)
            {   
                // Add edge to matching, column is not available
                matchingEdges.Push(new Tuple<int, int>(row, column));
                availableColumns.Remove(column);
                
                Step(row+1);
                
                // Remove edge from matching, column is now available
                matchingEdges.Pop();
                availableColumns.Add(column);
            }
        }
        
    }
}