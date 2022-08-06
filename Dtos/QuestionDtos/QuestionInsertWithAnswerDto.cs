using QuizingApi.Dtos.AnswerDtos;

namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionInsertWithAnswerDto {
        public QuestionInsertDto question {get; init;}
        public List<AnswerInsertWithQuestionDto> answers {get; init;}
    }
}