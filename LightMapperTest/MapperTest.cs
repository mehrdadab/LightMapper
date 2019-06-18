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
    }
}
