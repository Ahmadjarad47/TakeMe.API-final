using System.ComponentModel.DataAnnotations;

namespace TakeMe.DTOs
{
    public record RegisterDTO
       (


          [Required]
        [EmailAddress]
      string Email,
          [Required]
    string password

      );
}
