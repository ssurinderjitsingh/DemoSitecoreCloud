namespace DemoSitecoreCloud.Interface
{
    public interface IConnection
    {
        string EndPoint { get; }
        Task<string> GetAccessTokenAsync();
    }
}
