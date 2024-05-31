using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgo
{
    
    public abstract class Sort
    {
        protected int[] array;
        protected int[] bucket;

        protected Sort(int[] array)
        {
            this.array = array;
        }

        public Action<int[]> OnUpdate { get; set; }

        public abstract void SortArray();
        public abstract Task SortArrayAsync();
    


        public int[] GetArray()
        {
            return array;
        }

        protected async Task UpdateArray()
        {
            OnUpdate?.Invoke(array);
            await Task.Delay(20); // Затримка для наочної анімації
        }

        protected async Task UpdateBucket()
        {
            OnUpdate?.Invoke(bucket);
            await Task.Delay(50); // Затримка для наочної анімації
        }
        public static void ReverseArray(int[] sortedArray)
        {
            int left = 0;
            int right = sortedArray.Length - 1;

            while (left < right)
            {
                int temp = sortedArray[left];
                sortedArray[left] = sortedArray[right];
                sortedArray[right] = temp;
                left++;
                right--;
            }
        }
    }

}
