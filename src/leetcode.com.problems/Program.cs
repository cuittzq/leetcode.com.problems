using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace leetcode.com.problems
{
    public class Program
    {
        public static void Main(string[] args)
        {
            intarraytozero();
            Console.Read();
        }

        public static void intarraytozero()
        {
            int[] intarry = { -1, 1, 0, 4, -3 };
            // 快速排序
            QsortCommon(intarry, 0, intarry.Length - 1);

            Console.WriteLine(string.Join(",", intarry));
            DoQsort();
        }

        public static void DoQsort()
        {
            int[] arr = new int[100000];                        //10W个空间大小的数组

            Random rd = new Random();
            for (int i = 0; i < arr.Length; i++)          //随机数组
            {
                arr[i] = rd.Next();
            }

            //for (int i = 0; i < arr.Length; i++)          //升序数组
            //{
            //    arr[i] = i;
            //}

            //for (int i = 0; i < arr.Length; i++)          //降序数组
            //{
            //    arr[i] = arr.Length - 1 - i;
            //}

            //for (int i = 0; i < arr.Length; i++)          //重复数组
            //{
            //    arr[i] = 5768461;
            //}

            Stopwatch watch = new Stopwatch();
            watch.Start();                                  //开始计时

            //QsortCommon(arr, 0, arr.Length - 1);          //固定基准元
            //QsortRandom(arr, 0, arr.Length - 1);          //随机基准元
            //QsortMedianOfThree(arr, 0, arr.Length - 1);   //三数取中
            //QsortThreeInsert(arr, 0, arr.Length - 1);     //三数取中+插排
            QsortThreeInsertGather(arr, 0, arr.Length - 1); //三数取中+插排+聚集相同元素

            watch.Stop();                                   //计时结束

            Console.WriteLine(watch.ElapsedMilliseconds.ToString());
        }

        /// <summary>
        /// 1.0 固定基准元（基本的快速排序）
        /// </summary>
        public static void QsortCommon(int[] arr, int low, int high)
        {
            if (low >= high) return;                        //递归出口
            int partition = Partition(arr, low, high);      //将 >= x 的元素交换到右边区域，将 <= x 的元素交换到左边区域
            QsortCommon(arr, low, partition - 1);
            QsortCommon(arr, partition + 1, high);
        }

        /// <summary>
        /// 2.0 随机基准元
        /// </summary>
        public static void QsortRandom(int[] arr, int low, int high)
        {
            if (low >= high) return;                        //递归出口
            PartitionRandom(arr, low, high);                //随机基准元
            int partition = Partition(arr, low, high);      //将 >= x 的元素交换到右边区域，将 <= x 的元素交换到左边区域
            QsortRandom(arr, low, partition - 1);
            QsortRandom(arr, partition + 1, high);
        }

        /// <summary>
        /// 3.0 三数取中
        /// </summary>
        public static void QsortMedianOfThree(int[] arr, int low, int high)
        {
            if (low >= high) return;                        //递归出口
            PartitionMedianOfThree(arr, low, high);         //三数取中
            int partition = Partition(arr, low, high);      //将 >= x 的元素交换到右边区域，将 <= x 的元素交换到左边区域
            QsortMedianOfThree(arr, low, partition - 1);
            QsortMedianOfThree(arr, partition + 1, high);
        }

        /// <summary>
        /// 4.0 三数取中+插排
        /// </summary>        
        public static void QsortThreeInsert(int[] arr, int low, int high)
        {
            if (high - low + 1 < 10)
            {
                InsertSort(arr, low, high);
                return;
            }                                               //插排，递归出口
            PartitionMedianOfThree(arr, low, high);         //三数取中
            int partition = Partition(arr, low, high);      //将 >= x 的元素交换到右边区域，将 <= x 的元素交换到左边区域
            QsortMedianOfThree(arr, low, partition - 1);
            QsortMedianOfThree(arr, partition + 1, high);
        }

        /// <summary>
        /// 5.0 三数取中+插排+聚集相同元素
        /// </summary>        
        public static void QsortThreeInsertGather(int[] arr, int low, int high)
        {
            if (high - low + 1 < 10)
            {
                InsertSort(arr, low, high);
                return;
            }                                               //插排，递归出口
            PartitionMedianOfThree(arr, low, high);         //三数取中

            //进行左右分组（处理相等元素）
            int first = low;
            int last = high;
            int left = low;
            int right = high;
            int leftLength = 0;
            int rightLength = 0;
            int key = arr[first];
            while (first < last)
            {
                while (first < last && arr[last] >= key)
                {
                    if (arr[last] == key)                   //处理相等元素
                    {
                        Swap(arr, last, right);
                        right--;
                        rightLength++;
                    }
                    last--;
                }
                arr[first] = arr[last];
                while (first < last && arr[first] <= key)
                {
                    if (arr[first] == key)
                    {
                        Swap(arr, first, left);
                        left++;
                        leftLength++;
                    }
                    first++;
                }
                arr[last] = arr[first];
            }
            arr[first] = key;

            //一次快排结束
            //把与基准元key相同的元素移到最终位置周围
            int i = first - 1;
            int j = low;
            while (j < left && arr[i] != key)
            {
                Swap(arr, i, j);
                i--;
                j++;
            }
            i = last + 1;
            j = high;
            while (j > right && arr[i] != key)
            {
                Swap(arr, i, j);
                i++;
                j--;
            }
            QsortThreeInsertGather(arr, low, first - leftLength - 1);
            QsortThreeInsertGather(arr, first + rightLength + 1, high);
        }

        /// <summary>
        /// 固定基准元，默认数组第一个数为基准元，左右分组，返回基准元的下标
        /// </summary>
        public static int Partition(int[] arr, int low, int high)
        {
            int first = low;
            int last = high;
            int key = arr[low];                             //取第一个元素作为基准元
            while (first < last)
            {
                while (first < last && arr[last] >= key)
                    last--;
                arr[first] = arr[last];
                while (first < last && arr[first] <= key)
                    first++;
                arr[last] = arr[first];
            }
            arr[first] = key;                               //基准元居中
            return first;
        }

        /// <summary>
        /// 随机基准元，将确定好的基准元与第一个数交换，无返回值
        /// </summary>        
        public static void PartitionRandom(int[] arr, int low, int high)
        {
            Random rd = new Random();
            int randomIndex = rd.Next() % (high - low) + low;//取数组中随机下标
            Swap(arr, randomIndex, low);                     //与第一个数交换
        }

        /// <summary>
        /// 三数取中确定基准元，将确定好的基准元与第一个数交换，无返回值
        /// </summary>        
        public static void PartitionMedianOfThree(int[] arr, int low, int high)
        {
            int mid = low + (high + -low) / 2;
            if (arr[mid] > arr[high])
            {
                Swap(arr, mid, high);
            }
            if (arr[low] > arr[high])
            {
                Swap(arr, low, high);
            }
            if (arr[mid] > arr[low])
            {
                Swap(arr, mid, low);
            }                                                //将中间大小的数与第一个数交换
        }

        /// <summary>
        /// 插入排序
        /// </summary>
        public static void InsertSort(int[] arr, int low, int high)
        {
            for (int i = low + 1; i <= high; i++)
            {
                if (arr[i] < arr[i - 1])
                {
                    for (int j = low; j < i; j++)
                    {
                        if (arr[j] > arr[i])
                        {
                            Swap(arr, i, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 数组交换
        /// </summary>
        public static void Swap(int[] arr, int index1, int index2)
        {
            int temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }
    }
}
