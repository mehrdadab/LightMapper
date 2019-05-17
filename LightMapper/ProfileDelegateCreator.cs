using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class ProfileDelegateCreator
    {
        public static Func<Source, Destination, Destination> CreateDelegate<Source, Destination>()
            where Source : class
            where Destination : class

        {
            var type = typeof(ILightMapperProfile);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !type.IsAbstract);
            if (types.Count() > 1)
                throw new Exception("Two functions with the same name is not allowed inside the LightMapper profile classes.");
            Type functionClassType = types.First().GetType();
            MethodInfo methodInfo = functionClassType.GetMethod(functionClassType.Name);
            Func<Source, Destination, Destination> createdDelegate = (Func<Source, Destination, Destination>)Delegate.CreateDelegate(functionClassType, methodInfo);

            return createdDelegate;

        }
    }
}
