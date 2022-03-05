using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Utilities
{
    public static class EnumerableUtilities<T>
    {

        public static T[,] ToSquareArray(IEnumerable<T> input, int sideLength)
        {
            T[,] output = new T[sideLength, sideLength];
            var i = 0;
            foreach (T v in input)
            {
                var col = i % sideLength;
                var row = i / sideLength;
                output[col, row] = v;
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


        public static IEnumerable<IEnumerable<T>> ToSquareEnumerable(T[,] input)
        {
            int sideLength = (int) Math.Sqrt(input.Length);
            IEnumerable<T> getRow(int row)
            {
                for (int i = 0; i < sideLength; i++)
                {
                    yield return input[i, row];
                }
            }

            for (int i = 0; i < sideLength; i++)
            {
                yield return getRow(i);
            }
        }
    }
}
