using System.ComponentModel.DataAnnotations;

namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionInsertDto {

        [Required(ErrorMessage = "question description is required")]
        [MinLength(3, ErrorMessage = "question descriptions minimum length must be longer than 3 characters")]
        public string question {get; init;}

        [Required]
        [Range(0, 5, ErrorMessage = "question difficulty must be between 0 and 5")]
        public int difficulty {get; init;}

        [Required(ErrorMessage = "question type is required")]
        public string questionType {get; init;}
        public bool? hasImage {get; init;} = false;
        public string? imgUrl {get; init;} = string.Empty;

        [Required(ErrorMessage = "exam id is required")]
        public int examID {get; init;}
    }
}