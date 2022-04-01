using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Services
{
    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Query Asincrona del CREATE
        public async Task Create(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users 
                                                           (Email, StandarEmail, Password) 
                                                    VALUES (@Email, @StandarEmail, @Password); SELECT SCOPE_IDENTITY();", users);
            users.Id = id;
        }

        
        //Query que genera un boleano que confirma o no la existencia mediante el dato comparado
        //Esta consulta de validacion se llama desde el controller 
        public async Task<bool> Exist(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM Users WHERE Email = @Email;", new { Email });
            return exist == 1;
        }


        //QUERY DEL INDEX
        //Obtenemos los datos de la tabla que son llamadas en el Views
        //Con esta query dentro de un foreach generamos el listado en Views
        public async Task<IEnumerable<Users>> getUser()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Users>(@"SELECT Id, Email, StandarEmail FROM Users");
        }


        //QUERY DEL MODIFY
        public async Task Modify(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                            SET Email = @Email, StandarEmail=@StandarEmail, Password=@Password
                                            WHERE Id = @Id", users);
        }


        //QUERY DE LA VISTA DEL MODIFY
        public async Task<Users> getUserById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT Id, Email, StandarEmail, Password
                                                                      FROM Users WHERE Id = @Id", new { Id });
        }


        //QUERY DEL DELETE
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { Id });
        }


        //QUERY DEL LOGIN
        public async Task<Users> Login(string Email, string Password)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM Users WHERE Email=@Email AND Password=@Password",
                                                                     new {Email, Password});
        }
    }


}
