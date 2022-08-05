namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionMinimumDto {
        public string question {get; init;}
        public List<string> answers {get; init;}
    }
}