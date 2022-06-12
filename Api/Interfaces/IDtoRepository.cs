namespace Api.Interfaces
{
    public interface IDtoRepository<T> where T : class
    {
        IEnumerable<T> GetAllDtos();
        T GetDto(int id);
    }
}