
using Core.Application.Exceptions;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public static CancellationToken CreateCancelationToken(int delayInMilliseconds = AppConstants.CommandMaxExecutionDelayInMilliseconds)
        {
            var cancellationTokenSource = new CancellationTokenSource(delayInMilliseconds);
            return cancellationTokenSource.Token;
        }

        public static IQueryable<TModel> Paginate<TModel>(this IQueryable<TModel> query, int? pageNumber, int? pageSize)
        {
            var ps = pageSize ?? AppConstants.PageSize;
            var pn = pageNumber ?? AppConstants.PageNumber;

            BusinessException.ThrowIfNull(query, "query can't be null");
            query.Skip(ps * (pn - 1)).Take(ps);
            return query;
        }

        public static async Task<SearchResultMetadata> GetResultMetadata<TModel>(
                this IQueryable<TModel> query,
                int? pageSize,
                CancellationToken cancellationToken)
        {
            var ps = pageSize ?? AppConstants.PageSize;
            var totalRecords = await query.CountAsync(cancellationToken);
            return SearchResultMetadata.Create(totalRecords, ps);
        }

        public static DateTime GetSystemDate()
        {
            return DateTime.Now;
        }

        public static ClaimsPrincipal GetUserByJwtToken(string jwtToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jwtToken, "Jwt token is missing");

            var validationParameters = GetValidationParameters(); // Set your validation parameters

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                jwtToken,
                validationParameters,
                out var validatedToken);

            return principal;

            static TokenValidationParameters GetValidationParameters()
            {
                return new TokenValidationParameters
                {

                };
            }
        }
    
        public static string Localize(string text, string? text2)
        {
            return AppConstants.CurrentLanguageIsArabic ? text : text2 ?? text;
        }
    }
}