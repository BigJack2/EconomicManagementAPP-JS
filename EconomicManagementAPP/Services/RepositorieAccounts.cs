using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Services
{
    
    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Query Asincrona del CREATE
        public async Task Create(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Accounts 
                                                               (Name, AccountTypeId, Balance, Description) 
                                                        VALUES (@Name, @AccountTypeId, @Balance, @Description); SELECT SCOPE_IDENTITY();", accounts);
            accounts.Id = id;
        }


        //Query que genera un boleano que confirma o no la existencia mediante el dato comparado
        //Esta consulta de validacion se llama desde el controller 
        public async Task<bool> Exist(string Name)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM Accounts WHERE Name = @Name;", new { Name });
            return exist == 1;
        }


        //QUERY DEL INDEX
        //Obtenemos los datos de la tabla que son llamadas en el Views
        //Con esta query dentro de un foreach generamos el listado en Views
        public async Task<IEnumerable<Accounts>> getAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, Balance, Description FROM Accounts");
        }


        //QUERY DEL MODIFY
        public async Task Modify(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts SET Name = @Name, AccountTypeId=@AccountTypeId, Balance=@Balance, Description=@Description
                                            WHERE Id = @Id", accounts);
        }


        //QUERY DE LA VISTA DEL MODIFY
        public async Task<Accounts> getAccountsById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, Balance, Description
                                                                         FROM Accounts WHERE Id = @Id", new { Id });
        }


        //QUERY DEL DELETE
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { Id });
        }
       
    }
}
