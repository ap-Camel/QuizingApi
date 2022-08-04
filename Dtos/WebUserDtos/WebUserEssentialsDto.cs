namespace QuizingApi.Dtos.WebUserDtos {
    public record WebUserEssentialsDto {
        public string firstName {get; init;}
        public string lastName {get; init;}
        public string userName {get; init;}
        public string email {get; init;}
    }
}