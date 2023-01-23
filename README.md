# Quiz Api doc

# Api Description

- api is made with .NET core web api and C# using .NET version 6.0 LTS
- this api is for a simple quizing website [link here](https://quizingwebsite.azurewebsites.net/)
- api handles user authentication and authorization, also implements JWT.
- api handles manipulating of user, exam, question, answer, examination, and follower data.
- the api is secured with JWT.
- api is hosted on azure.

# Highlights

- each question for an exam has a difficulty level.
- each exam has a difficulty level and questions are chosen base on that difficulty.
- for an examination, questions are randomly chosen with still apllying the previous restrictions of choosing the right number of questions based on difficulty.
- time complexity of randomization algorithm is O(n).
- the results of each examination and which answers were chosen is saved after examinatoin.

# Dependencies 

- dapper is used for ORM of choice [link here](https://github.com/DapperLib/Dapper).
- using Microsoft.IdentityModel.Tokens [link here](https://www.nuget.org/packages/Microsoft.IdentityModel.Tokens/)
- using System.IdentityModel.Tokens.Jwt [link here](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/)
- using Microsoft.AspNetCore.Authentication.JwtBearer [link here](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
- using System.Data.SqlClient [link here](https://www.nuget.org/packages/System.Data.SqlClient)
- using Microsoft.AspNetCore.Authentication.JwtBearer [link here](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
- using Microsoft.AspNetCore.Authentication.OpenIdConnect [link here](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect)

# Database

- using MS SQL Server with SQL Server Managment Studio 2018

# Desclaimers

- the api is hosted on azure for free so it is slow.
- there is not code written to check for strings against sql injection attacks.
- the website does not implement all the functionality that the api provides due to development time restrictions.

# Controllers

## Auth Controller 

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/auth%20controller.png)

- this controller handles login and returns an object that contains a JWT.
- example return object

```json

{
    "webUser": {
        "firstName": "name",
        "lastName": "name",
        "userName": "username",
        "email": "name@user.com"
    },
    "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcm5hbWUiLCJJRCI6IjMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJuYW1lQHVzZXIuY29tIiwiZXhwIjoxNjY2Nzc5MjUxfQ.ioL4wfKBM3z6iy9x64tVD5iMtQPX2yV_21JKvJqA9x4"
}

```

## Quiz Controller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/Screenshot%202022-08-28%20064048.png)

- this controller handles generating and evaluating quizes.
- a generated quiz chooses questions randomly based on all the questions in the exam and the number of questins for the examination.
- the generated list of questions is based on the difficulty of the exam and difficulty of each question based on 5 difficulty levels: (very easy, easy, normal, hard, very hard).
- evaluated exam can not be evaluated agina.
- once exam generated, all data generated of user and the questions they got, after evaluation, the data is updated with which answers they chose.
- example of a generated examination:
``` json
{
    "examID": 25,
    "examinationID": 66,
    "quiz": [
        {
            "questionID": 46,
            "question": "generic very easy question 01",
            "answers": [
                "generic correct answer",
                "generic wrong answer",
                "generic wrong answer",
                "generic wrong answer"
            ]
        },
        {
            "questionID": 47,
            "question": "generic very easy question 02",
            "answers": [
                "generic correct answer",
                "generic wrong answer",
                "generic wrong answer",
                "generic wrong answer"
            ]
        }
    ],
    "duration": 30
}
```

## User controller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/Screenshot%202022-08-28%20064103.png)

- this controller handles manipulating the user data.
- handles adding, deleting, updating user information.
- handles getting user information.
- handles updating user's username


## Exam Controller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/exam%20controller.png)

- this controller handles manipulating exam data.
- handles adding, deleting, updating exam info.
- handles getting list of exams from search, top exams, username (all the exams of user), user(all exams of user making get request), id.
- deleting an exam deletes all questions and answers related to the exam.
- expample of a search result return request:

``` json
[
    {
        "id": 1,
        "title": "edit exam test 01",
        "duration": 60,
        "difficulty": 3,
        "imgURL": "https://www.viewstorm.com/wp-content/uploads/2014/10/default-img.gif",
        "count": 43
    },
    {
        "id": 20,
        "title": "dewar quiz",
        "duration": 60,
        "difficulty": 5,
        "imgURL": "https://www.eaie.org/.imaging/mte/eaie-theme/full-width-large/dam/images/blog-images/2019/1000x667_wall.jpg/jcr:content/1000x667_wall.jpg",
        "count": 20
    },
    {
        "id": 25,
        "title": "Generic Quiz",
        "duration": 30,
        "difficulty": 2,
        "imgURL": "https://www.viewstorm.com/wp-content/uploads/2014/10/default-img.gif",
        "count": 2
    }
]
```

## Questions Controller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/questions%20controller.png)

- this controller handles manipulating questions data.
- handles posting question by itself or with answers.
- handles getting question information, all questions of an exam, or all questions of a user, or question with its answers.
- deleting question deletes all its answers.
- example of posting a question with answers object
``` json

{
  "question": {
    "question": "string",
    "difficulty": 5,
    "questionType": "string",
    "hasImage": true,
    "imgUrl": "string",
    "examID": 0
  },
  "answers": [
    {
      "correct": true,
      "answer": "string",
      "active": true,
      "hasImage": true,
      "imgUrl": "string"
    }
  ]
}

```

## Answers COntroller 

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/actual%20answers%20controller.png)

- this controller handles manipulating answers data
- handles getting a specific answers, or all answers of a question
- handles inserting, updating, and deleting answer info


## Examination Controller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/examination%20controller.png)

- this controller handles getting info of all the examinations of the user, or detailed info of which questions and answers they have chosen for examination.

## Followers COntroller

![](https://github.com/ap-Camel/QuizingApi/blob/master/github-pictures/followers%20controller.png)

- this controller handles manupulating data for the user's followers and following
- can get list of the users in following, and the list of users who have followed the current user.
- can insert and delete from follower list.



