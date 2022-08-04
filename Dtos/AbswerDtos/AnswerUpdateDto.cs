using System.ComponentModel.DataAnnotations;

namespace QuizingApi.Dtos.AnswerDtos {
    public record AnswerUpdateDto {

        [Required(ErrorMessage = "answer is is required")]
        public int ID {get; init;}

        [Required(ErrorMessage = "please specify if the answer is correct or not")]
        public bool correct {get; init;}

        [Required(ErrorMessage = "answer description is required")]
        [MinLength(1, ErrorMessage = "the answer must contain atleast 1 letter")]
        public string answer {get; init;}

        [Required(ErrorMessage = "please specify if the answer is active or not")]
        public bool active {get; init;} = true;

        [Required(ErrorMessage = "please specify if the answer has image or not")]
        public bool hasImage {get; init;} = false;

        [Required(ErrorMessage = "please specify if the image url")]
        public string imgUrl {get; init;} = string.Empty;

        [Required(ErrorMessage = "the question id is required")]
        public int questionID {get; init;}
    }
}