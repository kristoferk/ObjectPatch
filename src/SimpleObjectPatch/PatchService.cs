using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleObjectPatch
{
    public class PatchService
    {
        private readonly JsonSerializer _serializer;

        public PatchService(JsonSerializer serializer = null)
        {
            _serializer = serializer ?? new JsonSerializer();
        }

        public T ApplyPatch<T>(JObject input, decimal? version, T original, params Expression<Func<T, object>>[] actions) where T : class, new()
        {
            PropertyInfo[] allPropertiesOnType = typeof(T).GetTypeInfo().DeclaredProperties.ToArray();
            T objectFromInput = input.ToObject<T>(_serializer);


            var propertyNames = GetPatchablePropertyNames(version, actions, allPropertiesOnType);

            //Properties in dynamic object
            foreach (var prop in input.Properties())
            {
                string propertyName = prop.Name;
                if (propertyNames.Contains(propertyName))
                {
                    var actualProperty = allPropertiesOnType.First(f => f.Name == propertyName);
                    if (actualProperty.CanWrite)
                    {
                        actualProperty.SetValue(original, actualProperty.GetValue(objectFromInput));
                    }
                }
            }

            return original;
        }

        private static List<string> GetPatchablePropertyNames<T>(decimal? version, Expression<Func<T, object>>[] actions, PropertyInfo[] allPropertiesOnType) where T : class, new()
        {
            var patchablePropertyNames = new List<string>();
            if (actions.Any())
            {
                patchablePropertyNames.AddRange(actions.Select(a => GetPropertyName(a.Body)).ToList());
            }
            else
            {
                foreach (PropertyInfo property in allPropertiesOnType)
                {
                    var patchable = property.GetCustomAttributes(typeof(PatchableAttribute), false).Cast<PatchableAttribute>().FirstOrDefault();
                    if (patchable == null || patchable.Patchable)
                    {
                        if (patchable != null && version.HasValue)
                        {                            
                            double versionAsDouble = (double)version.Value;

                            if (versionAsDouble < patchable.From)
                            {
                                continue;
                            }

                            if (versionAsDouble > patchable.To)
                            {
                                continue;
                            }
                        }
                        
                        patchablePropertyNames.Add(property.Name);
                    }
                }
            }

            return patchablePropertyNames;
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