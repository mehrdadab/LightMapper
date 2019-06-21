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
           var key = NameCreator.CacheKey(typeof(Source), typeof(Destination));
            MapInfo mapInfo = null;
            MapperCore.MapInfoList.TryGetValue(key,out mapInfo);
            Destination destination = new Destination();
            Type sourceType = null;

            Type destinationType = null;

            PropertyInfo[] propertiesSource = null;
            PropertyInfo[] propertiesDestination = null;

            if (mapInfo==null)
            {
                sourceType = typeof(Source);
                destinationType = typeof(Destination);
                propertiesSource = sourceType.GetProperties();
                propertiesDestination = destinationType.GetProperties();
                MapperCore.MapInfoList.TryAdd(key,new MapInfo {SourceType=sourceType,DestinationType=destinationType,SourcePropertyInfo=propertiesSource,DestinationPropertyInfo=propertiesDestination });
            }
            else
            {
                sourceType = mapInfo.SourceType;
                destinationType = mapInfo.DestinationType;
                propertiesSource = mapInfo.SourcePropertyInfo;
                propertiesDestination = mapInfo.DestinationPropertyInfo;

            }

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
