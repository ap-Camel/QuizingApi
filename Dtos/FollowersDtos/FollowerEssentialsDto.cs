namespace QuizingApi.Dtos.FollowersDtos {
    public record FollowerEssentialsDto {
        public string followerUsername {get; init;}
        public DateTime requestDate {get; init;}
    }
}