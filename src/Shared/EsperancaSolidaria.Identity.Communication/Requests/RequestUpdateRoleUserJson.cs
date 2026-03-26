using EsperancaSolidaria.Identity.Communication.Enums;

namespace EsperancaSolidaria.Identity.Communication.Requests
{
    public class RequestUpdateRoleUserJson
    {
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}