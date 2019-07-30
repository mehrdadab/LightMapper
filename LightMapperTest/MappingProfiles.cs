using LightMapper;
using LightMapperTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightMapperTest
{
    public class MappingProfiles:ILightMapperProfile
    {
        public ProductViewModel CustomMap(Product s, ProductViewModel d)
        {
            d.ProductName = "Hello World";
            return d;
        }
    }
}
