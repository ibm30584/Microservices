using Core.Application.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Core.Application.Models
{
    public static class FluentBusiness
    {
        public static TModel? Null<TModel>([AllowNull] this TModel model, string errorMessage)
        {
            return model is null ? model : throw new BusinessException(errorMessage);
        }
        public static TModel NotNull<TModel>([NotNull] this TModel model, string errorMessage)
        {
            return model is not null ? model : throw new BusinessException(errorMessage);
        }
        public static TModel MustExist<TModel>([NotNull] this TModel model, string errorMessage)
        {
            return model is not null ? model : throw new BusinessException(errorMessage) { ErrorCode = Enums.ErrorCodes.NotFound };
        }
        public static TModel Must<TModel>(this TModel model, Expression<Func<TModel, bool>> expression, string errorMessage)
        {
            try
            {
                var check = expression.Compile();
                return check(model) ? model : throw new BusinessException(errorMessage);
            }
            catch (ProcessingException)
            {
                throw;
            }
        }

        //public static TModel Must<TModel>(this TModel model, params (Expression<Func<TModel, bool>> expression, string errorMessage)[] rules)
        //{
        //    var businessException = new BusinessException();
        //    foreach (var (expression, errorMessage) in rules)
        //    {
        //        try
        //        {

        //        }
        //        catch (Exception)
        //        {
        //            businessException.Details
        //            throw;
        //        }
        //    }
        //}
    }
}
