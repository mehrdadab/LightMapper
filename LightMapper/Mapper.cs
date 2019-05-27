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
                Type genericClass = typeof(MappingRepository<,,>);

                Type constructedClass = genericClass.MakeGenericType(typeof(Source), typeof(Destination), item.PropertyType);

                IMappingRepository<Source, Destination> created = (IMappingRepository<Source, Destination>)Activator.CreateInstance(constructedClass);

                created.Map(source, destination, item.Name);

                listOfDelegates.Add(created);
            }

            return listOfDelegates;
        }
    }
}
