using System.ComponentModel.DataAnnotations;
using QuizingApi.Dtos.QuestionDtos;

namespace QuizingApi.Dtos.CustomeDtos {
    public record QuizRecieveDto {

        [Required]
        public int examID {get; init;}

        [Required]
        public List<QuestionEvaluateDto> quiz {get; init;}
    }
}