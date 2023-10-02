namespace TakeMe.Error
{
    public class APIException : BaseComonentResponse
    {
        public APIException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
