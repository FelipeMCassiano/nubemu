namespace Errors;

public record BucketNameConflictError() : AppError("Bucket with this name already exists", ErrorType.ConflictError) { }
