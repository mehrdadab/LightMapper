using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper.Models
{
    public class MapInfo
    {
        public PropertyInfo[] SourcePropertyInfo { get; set; }
        public PropertyInfo[] DestinationPropertyInfo { get; set; }
        public Type SourceType { get; set; }
        public Type DestinationType { get; set; }
    }
}
