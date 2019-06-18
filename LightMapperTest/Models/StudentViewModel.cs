using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightMapperTest.Models
{
    public class StudentViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Score { get; set; }
        public string Teacher { get; set; }
        public string[] ArrayTest { get; set; }
        public StudentTaskViewModel StudentTask { get; set; }
        public string Test { get; set; }

    }
}
