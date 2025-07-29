using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Modules;

public static class BasicDapper
{
    /// <summary>
    /// Simply demonstrates how to use Dapper with raw SQL queries and stored procedures.
    /// </summary>
    public static async Task SimpleDapperCall_RawSql(IDbConnection db)
    {
        Console.WriteLine("Raw SQL Query Example");
        Console.WriteLine("====================================");

        string sql = "SELECT * FROM [dbo].[Users]";

        var users = await db.QueryAsync<UserModel>(sql);

        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
        }

        sql = "SELECT * FROM [dbo].[Addresses]";

        var addresses = await db.QueryAsync<AddressModel>(sql);

        Console.WriteLine("");
        foreach (var a in addresses)
        {
            Console.WriteLine($"Id: {a.Id}, {a.StreetAddress}, {a.City}, {a.State}, {a.ZipCode}");
        }
    }
    public static async Task SimpleDapperCall_StoredProcedure(IDbConnection db)
    {
        Console.WriteLine("\nStored Procedure Example");
        Console.WriteLine("====================================");

        string sql = "[dbo].[spUsers_GetTop10]";

        var users = await db.QueryAsync<UserModel>(sql, commandType: CommandType.StoredProcedure);

        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
        }

        sql = "[dbo].[spAddresses_GetTop10]";

        var addresses = await db.QueryAsync<AddressModel>(sql, commandType: CommandType.StoredProcedure);

        Console.WriteLine("");
        foreach (var a in addresses)
        {
            Console.WriteLine($"Id: {a.Id}, {a.StreetAddress}, {a.City}, {a.State}, {a.ZipCode}");
        }
    }

    public static async Task SimulateSqlInjectionAttack(IDbConnection db)
    {
        Console.WriteLine("\nSimulating SQL Injection Attack");
        Console.WriteLine("====================================");
        string userInput = "1; DROP TABLE Users; --"; // Simulated malicious input
        string sql = $"SELECT * FROM [dbo].[Users] WHERE Id = {userInput}";
        try
        {
            var users = await db.QueryAsync<UserModel>(sql);
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task PreventSqlInjectionAttack(IDbConnection db)
    {
        Console.WriteLine("\nTo Prevent a SQL Injection Attack Use Parameters!");
        Console.WriteLine("====================================\n");

        Console.Write("Enter a last name to search for: ");
        string? lastName = Console.ReadLine();

        string sql = "SELECT * FROM [dbo].[Users] WHERE Lastname = @LastName";
        try
        {
            var users = await db.QueryAsync<UserModel>(sql, new { LastName = lastName });
            if (users.Any() == false)
            {
                Console.WriteLine("No users found with that last name.");
                return;
            }
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task SimpleDapperCall_StoredProcedureWithParameter(IDbConnection db)
    {
        Console.WriteLine("\nCall Stored Procedure With Parameter");
        Console.WriteLine("====================================\n");

        Console.Write("Enter a last name to search for: ");
        string? lastName = Console.ReadLine();

        string sql = "[dbo].[spUsers_GetByLastName]";

        try
        {
            var users = await db.QueryAsync<UserModel>(sql, new { lastName }, commandType: CommandType.StoredProcedure);
            if (users.Any() == false)
            {
                Console.WriteLine("No users found with that last name.");
                return;
            }
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task SimpleDapperCall_RawSql_UpdateLastName(IDbConnection db)
    {
        Console.WriteLine("\nUpdate Last Name");
        Console.WriteLine("====================================\n");

        Console.Write("Enter the User Id to Change: ");
        string? userInput = Console.ReadLine();

        try
        {
            if (int.TryParse(userInput, out int Id))
            {
                Console.Write("Enter the new Last Name: ");
                string? lastName = Console.ReadLine();
                var recordCount = await db.ExecuteAsync("UPDATE [dbo].[Users] SET LastName = @LastName WHERE Id = @Id", new { LastName = lastName, Id });
                Console.WriteLine(($"Number of records updated: {recordCount}"));
            }
            else
            {
                Console.WriteLine("Invalid User Id.");
            }
        }
        catch (SqlException ex)
        {

            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task DapperListSupport(IDbConnection db)
    {
        Console.WriteLine("\nDemonstration of Dapper's List Support");
        Console.WriteLine("====================================\n");

        int[] ids = { 1, 3, 5, 7, 8, 21, 99 }; // Example list of user IDs
        string sql = "SELECT * FROM [dbo].[Users] WHERE [Id] IN @ids";
        var users = await db.QueryAsync<UserModel>(sql, new { ids });
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}: {user.FirstName} {user.LastName}");
        }

        string[] lastNames = { "Smith", "Quinteros", "Williams" };
        sql = "SELECT * FROM [dbo].[Users] WHERE LastName IN @lastNames";
        var names = await db.QueryAsync<UserModel>(sql, new { lastNames });

        Console.WriteLine("-------------------------------------------------\n");
        foreach (var u in names)
        {
            Console.WriteLine($"Id: {u.Id}: {u.LastName}");
        }
    }

    public static async Task ReturnSingleRecord(IDbConnection db)
    {
        Console.WriteLine("\nReturn a Single Record");
        Console.WriteLine("====================================\n");
        Console.Write("Enter a User Id to search for: ");
        string? userInput = Console.ReadLine();
        try
        {
            if (int.TryParse(userInput, out int Id))
            {
                string sql = "SELECT * FROM [dbo].[Users] WHERE Id = @Id";
                var user = await db.QuerySingleOrDefaultAsync<UserModel>(sql, new { Id });  // NOTE:
                                                                                            // QuerySinggleAsync() will thorow an exception if 0 or more than 1 records are found,
                                                                                            // while QuerySingleOrDefaultAsync() will return null.
                if (user != null)
                {
                    Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
                }
                else
                {
                    Console.WriteLine("No user found with that Id.");
                }
            }
            else
            {
                Console.WriteLine("Invalid User Id.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task ReturnParameters(IDbConnection db)
    {
        Console.WriteLine("\nReturn Parameters from a Stored Procedure");
        Console.WriteLine("------------------------------------\n");
        await SimpleDapperCall_RawSql(db);

        Console.WriteLine("====================================\n");
        Console.Write("Enter a last name to search for: ");
        string? lastName = Console.ReadLine();


        string sql = "[dbo].[spUsers_GetData]";

        DynamicParameters parameters = new();
        parameters.Add("@lastName", value: lastName, dbType: DbType.String, size: 50);
        parameters.Add("@recordCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        await db.ExecuteAsync(sql, parameters);
        int userCount = parameters.Get<int>("@recordCount");

        Console.WriteLine($"Total number of users: {userCount}");
    }

    public static async Task OutputParameters(IDbConnection db)
    {
        Console.WriteLine("\nOutput Parameters from a Stored Procedure");
        Console.WriteLine("------------------------------------\n");
        await SimpleDapperCall_RawSql(db);

        Console.Write("Enter a User Id: ");
        string? userInput = Console.ReadLine();
        try
        {
            if (int.TryParse(userInput, out int Id))
            {
                string sql = "[dbo].[spUsers_GetDataWithOutput]";

                DynamicParameters parameters = new();
                parameters.Add("@Id", value: Id, dbType: DbType.Int32);
                parameters.Add("@lastName", "Grijalva", dbType: DbType.String, size: 50);
                parameters.Add("@fullName", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parameters.Add("@returnCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                //await db.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                var users = await db.QueryAsync<UserModel>(sql, parameters, commandType: CommandType.StoredProcedure);

                int returnCount = parameters.Get<int>("@returnCount");
                if (returnCount == 0)
                {
                    Console.WriteLine("No user found with that Id.");
                }
                else
                {
                    var fullName = parameters.Get<string>("@fullName");
                    Console.WriteLine($"{returnCount} user found with the name {fullName}");
                }

                Console.WriteLine("------------------------------------\n");
                foreach (var user in users)
                {
                    Console.WriteLine($"Id: {user.Id}, {user.FirstName}, {user.LastName}");
                }
            }
            else
            {
                Console.WriteLine("Invalid User Id.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
