﻿namespace GraphMatchings.Core.GraphRepresentation
{
    public class Edge
    {
        public Edge(int from, int to)
        {
            From = from;
            To = to;
        }

        public int From { get; set; }

        public int To { get; set; }
    }
}