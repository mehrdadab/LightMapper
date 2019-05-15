using LightMapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class ObjectCache
    {
        public static CachedObject[] GetObject(Type source, Type destination)
        {
            if (MapperCore.CachedObjectList == null)
                return null;
            CachedObject[] cachedObject = null;

            string key = NameCreator.CacheKey(source, destination);

            MapperCore.CachedObjectList.TryGetValue(key, out cachedObject);

            return cachedObject;
        }
        public static bool SetObject(Type source, Type destination,CachedObject[] cachedObject)
        {
            if (MapperCore.CachedObjectList == null)
                MapperCore.CachedObjectList = new System.Collections.Concurrent.ConcurrentDictionary<string, CachedObject[]>();

            string key = NameCreator.CacheKey(source, destination);

            return MapperCore.CachedObjectList.TryAdd(key, cachedObject);
        }
    }
}
