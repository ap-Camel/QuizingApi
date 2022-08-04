using QuizingApi.Dtos.AnswerDtos;
using QuizingApi.Dtos.ExamDtos;
using QuizingApi.Dtos.QuestionDtos;
using QuizingApi.Dtos.WebUserDtos;
using QuizingApi.Models;

namespace QuizingApi.Helpers {
    public class Converting {
        public static WebUserEssentialsDto toWebUserEssintials(WebUser user) {
            return new WebUserEssentialsDto {
                firstName = user.firstName,
                lastName = user.lastName,
                userName = user.userName,
                email = user.email
            };
        }

        public static ExamEssentialsDto toExamEssentials(ExamModel exam) {
            return new ExamEssentialsDto {
                title = exam.title,
                numOfQuestions = exam.numOfQuestions,
                duration = exam.duration,
                active = exam.active,
                difficulty = exam.difficulty,
                dateCreated = exam.dateCreated,
                dateUpdated = exam.dateUpdated

            };
        }

        public static QuestionEssentialsDto toQuestionEssentials(QuestionModel q) {
            return new QuestionEssentialsDto {
                ID = q.ID,
                question = q.question,
                difficulty = q.difficulty,
                dateCreated = q.dateCreated,
                dateUpdated = q.dateUpdated,
                questionType = q.questionType,
                hasImage = q.hasImage,
                imgUrl = q.imgUrl
            };
        }

        public static AnswerEssentialsDto toAnswerEssentials(AnswerModel a) {
            return new AnswerEssentialsDto {
                ID = a.ID,
                correct = a.correct,
                answer = a.answer,
                active = a.active,
                dateCreated = a.dateCreated,
                dateUpdated = a.dateUpdated,
                hasImage = a.hasImage,
                imgUrl = a.imgUrl,
                questionID = a.questionID
            };
        }
    }
}