using System;

namespace Models.DTO.Exceptions
{
    public class BasicException : Exception
    {
        public int ERROR_CODE { get; set; }
        public BasicException(string message) : base(message) { }

        public BasicException(string message, Exception innerException) : base(message, innerException) { }

        public BasicException(int error, string message) : base(message)
        {
            ERROR_CODE = error;
        }

        public BasicException(int error, string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = error;
        }
    }
    public class NotFoundException : BasicException
    {
        public NotFoundException(string message) : base(message)
        {
            ERROR_CODE = 404;
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 404;
        }
    }

    public class BadRequestException : BasicException
    {
        public BadRequestException(string message) : base(message)
        {
            ERROR_CODE = 400;
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 400;
        }
    }
    public class ConflictException : BasicException
    {
        public ConflictException(string message) : base(message)
        {
            ERROR_CODE = 409;
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 409;
        }
    }
    public class NotAuthorizedException : BasicException
    {
        public NotAuthorizedException(string message) : base(message)
        {
            ERROR_CODE = 401;
        }

        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 401;
        }
    }

    public class ForbiddenException : BasicException
    {
        public ForbiddenException(string message) : base(message)
        {
            ERROR_CODE = 403;
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 403;
        }
    }

    public class UnsupportedMediaTypeException : BasicException
    {
        public UnsupportedMediaTypeException(string message) : base(message)
        {
            ERROR_CODE = 415;
        }

        public UnsupportedMediaTypeException(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 415;
        }
    }

    public class PreconditionFailed : BasicException
    {
        public PreconditionFailed(string message) : base(message)
        {
            ERROR_CODE = 412;
        }

        public PreconditionFailed(string message, Exception innerException) : base(message, innerException)
        {
            ERROR_CODE = 412;
        }
    }


    public class InternalServerError : Exception
    {
        public InternalServerError(string message) : base(message) { }
        public InternalServerError(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class ExceptionTypeGuard
    {
        public static bool IsDomainException(this Exception e)
        {
            return e is BasicException;
        }
    }

}

