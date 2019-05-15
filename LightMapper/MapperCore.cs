using LightMapper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class MapperCore
    {
        public static ConcurrentDictionary<string, string[]> IgnoreList { get; set; }
        public static ConcurrentDictionary<string, ProfileFunctionCachedItem> ProfileFunctionList { get; set; }
        public static ConcurrentDictionary<string, CachedObject[]> CachedObjectList { get; set; }
    }
}
