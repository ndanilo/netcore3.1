namespace Models.DTO.Generics
{
    public class Error
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string StackTrace { get; set; }
    }
}
