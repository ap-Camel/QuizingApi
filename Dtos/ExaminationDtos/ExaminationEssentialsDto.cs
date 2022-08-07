namespace QuizingApi.Dtos.ExaminationDtos {
    public record ExaminationEssentialsDto {
        public int ID {get; init;}
        public int examID {get; init;}
        public string examTitle {get; init;}
        public DateTime atDate {get; init;}
        public int result {get; init;}
    }
}