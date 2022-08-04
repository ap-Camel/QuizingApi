using System.ComponentModel.DataAnnotations;

namespace QuizingApi.Dtos.WebUserDtos {
    public record WebUserLoginDto {
        [Required]
        public string email {get; init;}
        [Required]
        public string password {get; init;}
    }
}