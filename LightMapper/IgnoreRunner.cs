using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class IgnoreRunner
    {
        public static void RunIgnore()
        {
            object returnedResult = null;
            var type = typeof(LightMapperIgnore);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var item in types)
            {
                MethodInfo methodInfo = item.GetMethod("RegisterIgnore");
                if (methodInfo != null && !methodInfo.IsAbstract)
                {
                    object classInstance = Activator.CreateInstance(item, null);
                    returnedResult = methodInfo.Invoke(classInstance, null);
                }
            }
        }
    }
}
