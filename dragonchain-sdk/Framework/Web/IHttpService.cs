using System.Threading.Tasks;

namespace dragonchain_sdk.Framework.Web
{
    public interface IHttpService
    {
        Task<ApiResponse<T>> DeleteAsync<T>(string path);
        Task<ApiResponse<T>> GetAsync<T>(string path);
        Task<ApiResponse<T>> PostAsync<T>(string path, object body);
        Task<ApiResponse<T>> PutAsync<T>(string path, object body);
        void SetEndpoint(string endPoint);
    }
}