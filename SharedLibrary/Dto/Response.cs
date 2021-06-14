using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dto
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public ErrorDto Error { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            Response<T> response = new() { Data = data, StatusCode = statusCode, IsSuccessful = true };
            return response;
        }

        public static Response<T> Success(int statusCode)
        {
            Response<T> response = new() { Data = default, StatusCode = statusCode, IsSuccessful = true };
            return response;
        }

        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            Response<T> response = new() { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
            return response;
        }

        public static Response<T> Fail(string message, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(message, isShow);
            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}