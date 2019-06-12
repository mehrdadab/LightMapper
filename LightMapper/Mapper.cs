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
            MappingUnit mapping = null;
            var dest = SetValues<Source, Destination>(source, out mapping);

            var func = (ProfileFunction<Source, Destination>)mapping?.FunctonAfterMapping;

            if (func != null && func.Function!=null)
                dest = func.Function(source, dest);
      
            return dest;
        }

        private Destination SetValues<Source, Destination>(Source source, out MappingUnit mapping) where Source : class where Destination : class, new()
        {
            var cacheKey = "";
            mapping = null;
            MappingUnit cachedObject = null;

            Destination destination = new Destination();

            cacheKey = NameCreator.CacheKey(typeof(Source), typeof(Destination));

            MapperCore.MapDelegateList.TryGetValue(cacheKey, out cachedObject);

            if (cachedObject != null)
            {
                mapping = cachedObject;

                for (int i = 0; i < cachedObject.MappingRepositories.Length; i++)
                {
                    ((IMappingRepository<Source, Destination>)cachedObject.MappingRepositories[i]).Map(source, destination);
                }
            }
            else
            {
                IList<object> listOfDelegates = CreateMappingRepository(source, destination);

                var func = ProfileDelegateProvider.CreateDelegate<Source, Destination>();

                mapping = new MappingUnit { MappingRepositories = listOfDelegates.ToArray(), FunctonAfterMapping = func };

                MapperCore.MapDelegateList.TryAdd(cacheKey,mapping );
            }


            return destination;
        }

        private static IList<object> CreateMappingRepository<Source, Destination>(Source source, Destination destination)
            where Source : class
            where Destination : class, new()
        {
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            PropertyInfo[] properties = sourceType.GetProperties();

            IList<object> listOfDelegates = new List<object>();

            string[] ignoreList;

            bool isAnyItemIgnoredAtAll = IgnoreProvider.GetIgnoreList(typeof(Source), typeof(Destination), out ignoreList);

            foreach (var item in properties)
            {
                if (isAnyItemIgnoredAtAll)
                {
                    var ignored = ignoreList.Where(d => d == item.Name);

                    if (ignored.Any())
                        continue;
                }
                else if (item.PropertyType.IsClass && !item.PropertyType.FullName.StartsWith("System."))
                {
                    object result = CreateMappingRepositoryByReflection(source, destination, destinationType, item);

                    listOfDelegates = listOfDelegates.Concat((result as IList<object>)).ToList();

                }
                else
                {
                    Type genericClass = typeof(MappingRepository<,,>);

                    Type constructedClass = genericClass.MakeGenericType(typeof(Source), typeof(Destination), item.PropertyType);

                    IMappingRepository<Source, Destination> created = (IMappingRepository<Source, Destination>)Activator.CreateInstance(constructedClass);

                    created.Map(source, destination, item.Name);

                    listOfDelegates.Add(created);
                }
            }

            return listOfDelegates;
        }

        private static object CreateMappingRepositoryByReflection<Source, Destination>(Source source, Destination destination, Type destinationType, PropertyInfo item)
            where Source : class
            where Destination : class, new()
        {
            PropertyInfo[] destProperties = destinationType.GetProperties();

            var destItem = destProperties.First(d => d.Name == item.Name);

            //if (destItem == null)
            //   return;

            MethodInfo method = typeof(Mapper).GetMethod("CreateMappingRepository", BindingFlags.NonPublic | BindingFlags.Static);

            MethodInfo generic = method.MakeGenericMethod(item.PropertyType, destItem.PropertyType);

            var itemObject = item.GetValue(source);

            var destItemObject = Activator.CreateInstance(destItem.PropertyType);

            destItem.SetValue(destination, destItemObject);

            object[] parameters = new object[] { itemObject, destItemObject };

            var result = generic.Invoke(null, parameters);
            return result;
        }
    }
}
