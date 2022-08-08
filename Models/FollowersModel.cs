namespace QuizingApi.Models {
    public record FollowersModel {
        public int ID {get; init;}
        public int userID {get; init;}
        public int followerID {get; init;}
        public int pending {get; init;}
        public int accepted {get; init;}
        public DateTime requestDate {get; init;}
    }
}