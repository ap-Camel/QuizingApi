namespace QuizingApi.Models {
    public record AnswerModel {
        public int ID {get; init;}
        public bool correct {get; init;}
        public string answer {get; init;}
        public bool active {get; init;}
        public DateTime dateCreated {get; init;}
        public DateTime dateUpdated {get; init;}
        public bool hasImage {get; init;}
        public string imgUrl {get; init;}
        public int questionID {get; init;}
    }
}