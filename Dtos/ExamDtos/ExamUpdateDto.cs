namespace QuizingApi.Dtos.ExamDtos {
    public record ExamUpdateDto {
        public string title {get; init;}
        public int numOfQuestions {get; init;}
        public int duration {get; init;}
        public bool active {get; init;}
        public int difficulty {get; init;}
    }
}