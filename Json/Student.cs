using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json
{
	public class Student
	{
        [property:JsonIgnore]
        public int id { get; set; }
        [property:JsonPropertyName("MyName")]
		public string name { get; set; }
		public string school { get; set; }

        public Student()
		{
		
		}

        public override string ToString()
        {
            var student = new Student() { id = 1, name = "whl", school = "hawd" };
            return JsonSerializer.Serialize(student);
        }
    }
}

