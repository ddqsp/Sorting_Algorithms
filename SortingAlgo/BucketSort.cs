using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgo
{
    public class BucketSort : Sort
    {
        public int SwapCount { get; private set; }

        public BucketSort(int[] array) : base(array) { }
        public override async Task SortArrayAsync()
        {
            int n = array.Length;

            if (n <= 0)
                return;

            int minValue = array.Min();
            int maxValue = array.Max();
            int range = maxValue - minValue + 1;

            List<int>[] buckets = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                buckets[i] = new List<int>();
            }


            for (int i = 0; i < n; i++)
            {
                int bucketIndex = (array[i] - minValue) * (n - 1) / (maxValue - minValue);
                buckets[bucketIndex].Add(array[i]);
            }

            // Sort individual buckets using an appropriate sorting algorithm
            for (int i = 0; i < n; i++) // Iterate over buckets in ascending order
            {
                if (buckets[i].Count > 1)
                {
                    SwapCount += await SortBucketAsync(buckets[i]);
                }
            }

            // Concatenate all buckets back into the array
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < buckets[i].Count; j++)
                {
                    array[index++] = buckets[i][j];
                    await UpdateArray();
                }
            }
           
        }

        public override void SortArray()
        {
            int n = array.Length;

            if (n <= 0)
                return;

            // Find the minimum and maximum values in the array to determine the range
            int minValue = array[0];
            int maxValue = array[0];
            for (int i = 1; i < n; i++)
            {
                if (array[i] < minValue)
                    minValue = array[i];
                if (array[i] > maxValue)
                    maxValue = array[i];
            }

            // Create n empty buckets
            List<int>[] buckets = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                buckets[i] = new List<int>();
            }

            // Distribute the array elements into the buckets
            for (int i = 0; i < n; i++)
            {
                int bucketIndex = (array[i] - minValue) * (n - 1) / (maxValue - minValue);
                buckets[bucketIndex].Add(array[i]);
            }

            // Sort individual buckets using an appropriate sorting algorithm
            for (int i = n - 1; i >= 0; i--) // Iterate over buckets in reverse order
            {
                if (buckets[i].Count > 1)
                {
                    SwapCount += SortBucket(buckets[i]);
                }
            }

            // Concatenate all buckets back into the array
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < buckets[i].Count; j++)
                {
                    array[index++] = buckets[i][j];
                }
            }
        }

        private async Task<int> SortBucketAsync(List<int> bucket)
        {
            int swapCount = 0;
            int[] tempArray = bucket.ToArray();
            for (int i = 0; i < bucket.Count - 1; i++)
            {
                for (int j = 0; j < bucket.Count - i - 1; j++)
                {
                    if (bucket[j] > bucket[j + 1])
                    {
                        int temp = bucket[j];
                        bucket[j] = bucket[j + 1];
                        bucket[j + 1] = temp;
                        tempArray = bucket.ToArray();
                        OnUpdate?.Invoke(tempArray);
                        await UpdateArray();
                    }
                }
            }
            return swapCount;
        }
        

        private int SortBucket(List<int> bucket)
        {
            int swapCount = 0;
            for (int i = 0; i < bucket.Count - 1; i++)
            {
                for (int j = 0; j < bucket.Count - i - 1; j++)
                {
                    if (bucket[j] < bucket[j + 1])
                    {
                        int temp = bucket[j];
                        bucket[j] = bucket[j + 1];
                        bucket[j + 1] = temp;
                        swapCount++;
                    }
                }
            }
            return swapCount;
        }

    }

}