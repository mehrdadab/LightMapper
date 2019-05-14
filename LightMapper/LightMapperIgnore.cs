using System;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public abstract class LightMapperIgnore
    {
        public abstract void RegisterIgnore();
        public void RegisterMappingIgnore<Source,Destination>(params string[] propertiesToIgnore)
        {
             string key = NameCreator.CacheKey(typeof(Source), typeof(Destination));
            MapperCore.IgnoreList = new System.Collections.Concurrent.ConcurrentDictionary<string, string[]>();
            MapperCore.IgnoreList.TryAdd(key,propertiesToIgnore);
        }
    }
}
