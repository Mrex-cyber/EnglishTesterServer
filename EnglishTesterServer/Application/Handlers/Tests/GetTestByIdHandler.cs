using Connection;
using EnglishTesterServer.Application.Commands;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.Tests;
using EnglishTesterServer.Application.Queries.User;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTesterServer.Application.Handlers.Tests
{
    public class GetTestByIdHandler : IRequestHandler<GetTestById, IResult>
    {
        private readonly Connection.MsSql connection;
        private GetTestById? _request;
        public GetTestByIdHandler()
        {
            connection = new MsSql(Program.Settings.ConnectionString);
        }
        public async Task<IResult> Handle(GetTestById request, CancellationToken cancellationToken)
        {
            DataTable dt = await connection.getDataTable(GetTestById.query, new List<SqlQueryParameter>()
            {
                new SqlQueryParameter()
                {
                    Parameter = "@id",
                    Value = _request!.testId,
                    Type = SqlDbType.Int
                }
            });

            if (dt.Rows.Count == 0) return Results.NotFound("Object can not be found");
            else
            {
                Test foundTest = await Test.FillFromRow(dt.Rows[0]);

                return Results.Json(foundTest);
            }
        }
    }
}
