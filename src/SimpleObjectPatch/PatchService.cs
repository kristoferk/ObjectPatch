using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleObjectPatch
{
    public class PatchService
    {
        private readonly JsonSerializer _serializer;

        public PatchService(JsonSerializer serializer = null)
        {
            _serializer = serializer;
        }

        public T ApplyPatch<T>(JObject input, T original, params Expression<Func<T, object>>[] actions) where T : class, new()
        {
            var actualProperties = typeof(T).GetProperties();
            var obj = input.ToObject<T>(_serializer);

            var propertyNames = actions.Select(a => GetPropertyName(a.Body)).ToList();

            //Properties in dynamic object
            foreach (var prop in input.Properties())
            {
                string propertyName = prop.Name;
                if (propertyNames.Contains(propertyName))
                {
                    var actualProperty = actualProperties.First(f => f.Name == propertyName);
                    if (actualProperty.CanWrite)
                    {
                        actualProperty.SetValue(original, actualProperty.GetValue(obj));
                    }
                }
            }

            return original;
        }

        private static string GetPropertyName(Expression expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }

            MemberExpression body = expression as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = expression as UnaryExpression;
                if (ubody != null)
                {
                    body = ubody.Operand as MemberExpression;
                }
            }

            string propertyName = string.Empty;
            if (body != null)
            {
                propertyName = body.Member.Name;
            }

            return propertyName;
        }
    }
}