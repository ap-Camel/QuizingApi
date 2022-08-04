using System.ComponentModel.DataAnnotations;

namespace QuizingApi.Dtos.AnswerDtos {
    public record AnswerInsertDto {

        [Required(ErrorMessage = "please specify if the answer is correct or not")]
        public bool correct {get; init;}

        [Required(ErrorMessage = "answer description is required")]
        [MinLength(1, ErrorMessage = "the answer must contain atleast 1 letter")]
        public string answer {get; init;}


        public bool? active {get; init;} = true;
        public bool? hasImage {get; init;} = false;
        public string? imgUrl {get; init;} = string.Empty;

        [Required(ErrorMessage = "the question id is required")]
        public int questionID {get; init;}
    }
}