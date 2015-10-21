using System;

namespace OK.MultiprocessorScheduling.Logics
{
    internal static class Algorithm
    {
        public static int IndexOfMax(int[] array)
        {
            int index = 0;
            for (int j = 1, max = array[0]; j < array.Length; j++)
            {
                if (array[j] > max)
                {
                    max = array[j];
                    index = j;
                }
            }

            return index;
        }

        public static int IndexOfMin(int[] array)
        {
            int index = 0;
            for (int j = 1, min = array[0]; j < array.Length; j++)
            {
                if (array[j] < min)
                {
                    min = array[j];
                    index = j;
                }
            }

            return index;
        }

        public static bool NextPermutation<T>(T[] array, Func<T, T, bool> func)
        {
            var largestIndex = -1;
            for (var i = array.Length - 2; i >= 0; i--)
            {
                if (func(array[i], array[i + 1]))
                {
                    largestIndex = i;
                    break;
                }
            }

            if (largestIndex < 0) return false;

            var largestIndex2 = -1;
            for (var i = array.Length - 1; i >= 0; i--)
            {
                if (func(array[largestIndex], array[i]))
                {
                    largestIndex2 = i;
                    break;
                }
            }

            var tmp = array[largestIndex];
            array[largestIndex] = array[largestIndex2];
            array[largestIndex2] = tmp;

            for (int i = largestIndex + 1, j = array.Length - 1; i < j; i++, j--)
            {
                tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }

            return true;
        }

        public static void Swap<T>(ref T item1, ref T item2)
        {
            T temp = item1;
            item1 = item2;
            item2 = temp;
        }
    }
}