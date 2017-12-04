using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.Core.Twitter
{
    public interface ITwitterAppAccountRepository
    {
        Task<string> RegisterAsync(ITwitterAppAccount operation);
        Task UpdateAsync(ITwitterAppAccount twitterAppAccount);
        Task<ITwitterAppAccount> GetAsync(string email);
    }
}
