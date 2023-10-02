namespace FLZ.Services
{
    public interface IService
    {
        void OnAllServicesReady();
        bool IsReady();
    }
}