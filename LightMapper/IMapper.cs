namespace LightMapper
{
    public interface IMapper
    {
        Destination Map<Source, Destination>(Source source) where Destination : class, new();
    }
}