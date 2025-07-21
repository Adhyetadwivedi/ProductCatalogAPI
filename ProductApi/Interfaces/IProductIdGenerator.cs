namespace ProductApi.Interfaces
{
    public interface IProductIdGenerator
    {
        Task<string> GenerateAsync();
    }
}
