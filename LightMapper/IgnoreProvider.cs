using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class IgnoreProvider
    {
        public static bool IsItemIgnored(string[] ignoreList, bool isAnyItemIgnored, PropertyInfo[] propertiesInfo, int i)
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

        public static bool GetIgnoreList(Type sourceType, Type destinationType, out string[] ignoreList)
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


    }
}
