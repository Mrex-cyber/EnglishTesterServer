using Connection;
using EnglishTesterServer.Application.Commands;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.Tests;
using EnglishTesterServer.Application.Queries.User;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using EnglishTesterServer.Application.Queries.Questions;

namespace EnglishTesterServer.Application.Handlers.Tests
{
    public class GetAllTestsHandler : IRequestHandler<GetAllTestsQuery, IResult>
    {
        private readonly Connection.MsSql connection;
        private GetAllTestsQuery? _request = null;
        public GetAllTestsHandler()
        {
            connection = new MsSql(Program.Settings.ConnectionString);
        }
        public async Task<IResult> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            if (request is null) return Results.BadRequest();
            _request = request;

            if (_request.userEmail is null)
            {
                return await GetAllCommonTests();
            }
            else
            {
                return await GetAllUserTests();
            }
        }
        public async Task<IResult> GetAllCommonTests()
        {
            DataTable testsDt = await connection.getDataTable(GetAllTestsQuery.query, new List<SqlQueryParameter>());

            List<Test> tests = new List<Test>();
            foreach (DataRow row in testsDt.Rows)
            {
                Test curTest = await Test.FillFromRow(row);
                tests.Add(curTest);
            }

            foreach(var test in tests)
            {
                DataTable questionsDt = await connection.getDataTable(GetQuestionsByTestId.query, new List<SqlQueryParameter>()
                {
                    new SqlQueryParameter()
                    {
                        Parameter = "@testId",
                        Value = test.Id,
                        Type = SqlDbType.Int
                    }
                });

                List<Question> questions = new List<Question>();
                foreach (DataRow row in questionsDt.Rows)
                {
                    Question curQuestion = await Question.FillFromRow(row);

                    List<AnswerVariant> answers = new List<AnswerVariant>();
                    DataTable answersDt = await connection.getDataTable(GetAnswersByQuestionId.query, new List<SqlQueryParameter>()
                    {
                        new SqlQueryParameter()
                        {
                            Parameter = "@questionId",
                            Value = curQuestion.Id,
                            Type = SqlDbType.Int
                        }
                    });

                    foreach(DataRow answerRow in answersDt.Rows)
                    {
                        AnswerVariant answer = await AnswerVariant.FillFromRow(answerRow);

                        answers.Add(answer);
                    }
                    curQuestion.Answers = answers.ToList();

                    questions.Add(curQuestion);
                }

                test.Questions = questions.ToList();
            }
            

            return Results.Json(tests);
            
        }
        public async Task<IResult> GetAllUserTests()
        {
            DataTable testsDt = await connection.getDataTable(GetAllUserTestsQuery.query, new List<SqlQueryParameter>()
            {
                new SqlQueryParameter()
                {
                    Parameter = "@userEmail",
                    Value = _request.userEmail,
                    Type = SqlDbType.NVarChar
                }
            });

            List<Test> tests = new List<Test>();
            foreach (DataRow row in testsDt.Rows)
            {
                Test curTest = await Test.FillFromRow(row);
                tests.Add(curTest);
            }

            foreach (var test in tests)
            {
                DataTable questionsDt = await connection.getDataTable(GetQuestionsByTestId.query, new List<SqlQueryParameter>()
                {
                    new SqlQueryParameter()
                    {
                        Parameter = "@testId",
                        Value = test.Id,
                        Type = SqlDbType.Int
                    }
                });

                List<Question> questions = new List<Question>();
                foreach (DataRow row in questionsDt.Rows)
                {
                    Question curQuestion = await Question.FillFromRow(row);

                    List<AnswerVariant> answers = new List<AnswerVariant>();
                    DataTable answersDt = await connection.getDataTable(GetAnswersByQuestionId.query, new List<SqlQueryParameter>()
                    {
                        new SqlQueryParameter()
                        {
                            Parameter = "@questionId",
                            Value = curQuestion.Id,
                            Type = SqlDbType.Int
                        }
                    });

                    foreach (DataRow answerRow in answersDt.Rows)
                    {
                        AnswerVariant answer = await AnswerVariant.FillFromRow(answerRow);

                        answers.Add(answer);
                    }
                    
                    curQuestion.Answers = answers;

                    questions.Add(curQuestion);
                }

                test.Questions = questions;
            }


            return Results.Json(tests);

        }
    }
}
