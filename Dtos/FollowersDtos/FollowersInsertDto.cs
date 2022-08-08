namespace QuizingApi.Dtos.FollowersDtos {
    public record FollowersInsertDto {
        public int userID {get; init;}
        public int followerID {get; init;}
    }
}