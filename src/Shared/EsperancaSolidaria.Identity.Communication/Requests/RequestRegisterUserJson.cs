namespace EsperancaSolidaria.Identity.Communication.Requests
{
    public class RequestRegisterUserJson
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
