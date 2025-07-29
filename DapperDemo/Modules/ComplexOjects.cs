using Dapper;
using DapperDemo.Models;
using System.Data;

namespace DapperDemo.Modules;

public static class ComplexOjects
{

    public static async Task MultipleResults(IDbConnection db)
    {
        string sql = "";

        sql = "SELECT * FROM [dbo].[Users]; SELECT * FROM [dbo].[Addresses];";

        using var allQueries = await db.QueryMultipleAsync(sql);

        var users = await allQueries.ReadAsync<UserModel>();
        var addresses = await allQueries.ReadAsync<AddressModel>();

        foreach (var u in users) Console.WriteLine($"User: {u.FirstName} {u.LastName}");

        foreach (var a in addresses) Console.WriteLine($"Address: {a.StreetAddress}, {a.City}, {a.State}, {a.ZipCode}");
    }

    public static async Task NestedObjects(IDbConnection db)
    {
        string sql = "SELECT u.*, a.* FROM [dbo].[Users] u INNER JOIN [dbo].[Addresses] a ON u.Id = a.UserId";

        var users = await db.QueryAsync<UserModel, AddressModel, UserModel>(
            sql,
            (user, address) =>
            {
                user.Address = address;
                return user;
            },
            splitOn: "Id" // Assuming Id is the primary key of AddressModel
        );

        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            if (user.Address != null)
            {
                Console.WriteLine($"Address: {user.Address.StreetAddress}, {user.Address.City}, {user.Address.State}, {user.Address.ZipCode}");
            }
        }
    }

    public static async Task NestedObjects_OneToMany(IDbConnection db)
    {
        string sql = "SELECT * FROM [dbo].[Users] WHERE Id IN (SELECT DISTINCT [UserId] FROM [dbo].[Email]); SELECT * FROM [dbo].Email";

        using var allQueries = await db.QueryMultipleAsync(sql);

        var users = await allQueries.ReadAsync<UserModel>();
        var emails = await allQueries.ReadAsync<EmailModel>();

        var groupedEmails = emails.GroupBy(e => e.UserId).ToDictionary(grp => grp.Key, grp => grp.ToList());

        foreach (var user in users)
        {
            if (groupedEmails.TryGetValue(user.Id, out var userEmails))
            {
                user.Emails = userEmails;
            }            
        }

        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            if (user.Emails != null)
            {
                foreach (var email in user.Emails)
                {
                    Console.WriteLine($"    Email: {email.EmailAddress}");
                }
            }
        }
    }
}
