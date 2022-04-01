using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.IRepositories
{
    //Este archivo es consumido o asociado con el Service
    public interface IRepositorieUsers
    {
        Task Create(Users users); //Se agrega task por el asincronismo
        Task<bool> Exist(string Email); //Hacemos asincrona la validacion de un email existente
        Task<IEnumerable<Users>> getUser(); //Creamos un metodo al cual despues llamamos para la creacion de la lista
        Task Modify(Users users);// Datos para la vista del Modify
        Task<Users> getUserById(int Id); //Id para el Modify
        Task Delete(int Id);//Id Para el Delete
        Task<Users> Login(string Email, string Password); //Datos para el login
    }
}
