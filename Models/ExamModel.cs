namespace QuizingApi.Models{
    public record ExamModel {
        public int ID {get; init;}
        public string title {get; init;}
        public int numOfQuestions {get; init;}
        public int duration {get; init;}
        public bool active {get; init;}
        public int difficulty {get; init;}
        public DateTime dateCreated {get; init;}
        public DateTime dateUpdated {get; init;}
        public string imgURL {get; init;}
        public int userID {get; init;}
    }
}