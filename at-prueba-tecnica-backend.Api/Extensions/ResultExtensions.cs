using at_prueba_tecnica_backend.Api.Responses;
using Vali_Mediator.Core.Result;

namespace at_prueba_tecnica_backend.Api.Extensions;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return ApiResponse<T>.Ok(result.Value!);

        return ApiResponse<T>.Fail(result.Error!, result.ValidationErrors);
    }

    public static ListResponse<T> ToListResponse<T>(this Result<List<T>> result)
    {
        if (result.IsSuccess)
            return ListResponse<T>.Ok(result.Value!.AsReadOnly());

        return ListResponse<T>.Fail(result.Error!, result.ValidationErrors);
    }

    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this Result<(List<T> Data, int Total)> result,
        int page,
        int pageSize)
    {
        if (result.IsSuccess)
        {
            var (data, total) = result.Value;
            return PaginatedResponse<T>.Ok(data.AsReadOnly(), page, pageSize, total);
        }

        return PaginatedResponse<T>.Fail(result.Error!, result.ValidationErrors);
    }
}
