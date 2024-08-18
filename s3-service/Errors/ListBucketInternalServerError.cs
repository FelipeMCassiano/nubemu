namespace Errors;

public record ListBucketInternalServerError() : AppError("Error at listing buckets", ErrorType.InternalError);
