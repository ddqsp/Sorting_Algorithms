using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgo
{
    public class RadixSort : Sort
    {
        public int SwapCount { get; private set; }

        public RadixSort(int[] array) : base(array) { }

        public override async Task SortArrayAsync()
        {
            int maxValue = array.Max();
            for (int exp = 1; maxValue / exp > 0; exp *= 10)
            {
                await CountSortAsync(exp);
            }
        }
 

        private async Task CountSortAsync(int exp)
        {
            int n = array.Length;
            int[] output = new int[n];
            int[] count = new int[10];

            for (int i = 0; i < n; i++)
            {
                count[(array[i] / exp) % 10]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                output[count[(array[i] / exp) % 10] - 1] = array[i];
                count[(array[i] / exp) % 10]--;
            }

            for (int i = 0; i < n; i++)
            {
                array[i] = output[i];
                await UpdateArray();
            }
        }     
        public override void SortArray()
        {
            // Handle positive and negative numbers separately
            List<int> positiveNumbers = new List<int>();
            List<int> negativeNumbers = new List<int>();

            foreach (int number in array)
            {
                if (number >= 0)
                {
                    positiveNumbers.Add(number);
                }
                else
                {
                    negativeNumbers.Add(-number); // Store negative numbers as positive
                }
            }

            if (positiveNumbers.Count > 0)
            {
                RadixSortAlgorithm(positiveNumbers);
            }

            if (negativeNumbers.Count > 0)
            {
                RadixSortAlgorithm(negativeNumbers);
                negativeNumbers = negativeNumbers.Select(num => -num).ToList(); // Convert back to negative
                negativeNumbers.Reverse();
            }

            // Combine the results back into the original array
            int index = 0;
            foreach (int number in negativeNumbers)
            {
                array[index++] = number;
                SwapCount++;
            }

            foreach (int number in positiveNumbers)
            {
                array[index++] = number;
                SwapCount++;
            }
        }

        private void RadixSortAlgorithm(List<int> numbers)
        {
            int max = numbers.Max();

            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountingSort(numbers, exp);
            }
        }

        private void CountingSort(List<int> numbers, int exp)
        {
            int n = numbers.Count;
            int[] output = new int[n]; // Output array
            int[] count = new int[10]; // Count array

            // Store count of occurrences in count[]
            for (int i = 0; i < n; i++)
            {
                int index = (numbers[i] / exp) % 10;
                count[index]++;
            }

            // Change count[i] so that it contains the actual position of the digit in output[]
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            // Build the output array
            for (int i = n - 1; i >= 0; i--)
            {
                int index = (numbers[i] / exp) % 10;
                output[count[index] - 1] = numbers[i];
                count[index]--;
                SwapCount++;
            }

            // Copy the output array to numbers[], so that numbers[] contains sorted numbers
            for (int i = 0; i < n; i++)
            {
                numbers[i] = output[i];
                SwapCount++;
            }
        }
    }

}
