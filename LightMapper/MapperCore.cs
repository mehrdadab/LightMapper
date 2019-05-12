using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LightMapper
{
    public class MapperCore
    {
        public static ConcurrentDictionary<string, string[]> IgnoreList { get; set; }
    }
}
