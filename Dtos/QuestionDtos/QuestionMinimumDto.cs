namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionMinimumDto {
        public int questionID {get; init;}
        public string question {get; init;}
        public List<string> answers {get; init;}
    }
}