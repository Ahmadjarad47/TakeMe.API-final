using System.ComponentModel.DataAnnotations;

namespace TakeMe.DTOs
{
    public record AppUsersDTO
    (
      
        [Required]
        [EmailAddress]
      string Email,
        [Required]
    string password
    );
    
}
