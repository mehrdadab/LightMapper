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

            var func = ProfileDelegateProvider.CreateDelegate<Source, Destination>();

            if (func != null && func.Function != null)
                dest = func.Function(source, dest);

            return dest;
        }

        private bool IsItemIgnored(string[] ignoreList, bool isAnyItemIgnored, PropertyInfo[] propertiesInfo, int i)
        {
            bool isCurrentItemIgnored = false;

            if (isAnyItemIgnored && ignoreList != null && ignoreList.Length > 0)
            {
                for (int j = 0; j < ignoreList.Length; j++)
                {
                    if (ignoreList[j] == propertiesInfo[i].Name)
                    {
                        isCurrentItemIgnored = true;
                        break;
                    }

                }
            }

            return isCurrentItemIgnored;
        }

        private bool GetIgnoreList(Type sourceType, Type destinationType, out string[] ignoreList)
        {
            ignoreList = null;
            bool isAnyItemIgnored = false;

            string ignoreKey = NameCreator.CacheKey(sourceType, destinationType);

            if (MapperCore.IgnoreList != null)
            {
                isAnyItemIgnored = MapperCore.IgnoreList.TryGetValue(ignoreKey, out ignoreList);
            }
            return isAnyItemIgnored;
        }


        private Destination SetValues<Source, Destination>(Source source) where Source : class where Destination : class, new()
        {
            var cacheKey = "";

            object[] cachedObject = null;

            Destination destination = new Destination();

            MapperCore.MapDelegateList.TryGetValue(cacheKey, out cachedObject);

            if (cachedObject != null)
            {
                for (int i = 0; i < cachedObject.Length; i++)
                {
                    ((IMappingRepository<Source, Destination>)cachedObject[i]).Map(source, destination);
                }
            }
            else
            {
                Type sourceType = source.GetType();

                PropertyInfo[] properties = sourceType.GetProperties();

                IList<object> listOfDelegates = new List<object>();

                foreach (var item in properties)
                {

                    Type genericClass = typeof(MappingRepository<,,>);

                    Type constructedClass = genericClass.MakeGenericType(typeof(Source), typeof(Destination), item.PropertyType);

                    IMappingRepository<Source, Destination> created = (IMappingRepository<Source, Destination>)Activator.CreateInstance(constructedClass);

                    created.Map(source, destination, item.Name);

                    listOfDelegates.Add(created);
                }

                MapperCore.MapDelegateList.TryAdd(cacheKey, listOfDelegates.ToArray());
            }


            return destination;
        }
    }
}
