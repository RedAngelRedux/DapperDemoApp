namespace BlazorDemo.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadDataAsync<T, P>(string storedProcedure, P parameters, string connectionStringNAme = "Default");
        Task<int> SaveDataAsync<P>(string storedProcedure, P parameters, string connectionStringNAme = "Default");
    }
}