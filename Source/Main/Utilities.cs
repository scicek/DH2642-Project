namespace Main
{
    using System;
    using System.Linq.Expressions;

    public static class Utilities
    {
        /// <summary>
        /// Gets the name of the given property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>The name of the given property.</returns>
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
