using QuizingApi.Dtos.QuestionDtos;

namespace QuizingApi.Dtos.CustomeDtos {
    public record QuizSendDto {

        public int examID {get; init;}
        public List<QuestionMinimumDto> quiz {get; init;}
    }
}