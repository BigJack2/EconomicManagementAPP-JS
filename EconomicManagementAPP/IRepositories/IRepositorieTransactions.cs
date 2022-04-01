using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.IRepositories
{
    public interface IRepositorieTransactions
    {
        Task Create(Transactions transactions);
        Task<IEnumerable<Transactions>> getTransactions();
        Task Modify(Transactions transactions);
        Task<Transactions> getTransactionsById(int Id);
        Task Delete(int Id);
    }
}
