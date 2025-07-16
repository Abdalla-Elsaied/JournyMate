namespace journeyMate.Api.Errors
{
    public class ErrorValidationResponse: ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ErrorValidationResponse():base(400)
        {
            Errors = new List<string>(); 
        }
    }
}
