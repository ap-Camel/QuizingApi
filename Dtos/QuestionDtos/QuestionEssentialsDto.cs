namespace QuizingApi.Dtos.QuestionDtos {
    public record QuestionEssentialsDto {
        public int ID {get; init;}
        public string question {get; init;}
        public int difficulty {get; init;}
        public DateTime dateCreated {get; init;}
        public DateTime dateUpdated {get; init;}
        public string questionType {get; init;}
        public bool hasImage {get; init;}
        public string imgUrl {get; init;}
    }
}