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
        public Destination Map<Source, Destination>(Source source) where Destination : class, new()
        {
            string[] ignoreList = null;
            bool isAnyItemIgnored = false;
            PropertyInfo destPropertyInfo = null;

            var destination = new Destination();

            Type destinationType = destination.GetType();

            Type sourceType = source.GetType();

            var propertiesInfo = sourceType.GetProperties();

            CachedObject[] objectInfoList = ObjectCache.GetObject(sourceType, destinationType);
            if (objectInfoList == null)
            {
                isAnyItemIgnored = GetIgnoreList(typeof(Source), typeof(Destination),out ignoreList);

                IList<CachedObject> tempObjectList = new List<CachedObject>();
                for (int i = 0; i < propertiesInfo.Length; i++)
                {
                    bool isCurrentItemIgnored = IsItemIgnored(ignoreList, isAnyItemIgnored, propertiesInfo, i);

                    if (isCurrentItemIgnored == false)
                    {
                        destination = SetValue<Destination>(destination, propertiesInfo[i].Name, propertiesInfo[i].GetValue(source, null), propertiesInfo[i].PropertyType, out destPropertyInfo);

                        tempObjectList.Add(new CachedObject {SourceObjectInfo=propertiesInfo[i],DestinationObjectInfo=destPropertyInfo });

                    }
                }

                ObjectCache.SetObject(sourceType, destinationType, tempObjectList.ToArray());
                destination = (Destination)ProfileRunner.Run(source.GetType(), destination.GetType(), source, destination);
            }
            else
            {
                for(int i=0;i<objectInfoList.Length;i++)
                {
                    object valueToBeSet = objectInfoList[i].SourceObjectInfo.GetValue(source, null);

                    objectInfoList[i].DestinationObjectInfo.SetValue(destination, valueToBeSet);
                }
               // objectInfoList. propertiesInfo[i].SetValue(destination, valueToBeSet);
            }
            return destination;
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

        private bool GetIgnoreList(Type sourceType,Type destinationType,out string[] ignoreList) 
        {
            ignoreList = null;
            bool isAnyItemIgnored = false;

            string ignoreKey = NameCreator.CacheKey(sourceType,destinationType);

            if (MapperCore.IgnoreList != null)
            {
                isAnyItemIgnored = MapperCore.IgnoreList.TryGetValue(ignoreKey, out ignoreList);
            }
            return isAnyItemIgnored;
        }

        private Destination SetValue<Destination>(Destination destination, string propertyName, object valueToBeSet, Type sourceType,out PropertyInfo destPropertyInfo)
        {
            destPropertyInfo = null;
            Type t = destination.GetType();

            PropertyInfo[] propertiesInfo = t.GetProperties();

            for (int i = 0; i < propertiesInfo.Length; i++)
            {
                if (propertiesInfo[i].Name == propertyName)
                {
                    Type propertyType = propertiesInfo[i].PropertyType;

                    if (propertyType.Name == sourceType.Name)
                    {
                        propertiesInfo[i].SetValue(destination, valueToBeSet);

                        destPropertyInfo = propertiesInfo[i];
                    }
                    else
                    {
                        destPropertyInfo = null;
                    }

                    break;
                }
            }

            return destination;
        }
    }
}
