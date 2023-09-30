// See https://aka.ms/new-console-template for more information
using Algorithm;

Console.WriteLine("Hello, World!");

/* 冒泡排序 */

bool Compare(int a, int b)
{
    return a > b;
}

int[] a = { 2, 5, 7, 1, 4, 8, 3, 9 };
Console_<int>.WriteLine(a);
Console.WriteLine("升序");
BubbleSort<int>.Run(a, Compare);
Console_<int>.WriteLine(a);
Console.WriteLine("降序");
BubbleSort<int>.Run(a, Compare, false);
Console_<int>.WriteLine(a);

Console.ReadKey();


/* 数组去重 */
int[] b = { 2, 3, 4, 3, 2, 5 };
Console_<int>.WriteLine(b);
int[] newB = RemoveDuplicate.Run(b);
Console_<int>.WriteLine(newB);

