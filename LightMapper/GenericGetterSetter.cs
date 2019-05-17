using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class GenericGetterSetter<Source, Destination, Value> : IGenericGetterSetter<Source, Destination> where Source : class where Destination : class
    {
        private string _PropertyName;
        private Func<Source, Value> getter = null;
        private Action<Destination, Value> setter = null;
        public void Map(Source source, Destination destination, string propertyName = null)
        {
            if (propertyName == null)
                propertyName = _PropertyName;
            if (getter == null)
            {
                MethodInfo methodInfo = typeof(Source).GetProperty(propertyName).GetGetMethod();
                getter = (Func<Source, Value>)Delegate.CreateDelegate(typeof(Func<Source, Value>), null, methodInfo);
            }

            if (setter == null)
            {
                MethodInfo methodInfo = typeof(Destination).GetProperty(propertyName).GetSetMethod();
                setter = (Action<Destination, Value>)Delegate.CreateDelegate(typeof(Action<Destination, Value>), null, methodInfo);
            }

            if (!String.IsNullOrWhiteSpace(propertyName))
                _PropertyName = propertyName;

            var val = getter(source);
            setter(destination, val);
        }
    }
}
