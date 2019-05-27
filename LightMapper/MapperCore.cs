﻿using LightMapper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class MapperCore
    {
        public static ConcurrentDictionary<string, string[]> IgnoreList { get; set; }
        public static ConcurrentDictionary<string, object> ProfileFunctionList { get; set; } = new ConcurrentDictionary<string, object>();
        //public static ConcurrentDictionary<string, CachedObject[]> CachedObjectList { get; set; }
        public static ConcurrentDictionary<string, MappingUnit> MapDelegateList { get; set; } = new ConcurrentDictionary<string, MappingUnit>();
    }
}
