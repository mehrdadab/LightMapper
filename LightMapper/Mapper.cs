using LightMapper.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Linq;

namespace LightMapper
{
    public class Mapper : IMapper
    {
        public Mapper()
        {
            IgnoreRunner.RunIgnore();
        }
        public Destination Map<Source, Destination>(Source source) where Source : class where Destination : class, new()
        {

            var dest = SetValues<Source, Destination>(source);

            //var func = (ProfileFunction<Source, Destination>)mapping?.FunctonAfterMapping;

            //if (func != null && func.Function!=null)
            //    dest = func.Function(source, dest);
      
            return dest;
        }

        private Destination SetValues<Source, Destination>(Source source) where Source : class where Destination : class, new()
        {
            string[] ignoreList;

            bool isAnyItemIgnoredAtAll = IgnoreProvider.GetIgnoreList(typeof(Source), typeof(Destination), out ignoreList);

            Destination destination = new Destination();

            Type sourceType = typeof(Source);

            Type destinationType = typeof(Destination);

            PropertyInfo[] propertiesSource = sourceType.GetProperties();

            PropertyInfo[] propertiesDestination = destinationType.GetProperties();

            foreach (var propertyInfo in propertiesSource)
            {
                if (isAnyItemIgnoredAtAll)
                {
                    var ignored = ignoreList.Where(d => d == propertyInfo.Name);

                    if (ignored.Any())
                        continue;
                }

                if (propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.FullName.StartsWith("System."))
                {
                   var destPropertyInfo = propertiesDestination.First(d => d.Name == propertyInfo.Name);

                    var sourceVal = propertyInfo.GetValue(source);

                    var dest = ReflectionProvider.MapByReflection(this, sourceVal, destPropertyInfo.PropertyType, propertyInfo.Name);

                    destPropertyInfo.SetValue(destination, dest);
                }
                else
                {
                    ReflectionProvider.MapValue(source, destination, propertyInfo.Name);
                }
            }
  
            return destination;
        }

 
    }
}
