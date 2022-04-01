using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.IRepositories
{
    public interface IRepositorieCategories
    {
        Task Create(Categories categories);
        Task<bool> Exist(string Name);
        Task<IEnumerable<Categories>> getCategories();
        Task Modify(Categories categories);
        Task<Categories> getCategoriesById(int Id);
        Task Delete(int Id);
    }
}
