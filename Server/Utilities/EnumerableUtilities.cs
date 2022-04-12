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
            var sidelength = (int) Math.Sqrt(array.Length);
            for (int row = 0; row < sidelength; row++)
            {
                for (int col = 0; col < sidelength; col++)
                {
                    yield return array[col, row];
                }
            }
        }


        public static IEnumerable<IEnumerable<T>> ToSquareEnumerable(T[,] input)
        {
            int sideLength = (int) Math.Sqrt(input.Length);
            IEnumerable<T> getRow(int col)
            {
                for (int i = 0; i < sideLength; i++)
                {
                    yield return input[col, i];
                }
            }

            for (int i = 0; i < sideLength; i++)
            {
                yield return getRow(i);
            }
        }
    }
}
