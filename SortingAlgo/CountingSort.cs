using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgo
{
    public class CountingSort : Sort
    {
        public int SwapCount { get; private set; }
       
        public CountingSort(int[] array) : base(array) { }

        public override async Task SortArrayAsync()
        {
            int n = array.Length;

            if (n <= 0)
                return;

            // Find the maximum value in the array to determine the range
            int minValue = array.Min();
            int maxValue = array.Max();
            int range = maxValue - minValue + 1;
            int[] countArray = new int[range];
            await UpdateArray();

            for (int i = 0; i < n; i++)
            {

                countArray[array[i] - minValue]++;
                await UpdateArray();
            }

            int index = 0;
            for (int i = 0; i < countArray.Length; i++)
            {
                for (int j = 0; j < countArray[i]; j++)
                {
                           
                                array[index++] = i + minValue;
                                await UpdateArray();
                }
            }
            
        }

        public override void SortArray()
        {
        int n = array.Length;

            if (n <= 0)
                return;
            
            int minValue = array.Min();
            int maxValue = array.Max();
            int range = maxValue - minValue + 1;            
            int[] countArray = new int[range];
            for (int i = 0; i < n; i++)
            {
                countArray[array[i] - minValue]++;
                SwapCount++;

            }


            // Reconstruct the array from the countArray
            int index = 0;
            for (int i = 0; i < countArray.Length; i++)
            {
                for (int j = 0; j < countArray[i]; j++)
                {
                    array[index++] = i + minValue;
                    SwapCount++;
                }
            }

        }
    }

}
