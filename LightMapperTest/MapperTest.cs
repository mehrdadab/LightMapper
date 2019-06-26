using LightMapperTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LightMapperTest
{
    public class MapperTest
    {
        [Fact]
        public void Mapping_SimpleObject()
        {
            var product = new Product();
            product.ProductName = "Milk";
            product.ProductDetails = "Milk is good for health";
            //product.Ids = new decimal[] { 10, 11, 12 };
            var mapper = new LightMapper.Mapper();
            var result = mapper.Map<Product, ProductViewModel>(product);
            bool mapresult = result.ProductName == "Milk" && result.ProductDetails == "Milk is good for health";// && result.Ids[0]==10 &&
            Assert.True(mapresult);

        }
        [Fact]
        public void Mapping_RecursiveObjectMap()
        {
            StudentViewModel result3 = new StudentViewModel();
            var student = new Student { Name = "Mike", Age = 18, Score = 19.2M, Teacher = "Jim", ArrayTest = new string[3] { "a", "b", "c" }, StudentTask = new StudentTask() { TaskName = "My Test", Duration = 10.0M } };

             var mapper = new LightMapper.Mapper();
            var result = mapper.Map<Student, StudentViewModel>(student);
            bool mapresult = result.Name == "Mike" && result.Age == 18 && result.Score==19.2M 
                && result.Teacher=="Jim" && result.StudentTask.TaskName== "My Test" && result.StudentTask.Duration==10.0M;
            Assert.True(mapresult);

        }
    }
}
