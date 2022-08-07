namespace QuizingApi.Dtos.ChoosenQuestionAnswersDtos {
    public record ChoosenQAEssentials {

        public int questionID {get; init;}
        public string question {get; init;}
        public int answerID {get; init;}
        public string answer {get; init;}
        public bool correct {get; init;}
        public int examinationID {get; init;}
    }
}