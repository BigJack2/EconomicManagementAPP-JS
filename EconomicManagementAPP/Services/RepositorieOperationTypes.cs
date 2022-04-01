using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Services
{
    public class RepositorieOperationTypes : IRepositorieOperationTypes
    {
        private readonly string connectionString;

        public RepositorieOperationTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Query Asincrona del CREATE
        public async Task Create(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO OperationTypes (Description) 
                                                                VALUES (@Description); SELECT SCOPE_IDENTITY();", operationTypes);
            operationTypes.Id = id;
        }


        //Query que genera un boleano que confirma o no la existencia mediante el dato comparado
        //Esta consulta de validacion se llama desde el controller 
        public async Task<bool> Exist(string Description)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM OperationTypes
                                                                         WHERE Description = @Description;", new { Description });
            return exist == 1;
        }


        //QUERY DEL INDEX
        //Obtenemos los datos de la tabla que son llamadas en el Views
        //Con esta query dentro de un foreach generamos el listado en Views
        public async Task<IEnumerable<OperationTypes>> getOperationTypes()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<OperationTypes>(@"SELECT Id, Description FROM OperationTypes");
        }


        //QUERY DEL MODIFY
        public async Task Modify(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE OperationTypes SET Description = @Description
                                            WHERE Id = @Id", operationTypes);
        }


        //QUERY DE LA VISTA DEL MODIFY
        public async Task<OperationTypes> getOperationTypesById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<OperationTypes>(@"SELECT Description FROM OperationTypes
                                                                               WHERE Id = @Id", new { Id });
        }


        //QUERY DEL DELETE
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE OperationTypes WHERE Id = @Id", new { Id });
        }

    }


}
