
using Core.Application.Exceptions;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Core.Application.Models
{
    public static class AppUtilities
    {
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
            return query.Skip(ps * (pn - 1)).Take(ps);
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
            ArgumentException.ThrowIfNullOrWhiteSpace(jwtToken);

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
            return IsCurrentLanguageArabic() ? text : text2 ?? text;
        }

        public static bool IsCurrentLanguageArabic()
        {
            var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return 0 == string.Compare(language, "ar", true);
        }

        internal static string GetFieldName<TRequest>(Expression<Func<TRequest, object>> field)
        {
            if (field.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else
            {
                return field.Body is UnaryExpression unaryExpression &&
                    unaryExpression.Operand is MemberExpression innerMemberExpression
                ? innerMemberExpression.Member.Name
                : throw new ArgumentException("Expression is not a valid member access expression.", nameof(field));
            }
        }


        public static string Format(this string str, params Expression<Func<string, object>>[] args)
        {
            var parameters = args.ToDictionary(
                e => string.Format("{{{0}}}", e.Parameters[0].Name), 
                e => e.Compile()(e.Parameters[0].Name));

            var sb = new StringBuilder(str);
            foreach (var kv in parameters)
            {
                sb.Replace(kv.Key, kv.Value != null ? kv.Value.ToString() : "");
            }

            return sb.ToString();
        }
    }
}