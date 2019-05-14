using System;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class NameCreator
    {
        public static string CacheKey(Type source,Type destination)
        {
            string sourceName = source.FullName;
            string destinationName = destination.FullName;
            return $"{sourceName}-{destinationName}";

        }
    }
}
