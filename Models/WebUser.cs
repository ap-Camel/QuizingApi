namespace QuizingApi.Models {
    public record WebUser {
        public int ID {get; init;}
        public string firstName {get; init;}
        public string lastName {get; init;}
        public string userName {get; init;}
        public string email {get; init;}
        public string userPassword {get; init;}
        public DateTime dateCreated {get; init;}
    }
}