namespace Syntax;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        #region FindAll深拷贝
        //List<Student> students = new List<Student>()
        //{
        //    new Student(){  name= "a", age=1 },
        //    new Student(){  name= "b", age=2},
        //    new Student(){  name= "c", age=1},
        //    new Student(){  name= "d", age=2},
        //    new Student(){  name= "e", age=1},
        //    new Student(){  name= "f", age=2},
        //};

        //List<Student> partStudents = students.FindAll(x => x.age == 1);

        //students.Clear();

        //foreach(Student s in partStudents)
        //{
        //    Console.WriteLine(s.name);
        //}
        #endregion

        #region ToList 浅拷贝
        //List<Student> students = new List<Student>()
        //{
        //    new Student(){  name= "a", age=1 },
        //    new Student(){  name= "b", age=2},
        //    new Student(){  name= "c", age=1},
        //    new Student(){  name= "d", age=2},
        //    new Student(){  name= "e", age=1},
        //    new Student(){  name= "f", age=2},
        //};

        //List<Student> newStudents = students.ToList();

        //foreach(Student s in students)
        //{
        //    s.name += s.name;
        //}

        //students.Add(new Student() { name = "g", age = 2 }); // students.Length = 7

        ////students.Clear();

        //foreach (Student s in newStudents) // newStudents.Length = 6
        //{
        //    s.name += s.name;
        //    Console.WriteLine(s.name); // name = "aa"
        //}
        #endregion

        #region ??
        //DateTime? dateTime = null;
        //DateTime oldTime = dateTime ?? DateTime.Now;
        //dateTime = DateTime.Today.AddDays(1);
        //DateTime newTime = dateTime ?? DateTime.Now;
        #endregion

        #region ForEach
        //List<Student> students = new List<Student>();
        //students.ForEach((x) => { Console.Write(x); });
        //Dictionary<int, Student> studentDict = new Dictionary<int, Student>();
        ////studentDict.ForEach // 没有ForEach
        #endregion

        #region Null?.ToList()
        //List<Student> students = null;
        //List<Student> newStudents = students?.ToList();
        #endregion

        #region 二元操作符 ^
        //Console.WriteLine(0 ^ 4);
        //Console.WriteLine(true ^ false);
        //Console.WriteLine(System.Convert.ToString(11, 2));
        //Console.WriteLine(System.Convert.ToString(12, 2));
        //Console.WriteLine(11 ^ 12);
        //Console.WriteLine(Convert.ToString(11 ^ 12, 2));
        #endregion

        #region NumbersToList
        //string numbers = "1, 2,3 ,4,";
        //try
        //{
        //    List<int> result = numbers?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.TryParse(x, out int y) ? y : int.MaxValue).ToList();
        //    result?.Remove(int.MaxValue);
        //}
        //catch(Exception ex)
        //{

        //}
        #endregion

        Dictionary<int, List<int>> ddd = new Dictionary<int, List<int>>();
        ddd.Add(1, new List<int>());
        ddd.Add(2, new List<int>());

        Console.ReadKey();
    }
}

public class Student
{
    public int age { get; set; }
    public string name { get; set; }
}
