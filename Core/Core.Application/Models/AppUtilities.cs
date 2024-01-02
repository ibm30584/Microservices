
using Core.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Models
{
    public static class AppUtilities
    {
        public static bool IsArabic(string language)
        {
            return string.IsNullOrWhiteSpace(language) || language.ToLower().IndexOfAny(["ar", "arabic"]) != -1;
        }

        public static int IndexOfAny(this string input, string[] searchStrings)
        {
            var indices = searchStrings.Select(s => input.IndexOf(s)).Where(i => i != -1);

            return indices.Any() ? indices.Min() : -1;
        }

        public static CancellationToken CreateCancelationToken(int delayInMilliseconds = AppConstants.CommandMaxExecutionDelayInMiliseconds)
        {
            var cancellationTokenSource = new CancellationTokenSource(delayInMilliseconds);
            return cancellationTokenSource.Token;
        }

        public static IQueryable<TModel> Paginate<TModel>(this IQueryable<TModel> query, int pageNumber, int pageSize)
        {
            BusinessException.ThrowIfNull(query, "query can't be null");
            query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return query;
        }

        public static async Task<SearchResultMetadata> GetResultMetadata<TModel>(
                this IQueryable<TModel> query,
                int pageSize,
                CancellationToken cancellationToken)
        {
            var totalRecords = await query.CountAsync(cancellationToken);
            return SearchResultMetadata.Create(totalRecords, pageSize);
        }

        public static DateTime GetSystemDate()
        {
            return DateTime.Now;
        }
    }
}