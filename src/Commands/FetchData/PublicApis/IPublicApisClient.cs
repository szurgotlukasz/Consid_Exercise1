using System.Threading.Tasks;

namespace Exercise1.Commands.FetchData
{
    public interface IPublicApisClient
    {
        Task<PublicApisResponse> GetAsync();
    }
}