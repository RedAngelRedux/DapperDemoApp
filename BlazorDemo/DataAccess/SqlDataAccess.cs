using BlazorDemo.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorDemo.DataAccess;

public class SqlDataAccess(
    IConfiguration configuration) : ISqlDataAccess
{
    private readonly IConfiguration _config = configuration;

    public async Task<List<T>> LoadDataAsync<T, P>(string storedProcedure, P parameters, string connectionStringNAme = "Default")
    {
        string? connectionString = _config.GetConnectionString(connectionStringNAme);

        using IDbConnection db = new SqlConnection(connectionString);

        try
        {
            List<T> rows = [.. (await db.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure))];
            return rows;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SaveDataAsync<P>(string storedProcedure, P parameters, string connectionStringNAme = "Default")
    {
        string? connectionString = _config.GetConnectionString(connectionStringNAme);
        using IDbConnection db = new SqlConnection(connectionString);
        try
        {
            return await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}
