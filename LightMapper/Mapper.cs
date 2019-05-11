using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LightMapper
{
    public class Mapper
    {
        public Destination Map<Source, Destination>(Source source) where Destination : class, new()
        {
            var destination = new Destination();
            Type destinationType= destination.GetType();
            Type sourceType = source.GetType();
            Console.WriteLine(sourceType.Name);
            Console.WriteLine(destinationType.Name);
            var propertiesInfo =  sourceType.GetProperties();
            for(int i=0;i<propertiesInfo.Length;i++)
            {
                Console.WriteLine(propertiesInfo[i].Name+"=>"+ propertiesInfo[i].GetValue(source,null));

                destination =  SetValue<Destination>(destination, propertiesInfo[i].Name, propertiesInfo[i].GetValue(source, null),propertiesInfo[i].PropertyType);
            }
            destination = (Destination)ProfileRunner.Run(source.GetType(),destination.GetType(),source,destination);
            return destination;
        }
        private Destination SetValue<Destination>(Destination destination,string propertyName,object valueToBeSet,Type sourceType)
        {
            Type t = destination.GetType();
            PropertyInfo[] propertiesInfo = t.GetProperties();
            //propertiesInfo.SetValue()
            for (int i = 0; i < propertiesInfo.Length; i++)
            {   
                if(propertiesInfo[i].Name == propertyName)
                {
                    Type propertyType = propertiesInfo[i].PropertyType;
                    if(propertyType.Name == sourceType.Name)
                        propertiesInfo[i].SetValue(destination, valueToBeSet);
                    
                    break;
                }
                    //+ "=>" + propertiesInfo[i].GetValue(dest, null));
            }
            return destination;
        }
    }
}
