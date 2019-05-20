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
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);

            if (types.Count() > 1)
                throw new Exception("Two functions with the same name is not allowed inside the LightMapper profile classes.");

            if (types.Any())
            {
                Type functionClassType = types.First();
                MethodInfo methodInfo = null;
                try
                {
                    Type sourceType = typeof(Source);
                    Type destType = typeof(Destination);

                    MethodInfo[] methodInfoList = functionClassType.GetMethods();

                    methodInfo = methodInfoList.SingleOrDefault(d =>
                   d.GetParameters().Count() == 2 &&
                   d.GetParameters()[0].ParameterType == sourceType &&
                   d.GetParameters()[1].ParameterType == destType);

                    if (methodInfo == null)
                        return null;
                }
                catch
                {
                    throw new Exception("Two functions with the same name is not allowed inside the LightMapper profile classes.");
                }
                Func<Source, Destination, Destination> createdDelegate = (Func<Source, Destination, Destination>)Delegate.CreateDelegate(typeof(Func<Source,Destination,Destination>),null, methodInfo);

                return createdDelegate;
            }
            else
            {
                return null;
            }

        }
    }
}
