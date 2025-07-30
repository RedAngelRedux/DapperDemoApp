namespace BlazorDemo.Models;

public class UserModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressModel? Address { get; set; }
    public List<EmailModel>? Emails { get; set; }
}
