namespace QuizingApi.Dtos.ExaminationDtos {
    public record ExaminationInsertDto {
        public int examID {get; init;}
        public int userID {get; init;}
        public DateTime atDate {get; init;}
        public int result {get; init;}
    }
}