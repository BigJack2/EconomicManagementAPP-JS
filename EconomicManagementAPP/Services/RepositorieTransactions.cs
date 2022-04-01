using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Services
{
    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;

        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Query Asincrona del CREATE
        public async Task Create(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Transactions 
                                                               (UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId) 
                                                        VALUES (@UserId, @TransactionDate, @Total, @OperationTypeId, @Description, @AccountId, @CategoryId);
                                                        SELECT SCOPE_IDENTITY();", transactions);
            transactions.Id = id;
        }


        //QUERY DEL INDEX
        //Obtenemos los datos de la tabla que son llamadas en el Views
        //Con esta query dentro de un foreach generamos el listado en Views
        public async Task<IEnumerable<Transactions>> getTransactions()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(@"SELECT Id, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                               FROM Transactions");
        }



        //QUERY DEL MODIFY
        public async Task Modify(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Transactions
                                            SET UserId = @UserId,
                                                TransactionDate=@TransactionDate,
                                                Total=@Total,
                                                OperationTypeId=@OperationTypeId,
                                                Description=@Description,
                                                AccountId=@AccountId,
                                                CategoryId=@CategoryId
                                            WHERE Id = @Id", transactions);
        }


        //QUERY DE LA VISTA DEL MODIFY
        public async Task<Transactions> getTransactionsById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transactions>(@"SELECT Id, UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                                             FROM Transactions WHERE Id = @Id", new { Id });
        }


        //QUERY DEL DELETE
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Transactions WHERE Id = @Id", new { Id });
        }

    }
}
