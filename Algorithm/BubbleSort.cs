using System;

namespace Algorithm
{
	public class BubbleSort<T>
	{
        public delegate bool Compare(T t1, T t2);//传入两个参数来作比较 

        public static IList<T> Run(IList<T> ls, Compare compare, bool asc = true)
		{
            for (int i = 0; i < ls.Count - 1; i++)
            {
                for (int j = 0; j < ls.Count - 1 - i; j++)
                {
                    if (asc)
                    {
                        if (compare(ls[j], ls[j + 1]))
                        {
                            Exchange(ls, j);
                        }
                    }
                    else
                    {
                        if (!compare(ls[j], ls[j + 1]))
                        {
                            Exchange(ls, j);
                        }
                    }
                }
            }
            return ls;
        }

        private static void Exchange(IList<T> ls, int j)
        {
            T temp = ls[j];
            ls[j] = ls[j + 1];
            ls[j + 1] = temp;
        }
	}
}