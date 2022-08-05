using QuizingApi.Dtos.QuestionDtos;

namespace QuizingApi.Dtos.CustomeDtos {
    public record QuizRecieveDto {
        public int examID {get; init;}
        public List<QuestionMinimumDto> quiz {get; init;}
    }
}