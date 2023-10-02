namespace TakeMe.Core.DTOs
{
    public record TokenAPIDTO
   (
      string AccessToken,
      /*string RefreshToken*/
      string Name,
      string Email,
    int statusCode
   );

}
      
