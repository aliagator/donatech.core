using System;
namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO para retornar valores desde los Repositorios y que permite
    /// validar si el resultado viene con datos o si viene una excepción de origen
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultDto<T>
    {
        public T? Result
        {
            get; set;
        }

        public ResultError? Error
        {
            get; set;
        }

        public bool HasError
        {
            get
            {
                return Error != null;
            }
        }

        public ResultDto(T? result = default, ResultError? error = null)
        {
            Result = result;
            Error = error;
        }
    }

    public class ResultError
    {
        public string? ErrorMessage
        {
            get; set;
        }

        public Exception? Exception
        {
            get; set;
        }

        public ResultError(string? errorMessage, Exception? exception = null)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }
    }
}

