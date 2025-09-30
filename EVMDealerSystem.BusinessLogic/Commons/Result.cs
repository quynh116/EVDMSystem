using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Commons
{
    public enum ResultStatus
    {
        Success,
        NotFound,
        Invalid,
        Conflict, // Thường dùng cho lỗi trùng lặp (Duplicated)
        Unauthorized, // Thay cho NotVerified và một số trường hợp Failed
        Forbidden, // Thêm nếu cần cho lỗi quyền truy cập
        InternalServerError // Thay cho Error và Failure
    }

    public class Result<T>
    {
        public T? Data { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string[]? Messages { get; set; }

        public bool IsSuccess => ResultStatus == ResultStatus.Success;

        public static Result<T> Success(T data, string message = "Success.")
            => new Result<T> { Data = data, ResultStatus = ResultStatus.Success, Messages = new[] { message } };

        public static Result<T> Success(string message = "Success.")
            => new Result<T> { ResultStatus = ResultStatus.Success, Messages = new[] { message } };

        public static Result<T> NotFound(string message = "Resource not found.")
            => new Result<T> { ResultStatus = ResultStatus.NotFound, Messages = new[] { message } };

        public static Result<T> Invalid(string message = "Invalid data.", params string[]? errors)
            => new Result<T> { ResultStatus = ResultStatus.Invalid, Messages = errors?.Any() == true ? errors : new[] { message } };

        public static Result<T> Conflict(string message = "Conflict detected.")
            => new Result<T> { ResultStatus = ResultStatus.Conflict, Messages = new[] { message } };

        public static Result<T> Unauthorized(string message = "Authentication failed.")
            => new Result<T> { ResultStatus = ResultStatus.Unauthorized, Messages = new[] { message } };

        public static Result<T> InternalServerError(string message = "An internal server error occurred.")
            => new Result<T> { ResultStatus = ResultStatus.InternalServerError, Messages = new[] { message } };
    }
}