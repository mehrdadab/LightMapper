using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper.Models
{
    public class CachedObject
    {
        public PropertyInfo SourceObjectInfo { get; set; }
        public PropertyInfo DestinationObjectInfo { get; set; }
    }
}
