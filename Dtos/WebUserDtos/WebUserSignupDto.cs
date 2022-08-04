namespace QuizingApi.Dtos.WebUserDtos {
    public record WebUserSignupDto {
        public string firstName {get; init;}
        public string lastName {get; init;}
        public string userName {get; init;}
        public string email {get; init;}
        public string userPassword {get; init;}
    }
}