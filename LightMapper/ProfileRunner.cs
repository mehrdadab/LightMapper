using LightMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class ProfileRunner
    {
        public static object Run(Type sourceType,Type destType,params object[] parameters)
        {
            object returnedResult = null;
            int numberOfMethodsFound = 0;
            ProfileFunction cachedItem = null;
            cachedItem = ProfileFunctionCache.GetFunction(sourceType, destType);

            if (cachedItem != null && cachedItem.MethodInfo == null)
            {
                return parameters[1];//means return destination without any change
            }
            else if(cachedItem !=null && cachedItem.MethodInfo!=null)
            {
                object classInstance = Activator.CreateInstance(cachedItem.ClassType, null);

                returnedResult = cachedItem.MethodInfo.Invoke(classInstance, parameters);

                return returnedResult;
            }
            else
            {
                cachedItem = new ProfileFunction();
                cachedItem.Source = sourceType;
                cachedItem.Destination = destType;
            }

            var type = typeof(ILightMapperProfile);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var classType in types)
            {
                MethodInfo methodInfo = null;
                try
                {
                    methodInfo = classType.GetMethods().SingleOrDefault(d => 
                    d.GetParameters().Count() == 2 &&
                    d.GetParameters()[0].ParameterType == sourceType &&
                    d.GetParameters()[1].ParameterType == destType && 
                    !d.IsAbstract);
                }
                catch
                {
                    throw new Exception("Two functions with the same name is not allowed inside the LightMapper profile classes.");
                }
                //MethodInfo methodInfo = item.GetMethod(methodName);
                if (methodInfo != null)
                {
                    numberOfMethodsFound++;

                    if (numberOfMethodsFound > 1)
                        throw new Exception("Two functions with the same name is not allowed inside the LightMapper profile classes.");

                    object classInstance = Activator.CreateInstance(classType, null);

                    returnedResult = methodInfo.Invoke(classInstance, parameters);

                    cachedItem.ClassType = classType;

                    cachedItem.MethodInfo = methodInfo;

                    ProfileFunctionCache.SetFunction(sourceType, destType, cachedItem);
                }
                else
                {
                    ProfileFunctionCache.SetFunction(sourceType, destType, cachedItem);
                    return parameters[1];
                }
            }

            return returnedResult;
        }
 
    }
}
