using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Utilities
{
    public static class EnumerableUtilities
    {

        public static T[,] ToSquareArray<T>(IEnumerable<T> input)
        {
            int sideLength = GetSquareRoot(input.Count());
            T[,] output = new T[sideLength, sideLength];
            for (var i = 0; i < input.Count(); i++)
            {
                var col = i % sideLength;
                int row = i / sideLength;
                output[col, row] = input.ElementAt(i);
            }
            return output;
        }

        public static IEnumerable<T> From2DArray<T>(T[,] array)
        {
            var sidelength = GetSquareRoot(array.Length);
            for (int row = 0; row < sidelength; row++)
            {
                for (int col = 0; col < sidelength; col++)
                {
                    yield return array[col, row];
                }
            }
        }


        public static IEnumerable<IEnumerable<T>> ToSquareEnumerable<T>(T[,] input)
        {
            int sideLength = GetSquareRoot(input.Length);
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

        private static int GetSquareRoot(int input)
        {
            switch (input) {
                case 81:
                    return 9;
                case 64:
                    return 8;
                case 9:
                    return 3;
                default:
                    return -1;
            }
        }
    }
}
