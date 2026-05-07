using Ai_ShopBot.Core.Exntensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Ai_ShopBot.Core.DTOs
{
    public class PaginatedResponse<T> : BaseResponse<IEnumerable<T>> where T : class
    {

        public PaginatedResponse<T> ToValidationErrors(Dictionary<string, List<string>> errors, HttpStatusCode statusCode,
            string message)
        {
            return new PaginatedResponse<T>(message, statusCode, errors);
        }

        public PaginatedResponse()
        {

        }

        public PaginatedResponse(
            IEnumerable<T> items,
            int totalCount,
            int pageNumber,
            int pageSize,
            string? message = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = items;
            StatusCode = statusCode;
            Message = message;
        }

        public PaginatedResponse(
            string message,
            HttpStatusCode statusCode,
            Dictionary<string, List<string>>? errors = null)
        {
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }


        public static PaginatedResponse<T> Success(
            List<T> data,
            int count,
            int pageNumber,
            int pageSize,
            string? message = null)
        {
            return new PaginatedResponse<T>(data, count, pageNumber, pageSize, message);
        }

        public static Task<PaginatedResponse<T>> SuccessAsync(
            List<T> items,
            int totalCount,
            int pageNumber,
            int pageSize,
            string? message = null)
        {
            return Task.FromResult(new PaginatedResponse<T>(
                items: items,
                totalCount: totalCount,
                pageNumber: pageNumber,
                pageSize: pageSize,
                message: message));
        }

        public static PaginatedResponse<T> Failure(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            Dictionary<string, List<string>>? errors = null)
        {
            return new PaginatedResponse<T>(message, statusCode, errors);
        }

        public static Task<PaginatedResponse<T>> FailureAsync(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            Dictionary<string, List<string>>? errors = null)
        {
            return Task.FromResult(Failure(message, statusCode, errors));
        }

        public new static PaginatedResponse<T> ValidationFailure(
            List<ValidationFailure> validationFailures,
            string? message = null)
        {
            return new PaginatedResponse<T>(
                message ?? "Validation failed",
                HttpStatusCode.UnprocessableEntity,
                validationFailures.GetErrorsDictionary());
        }

        public new static PaginatedResponse<T> ValidationFailure(IEnumerable<IdentityError> errors)
        {
            return new()
            {
                Errors = errors.ToList().GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public new static PaginatedResponse<T> ValidationFailure(IEnumerable<IdentityError> errors, string message)
        {
            return new()
            {
                Errors = errors.ToList().GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message
            };
        }
    }
}
