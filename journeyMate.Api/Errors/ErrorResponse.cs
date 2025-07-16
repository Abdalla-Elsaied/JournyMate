
namespace journeyMate.Api.Errors
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ErrorResponse(int StatusCode ,string? Message=null)
        {
           this.StatusCode = StatusCode;
           this.Message = Message??GetDefaultMessage(StatusCode); 
        }

        private string? GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                500 => "Servier Error ..are the path to the dark side",
                _ => null
            };
        }
    }
}
