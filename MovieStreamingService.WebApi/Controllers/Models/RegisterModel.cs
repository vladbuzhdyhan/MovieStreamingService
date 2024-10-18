using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.WebApi.Controllers.Models
{
    public class RegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
    }
}
