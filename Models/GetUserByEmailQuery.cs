using System.ComponentModel.DataAnnotations;

namespace UserApiTest.Models;

public class GetUserByEmailQuery
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}