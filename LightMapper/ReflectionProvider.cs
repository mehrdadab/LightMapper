﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class ReflectionProvider
    {
        public static void MapValue(object source, object destination, string propertyName)
        {
            Type sourceObjectType = source.GetType();
            Type destinationObjectType = destination.GetType();
            var cacheKey = NameCreator.CacheKey(sourceObjectType, destinationObjectType, propertyName);
            ReflectionMapObject reflectionObject = null;
            MapperCore.ReflectionMapObjectList.TryGetValue(cacheKey, out reflectionObject);
            if (reflectionObject != null)
            {
                var val = reflectionObject.MethodInfoGet.Invoke(source, null);

                reflectionObject.MethodInfoSet.Invoke(destination, new object[] { val });

            }
            else
            {
                var sourceType = sourceObjectType.GetProperty(propertyName).PropertyType;
                var destinationType = destinationObjectType.GetProperty(propertyName).PropertyType;

                if (sourceType == destinationType)
                {
                    MethodInfo methodInfoGet = sourceObjectType.GetProperty(propertyName).GetGetMethod();

                    MethodInfo methodInfoSet = destinationObjectType.GetProperty(propertyName).GetSetMethod();

                    MapperCore.ReflectionMapObjectList.TryAdd(cacheKey, new ReflectionMapObject { MethodInfoGet = methodInfoGet, MethodInfoSet = methodInfoSet });

                    var val = methodInfoGet.Invoke(source, null);
                    methodInfoSet.Invoke(destination, new object[] { val });
                }
            }
        }
        public static object MapByReflection(Mapper mapper, object source, Type destinationType, string propertyName)
        {
            Type sourceType = source.GetType();
            var cacheKey = NameCreator.CacheKey(sourceType, destinationType, propertyName);
            MethodInfo generic = null;
            MapperCore.MapByReflectionList.TryGetValue(cacheKey, out generic);
            if (generic != null)
            {
                var dest = generic.Invoke(mapper, new object[] { source });

                return dest;
            }
            else
            {
                MethodInfo method = typeof(Mapper).GetMethod("Map");

                generic = method.MakeGenericMethod(sourceType, destinationType);

                var dest = generic.Invoke(mapper, new object[] { source });

                MapperCore.MapByReflectionList.TryAdd(cacheKey, generic);

                return dest;

            }
            //    var itemObject = item.GetValue(source);

            //    var destItemObject = Activator.CreateInstance(destItem.PropertyType);

            //    destItem.SetValue(destination, destItemObject);

            //    object[] parameters = new object[] { itemObject, destItemObject };

            //    var result = generic.Invoke(null, parameters);

        }

    }

}