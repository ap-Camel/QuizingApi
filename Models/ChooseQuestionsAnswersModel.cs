namespace QuizingApi.Models {
    public record ChooseQuestionsAnswersModel {
        public int ID {get; init;}
        public int questionID {get; init;}
        public int answerID {get; init;}
        public int examinationID {get; init;}
    }
}