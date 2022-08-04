namespace QuizingApi.Models {
    public record ExaminationModel {
        public int ID {get; init;}
        public int examID {get; init;}
        public int userID {get; init;}
        public DateTime atDate {get; init;}
        public int result {get; init;}
    }
}