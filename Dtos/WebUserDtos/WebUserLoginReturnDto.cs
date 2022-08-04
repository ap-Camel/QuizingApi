namespace QuizingApi.Dtos.WebUserDtos {
    public record WebUserLoginReturnDto {
        public WebUserEssentialsDto webUser {get; init;}
        public string JWT {get; init;}
    }
}