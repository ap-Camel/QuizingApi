namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionEvaluateDto {
        public int questionID {get; init;}
        public string answer {get; init;}
    }
}