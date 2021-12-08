using System;
using System.Collections.Generic;

namespace Brenda.Contracts.V1.Responses
{
    public static class Responses
    {
        public static Response Successful => new Response("successful");

        public static Response Id(Guid id) => new Response(new ResponseId(id));

        public static Response Redirect(string redirect)
        {
            return new Response(new { redirect });
        }

        public static Response Message(string message) => new Response(message);

        public static object Data(object obj)
        {
            return new Response(new { data = obj });
        }

        public static object Errors(List<string> errors)
        {
            return new ResponseError(errors);
        }
    }

    public struct ResponseId
    {
        private Guid _id;
        public ResponseId(Guid id)
        {
            _id = id;
        }

        public Guid Id { get { return _id; } }
    }

    public class ResponseError
    {
        public ResponseError(List<string> errors)
        {
            Errors = errors;
        }

        public string Result => "error";

        public List<string> Errors { get; }
    }

    public class Response
    {
        public Response() { }

        public Response(object response)
        {
            Result = response;
        }

        public object Result { get; set; }
    }

}
