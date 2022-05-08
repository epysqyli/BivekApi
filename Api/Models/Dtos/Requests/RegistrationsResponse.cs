namespace Api.Models
{
    public class RegistrationResponse
        {
        public string Token { get; set; }
        public bool Result { get; set; }

        public List<string> Errors { get; set; }
    }
}