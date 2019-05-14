using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper.Models
{
    public class ProfileFunctionCachedItem
    {
        public Type Source { get; set; }
        public Type Destination { get; set; }
        public Type ClassType { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
