namespace LightMapper
{
    public interface IGenericGetterSetter<Source, Destination>
        where Source : class
        where Destination : class
    {
        void Map(Source source, Destination destination, string propertyName = null);
    }
}