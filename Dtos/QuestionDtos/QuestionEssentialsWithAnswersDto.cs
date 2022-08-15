using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Models;

namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionEssentialsWithAnswersDto {
        public QuestionEssentialsDto question {get; set;}
        public IEnumerable<AnswerModel> answers {get; set;}
    }
}