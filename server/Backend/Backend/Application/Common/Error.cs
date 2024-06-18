namespace Backend.Application.Common
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType ErrorType { get; }
        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) =>
          new(code, description, ErrorType.Conflict);

        public static Error Validation(string code, string description) =>
          new(code, description, ErrorType.Validation);
        public static Error Auth(string code, string description) =>
          new(code, description, ErrorType.Auth);

        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Conflict);
    }


    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess == true && error != Error.None ||
                isSuccess == false && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result<T> Success<T>(T value) =>  new (value, true,  Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<T> Failure<T>(Error error) => new (default,false, error);
    }

    public class Result<T> : Result
    {
        private T _value;

        protected internal Result(T value ,bool isSuccess, Error error) 
            : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess
            ? _value
            : throw new InvalidOperationException();
    }

    public enum ErrorType
    {
        Conflict = 0,
        Validation = 1,
        NotFound = 2,
        Auth = 3
    }
}
