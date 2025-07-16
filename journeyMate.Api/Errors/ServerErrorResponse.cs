namespace journeyMate.Api.Errors
{
    public class ServerErrorResponse:ErrorResponse
    {
        public string? Details { get; set; }
        public ServerErrorResponse(int code, string? message = null, string? details = null) : base(code, message)
        {
            Details = details;
        }
    }
}
