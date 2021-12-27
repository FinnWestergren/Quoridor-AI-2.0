using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Utilities
{
    public static class EnumerableUtilities<T>
    {

        public static T[,] To2DArray(IEnumerable<T> input, int rowLength)
        {
            var colLength = input.Count() / rowLength;
            T[,] output = new T[rowLength, colLength];
            var i = 0;
            foreach (var v in input)
            {
                var col = i / colLength;
                var row = i % rowLength;
                output[row, col] = v;
                i++;
            }
            return output;
        }

        public static IEnumerable<T> From2DArray(T[,] array)
        {
            foreach (var v in array)
            {
                yield return v;
            }
        }
    }
}
