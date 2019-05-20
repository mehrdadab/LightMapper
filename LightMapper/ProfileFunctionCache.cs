using LightMapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class ProfileFunctionCache
    {
        public static void SetFunction(Type source, Type destination,ProfileFunction cachedItem)
        {
           var key = NameCreator.CacheKey(source, destination);
            MapperCore.ProfileFunctionList = new System.Collections.Concurrent.ConcurrentDictionary<string, ProfileFunction>();
            MapperCore.ProfileFunctionList.TryAdd(key, cachedItem);
        }
        public static ProfileFunction GetFunction(Type source, Type destination)
        {
            if (MapperCore.ProfileFunctionList == null)
                return null;
            var key = NameCreator.CacheKey(source, destination);
            ProfileFunction cachedItem = null;
            MapperCore.ProfileFunctionList.TryGetValue(key, out cachedItem);
            return cachedItem;
        }
    }
}
