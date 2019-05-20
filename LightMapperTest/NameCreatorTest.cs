using LightMapper;
using System;
using Xunit;

namespace LightMapperTest
{
    public class NameCreatorTest
    {
        [Fact]
        public void Create_CacheKey_withTwoParameters()
        {
            string cacheKey = NameCreator.CacheKey(typeof(string), typeof(Int32));
            Assert.Equal("System.String-System.Int32", cacheKey);
        }
        [Fact]
        public void Create_CacheKey_withThreeParameters()
        {
            string cacheKey = NameCreator.CacheKey(typeof(string), typeof(Int32), "PropertyName");
            Assert.Equal("System.String-System.Int32-PropertyName", cacheKey);
        }
        [Fact]
        public void Create_CacheKey_withPropertyNameEmpty()
        {
            const string EMPTY_PROPERTY_NAME = "";
            Action actual = () => NameCreator.CacheKey(typeof(string), typeof(Int32), EMPTY_PROPERTY_NAME);
            Assert.Throws<ArgumentNullException>(actual);
        }
    }
}
