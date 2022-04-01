using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.IRepositories
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts accounts);
        Task<bool> Exist(string Name);
        Task<IEnumerable<Accounts>> getAccounts();
        Task Modify(Accounts accounts);
        Task<Accounts> getAccountsById(int Id);
        Task Delete(int Id);
    }
}