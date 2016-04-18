namespace Main
{
    using System;
    using System.Linq.Expressions;

    public static class Utilities
    {
        public static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            var memberExpression = property.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("The property must not be null.");
            }

            return memberExpression.Member.Name;
        }
    }
}
