using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using DapperDemo.Models;
using DapperDemo.Services;

namespace DapperDemo.Modules;

public static class CodeSimplification
{
    public static async Task ShowTop10Users(SqlDataAccess sqlDataAccess)
    {
        var users = await sqlDataAccess.LoadDataAsync<UserModel, dynamic>(
            "dbo.spUsers_GetTop10",
            new { },
            "DefaultConnection");

        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
        }
    }

    public static async Task ShowTop10Addresses(SqlDataAccess sqlDataAccess)
    {
        var addresses = await sqlDataAccess.LoadDataAsync<AddressModel, dynamic>(
            "dbo.spAddresses_GetTop10",
            new { },
            "DefaultConnection");
        foreach (var address in addresses)
        {
            Console.WriteLine($"Address: {address.StreetAddress}, {address.City}, {address.State}, {address.ZipCode}");
        }
    }

    public static async Task UpdateUserName(SqlDataAccess sqlDataAccess)
    {
        int userId;
        string newFirstName;
        string newLastName;
        
        Console.Write("Enter User ID to update: ");
        while (!int.TryParse(Console.ReadLine(), out userId) || userId <= 0)
        {
            Console.Write("Invalid User ID. Please enter a valid positive integer: ");
        }

        Console.Write("Enter new First Name: ");
        newFirstName = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter new Last Name: ");
        newLastName = Console.ReadLine()?.Trim() ?? string.Empty;

        var parameters = new
        {
            Id = userId,
            FirstName = newFirstName,
            LastName = newLastName
        };
        
        int rowsAffected = await sqlDataAccess.SaveDataAsync(
            "dbo.spUsers_UpdateName",
            parameters,
            "DefaultConnection");

        Console.WriteLine($"{rowsAffected} row(s) updated.");
    }
}
