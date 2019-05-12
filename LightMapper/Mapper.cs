using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class Mapper
    {
        public Mapper()
        {
            ProfileRunner.RunIgnore();
        }
        public Destination Map<Source, Destination>(Source source) where Destination : class, new()
        {
            string[] ignoreList = null;
            bool isAnyItemIgnored = false;

            var destination = new Destination();

            Type destinationType = destination.GetType();

            Type sourceType = source.GetType();

            var propertiesInfo = sourceType.GetProperties();

            string ignoreKey = NameCreator.IgnoreKey(typeof(Source), typeof(Destination));

            if (MapperCore.IgnoreList!=null)
            {
                isAnyItemIgnored = MapperCore.IgnoreList.TryGetValue(ignoreKey, out ignoreList);
            }

            for (int i = 0; i < propertiesInfo.Length; i++)
            {

                bool  isCurrentItemIgnored = false;

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
                if (isCurrentItemIgnored == false)
                    destination = SetValue<Destination>(destination, propertiesInfo[i].Name, propertiesInfo[i].GetValue(source, null), propertiesInfo[i].PropertyType);
            }

            destination = (Destination)ProfileRunner.Run(source.GetType(), destination.GetType(), source, destination);

            return destination;
        }
        private Destination SetValue<Destination>(Destination destination, string propertyName, object valueToBeSet, Type sourceType)
        {
            Type t = destination.GetType();

            PropertyInfo[] propertiesInfo = t.GetProperties();

            for (int i = 0; i < propertiesInfo.Length; i++)
            {
                if (propertiesInfo[i].Name == propertyName)
                {
                    Type propertyType = propertiesInfo[i].PropertyType;

                    if (propertyType.Name == sourceType.Name)
                        propertiesInfo[i].SetValue(destination, valueToBeSet);

                    break;
                }
            }

            return destination;
        }
    }
}
