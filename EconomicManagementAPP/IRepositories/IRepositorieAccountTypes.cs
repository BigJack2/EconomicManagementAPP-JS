using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.IRepositories
{
    public interface IRepositorieAccountTypes
    {
        Task Create(AccountTypes accountTypes);
        Task<bool> Exist(string Name);
        Task<IEnumerable<AccountTypes>> getAccounts();
        Task Modify(AccountTypes accountTypes);
        Task<AccountTypes> getAccountById(int Id);
        Task Delete(int Id);
    }
}
