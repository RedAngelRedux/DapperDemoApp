using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using DapperDemo.Modules;
using DapperDemo.Services;
using DapperDemo.Models;

// Prepare the console application to use Dapper with SQL Server
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .Build();


//// BASIC DAPPER USAGE
//string? connectionString = config.GetConnectionString("DefaultConnection");
//using IDbConnection db = new SqlConnection(connectionString);
//await  BasicDapper.SimulateSqlInjectionAttack(db);
//await  BasicDapper.SimpleDapperCall_RawSql(db);
//await  BasicDapper.SimpleDapperCall_StoredProcedure(db);
//await  BasicDapper.SimpleDapperCall_StoredProcedureWithParameter(db);
//await  BasicDapper.SimpleDapperCall_RawSql_UpdateLastName(db);
//await  BasicDapper.PreventSqlInjectionAttack(db);
//await  BasicDapper.DapperListSupport(db);
//await  BasicDapper.ReturnSingleRecord(db);
//await  BasicDapper.ReturnParameters(db);
//await  BasicDapper.OutputParameters(db);

// COMPLEX OBJECTS
//string? connectionString = config.GetConnectionString("DefaultConnection");
//using IDbConnection db = new SqlConnection(connectionString);
//await ComplexOjects.MultipleResults(db);
//await ComplexOjects.NestedObjects_OneToOne(db);
//await ComplexOjects.NestedObjects_OneToMany(db);

//// CODE SIMPLIFICATION
///
SqlDataAccess sqlDataAccess = new (config);

//await CodeSimplification.ShowTop10Users(sqlDataAccess);
//await CodeSimplification.ShowTop10Addresses(sqlDataAccess);
await CodeSimplification.UpdateUserName(sqlDataAccess);