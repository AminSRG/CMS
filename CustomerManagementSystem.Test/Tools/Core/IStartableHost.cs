namespace CustomerManagementSystem.Test.Tools.Core
{
    public interface IStartableHost : IHost
    {
        void Start();
        void Stop();
    }
}