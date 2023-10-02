namespace TakeMe.Error
{
    public class BaseComonentResponse
    {
        public BaseComonentResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultMessage(statusCode);
        }

        private string DefaultMessage(int statuscode)
        => statuscode switch
        {
            200=>"Done !",
            400 => "Bad Request",
            401 => "Not Authorized",
            404 => "Not Found",
            500 => "Server Error",
            _ => null
        };

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
