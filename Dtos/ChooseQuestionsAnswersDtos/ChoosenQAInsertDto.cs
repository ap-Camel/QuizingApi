namespace QuizingApi.DtosChoosenQuestionAnswersDtos {
    public record ChoosenQAInsertDto {
        public int questionID {get; init;}
        public int answerID {get; init;}
        public int examinationID {get; init;}
    }
}