using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class ProfileRunner
    {
        public static object Run(Type sourceType,Type DestType,params object[] parameters)
        {
            object returnedResult = null;
            int numberOfMethodsFound = 0;
            var type = typeof(ILightMapperProfile);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var item in types)
            {
                MethodInfo methodInfo = null;
                try
                {
                    methodInfo = item.GetMethods().SingleOrDefault(d => 
                    d.GetParameters().Count() == 2 &&
                    d.GetParameters()[0].ParameterType == sourceType &&
                    d.GetParameters()[1].ParameterType == DestType && 
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
                    object classInstance = Activator.CreateInstance(item, null);
                    returnedResult = methodInfo.Invoke(classInstance, parameters);
                }
                else
                {
                    return parameters[1];
                }
            }

            return returnedResult;
        }
 
    }
}
