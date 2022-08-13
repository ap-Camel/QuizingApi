namespace QuizingApi.Dtos.ExamDtos {
    public record ExamSearchReturnDto {
        public int ID {get; init;}
        public string title {get; init;}
        public int duration {get; init;}
        public int difficulty {get; init;}
        public string imgURL {get; init;}
        public int count {get; init;}
    }
}