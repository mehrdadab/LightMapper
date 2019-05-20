using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper.Models
{
    public class ProfileFunction<Source,Destination> 
        where Source:class
        where Destination:class
    {
        public Func<Source,Destination,Destination> Function { get; set; }
    }
}
