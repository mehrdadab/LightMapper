using LightMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class ProfileDelegateProvider
    {
        public static ProfileFunction<Source, Destination> CreateDelegate<Source, Destination>()
            where Source : class
            where Destination : class

        {
            var function = GetFunction<Source, Destination>();
            if (function != null)
                return function;
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
                var createdProfileFunction = AddFunction<Source, Destination>(createdDelegate);
                return createdProfileFunction;
            }
            else
            {
                var createdProfileFunction = AddFunction<Source, Destination>(null);
                return createdProfileFunction;
            }

        }
        private static ProfileFunction<Source, Destination> AddFunction<Source, Destination>(Func<Source, Destination, Destination> func) where Source : class where Destination : class
        {
            var key = NameCreator.CacheKey(typeof(Source), typeof(Destination));
            var profileFunction = new ProfileFunction<Source, Destination>();
            profileFunction.Function = func;
            MapperCore.ProfileFunctionList.TryAdd(key, profileFunction);
            return profileFunction;
        }
        private static ProfileFunction<Source, Destination> GetFunction<Source, Destination>() where Source : class where Destination : class
        {
            var key = NameCreator.CacheKey(typeof(Source), typeof(Destination));
            object profileFunction = null;
            MapperCore.ProfileFunctionList.TryGetValue(key,out profileFunction);
            return ((ProfileFunction<Source, Destination>)profileFunction);
        }
    }
}
