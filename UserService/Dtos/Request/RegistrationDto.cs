using System.ComponentModel.DataAnnotations;
using System.Text;
using UserService.Models;

namespace UserService.Dtos.Request;

public class RegistrationDto
{
    
    [Required]
    public string FirstName { set; get; }
    [Required]
    public string LastName{ set; get; }
    [Required]
    public string PhoneNumber{ set; get; }
    [Required]
    [EmailAddress]
    public string Email{ set; get; }
    [Required]
    [Length(8,32)]
    public string Password{ set; get; }

    public User CreateUser()
    {
        return new User()
        {
            FirstName = FirstName,
            Lastname = LastName,
            UserName = $"{FirstName}{LastName}",
            PhoneNumber = PhoneNumber,
            Email = Email
        };
    }
}