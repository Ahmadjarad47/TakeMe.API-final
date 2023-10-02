namespace TakeMe.Error
{
    public class APIValidationError : BaseComonentResponse
    {
        public APIValidationError() : base(400)
        {
        }
        public IEnumerable<string> Error { get; set; }
    }
}
