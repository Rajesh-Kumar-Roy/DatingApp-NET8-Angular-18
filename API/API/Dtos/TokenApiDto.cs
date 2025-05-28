namespace API.Dtos
{
    public class TokenApiDto
    {
        public required string RefreshToken { get; set; }
        public required string AccessToken { get; set; }
    }
}
