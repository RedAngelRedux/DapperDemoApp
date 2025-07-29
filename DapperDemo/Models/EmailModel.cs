
namespace DapperDemo.Models;

public class EmailModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;

    // Navigation property
    public UserModel? User { get; set; }
}
