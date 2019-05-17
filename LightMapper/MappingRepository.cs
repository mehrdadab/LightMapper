using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class MappingRepository<Source, Destination, Value> : IMappingRepository<Source, Destination> where Source : class where Destination : class
    {
        private Func<Source, Value> getter = null;
        private Action<Destination, Value> setter = null;
        public void Map(Source source, Destination destination, string propertyName = null)
        {
             if (getter == null)
            {
                MethodInfo methodInfoGet = typeof(Source).GetProperty(propertyName).GetGetMethod();
                getter = (Func<Source, Value>)Delegate.CreateDelegate(typeof(Func<Source, Value>), null, methodInfoGet);
            
                MethodInfo methodInfoSet = typeof(Destination).GetProperty(propertyName).GetSetMethod();
                setter = (Action<Destination, Value>)Delegate.CreateDelegate(typeof(Action<Destination, Value>), null, methodInfoSet);
            }

            var val = getter(source);
            setter(destination, val);
        }
    }
}
