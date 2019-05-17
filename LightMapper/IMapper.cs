namespace LightMapper
{
    public interface IMapper
    {
        Destination Map<Source, Destination>(Source source) where Source:class where Destination : class, new();
    }
}