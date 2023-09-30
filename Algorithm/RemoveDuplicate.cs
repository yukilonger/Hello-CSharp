using System;
namespace Algorithm
{
	public class RemoveDuplicate
	{
		public RemoveDuplicate()
		{

		}

		public static int[] Run(int[] arr)
		{
            int i = 0, j = 1;

            while (i < arr.Length)
            {
                for (; j < arr.Length; j++)
                {
                    if (arr[i] != arr[j])
                    {
                        arr[++i] = arr[j++];// 找到不一样的一个个的都放到前面
                        break;
                    }
                }
            }

            int[] newArr = arr.Take(++i).ToArray();

            return newArr;
        }
	}
}

