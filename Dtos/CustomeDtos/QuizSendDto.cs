using QuizingApi.Dtos.QuestionDtos;

namespace QuizingApi.Dtos.CustomeDtos {
    public record QuizSendDto {

        public int examID {get; set;}
        public int examinationID {get; set;}
        public List<QuestionMinimumDto> quiz {get; set;}
    }
}