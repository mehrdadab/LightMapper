using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightMapperTest.Models
{
    public class Student
    {
        public Student()
        {
            Test = "Hello";
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Score { get; set; }
        public string Teacher { get; set; }
        public string[] ArrayTest { get; set; }
        public StudentTask StudentTask { get; set; }
        private string Test { get; set; }

    }
}
