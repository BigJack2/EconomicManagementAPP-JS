using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Services
{

    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Query Asincrona del CREATE
        public async Task Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountTypes (Name, UserId, OrderAccount) 
                                                               VALUES (@Name, @UserId, @OrderAccount); SELECT SCOPE_IDENTITY();", accountTypes);
            accountTypes.Id = id;
        }

        //Query que genera un boleano que confirma o no la existencia mediante el dato comparado
        //Esta consulta de validacion se llama desde el controller 
        public async Task<bool> Exist(string Name)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM AccountTypes
                                                                         WHERE Name = @Name",
                                                                         new { Name});
            return exist == 1;
        }

        //QUERY DEL INDEX
        //Obtenemos los datos de la tabla que son llamadas en el Views
        //Con esta query dentro de un foreach generamos el listado en Views
        public async Task<IEnumerable<AccountTypes>> getAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT Id, Name, OrderAccount FROM AccountTypes");
        }


        //QUERY DEL MODIFY
        public async Task Modify(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes SET Name = @Name WHERE Id = @Id", accountTypes);
        }


        //QUERY DE LA VISTA DEL MODIFY
        public async Task<AccountTypes> getAccountById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountTypes>(@"SELECT Id, Name, UserId, OrderAccount FROM AccountTypes
                                                                             WHERE Id = @Id", new { Id });
        }


        //QUERY DEL DELETE
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE AccountTypes WHERE Id = @Id", new { Id });
        }
    }
}
