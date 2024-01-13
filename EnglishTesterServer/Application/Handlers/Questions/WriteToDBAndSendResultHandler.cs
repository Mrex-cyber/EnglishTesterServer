using Connection;
using EnglishTesterServer.Application.Commands.Questions;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.Questions;
using EnglishTesterServer.Application.Queries.Tests;
using MediatR;
using System.Data;

namespace EnglishTesterServer.Application.Handlers.Questions
{
    public class WriteToDBAndSendResultHandler : IRequestHandler<GetAllAnswersQuery, IResult>
    {
        private readonly Connection.MsSql connection;
        public WriteToDBAndSendResultHandler()
        {
            connection = new MsSql(Program.Settings.ConnectionString);
        }
        public async Task<IResult> Handle(GetAllAnswersQuery request, CancellationToken cancellationToken)
        {
            int result = 0;

            List<AnswerVariant> rightAnswers = new List<AnswerVariant>();

            for (int i = 0; i < request.answersData.answers.Length; i++)
            {
                DataTable rightAnswerDt = await connection.getDataTable(GetRightAnswerByQuestionIdQuery.query, new List<SqlQueryParameter>()
                {
                    new SqlQueryParameter()
                    {
                        Parameter = "@questionId",
                        Value = request.answersData.answers[i].QuestionId,
                        Type = SqlDbType.Int
                    }
                });

                if (rightAnswerDt.Rows.Count > 0)
                {
                    AnswerVariant currentRightAnswer = await AnswerVariant.FillFromRow(rightAnswerDt.Rows[0]);
                    rightAnswers.Add(currentRightAnswer);
                }
            }
            
            foreach(AnswerVariant rightAnswer in rightAnswers)
            {
                if (request.answersData.answers.Where(a => a.QuestionId == rightAnswer.QuestionId && a.Id == rightAnswer.Id).Count() > 0)
                {
                    result++;
                }
            }

            if (request.answersData.userEmail is not null && request.answersData.userEmail != "")
            {
                await connection.executeNonQuery(WriteTestResultToDB.command, new List<SqlQueryParameter>()
                {
                    new SqlQueryParameter()
                    {
                        Parameter = "@userEmail",
                        Value = request.answersData.userEmail,
                        Type = SqlDbType.NVarChar
                    },
                    new SqlQueryParameter()
                    {
                        Parameter = "@testId",
                        Value = request.answersData.testId,
                        Type = SqlDbType.Int
                    },
                    new SqlQueryParameter()
                    {
                        Parameter = "@result",
                        Value = result,
                        Type = SqlDbType.Int
                    }
                });
            }

            return Results.Json(result);
        }
    }
}
