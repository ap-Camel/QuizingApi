using System.ComponentModel.DataAnnotations;

namespace QuizingApi.Dtos.ExamDtos {
    public record ExamInsertDto {

        [Required(ErrorMessage = "exam title is required")]
        [MinLength(2, ErrorMessage = "title length must be two or more character")]
        public string title {get; init;}

        [Required(ErrorMessage = "number of questions is required")]
        [Range(1, 150, ErrorMessage = "number of questions must be between 1 and 150")]
        public int numOfQuestions {get; init;}

        [Required(ErrorMessage = "exam duration is required")]
        [Range(0, 240, ErrorMessage = "exam duration must be between 0 and 240 minutes")]
        public int duration {get; init;}

        [Required (ErrorMessage = "exam active is required")]
        public bool active {get; init;}

        [Required(ErrorMessage = "exam difficulty is required")]
        [Range(0, 5, ErrorMessage = "exam difficulty must be between 0 and 5")]
        public int difficulty {get; init;}
    }
}