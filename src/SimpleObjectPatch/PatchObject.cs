using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleObjectPatch
{
    [JsonObject]
    public class PatchObject<T> where T : class, new()
    {
        private readonly IActionContextAccessor _httpContextAccessor;

        public PatchObject(IActionContextAccessor request)
        {
            _httpContextAccessor = request;
        }

        public async Task<T> CreateAsync(params Expression<Func<T, object>>[] actions)
        {
            var original = Activator.CreateInstance<T>();
            var reader = new StreamReader(_httpContextAccessor.ActionContext.HttpContext.Request.Body);
            string content = await reader.ReadToEndAsync();
            JObject json = JObject.Parse(content);
            return new PatchService().ApplyPatch(json, original, actions);
        }

        public async Task<T> PatchAsync(T original, params Expression<Func<T, object>>[] actions)
        {
            var reader = new StreamReader(_httpContextAccessor.ActionContext.HttpContext.Request.Body);
            string content = await reader.ReadToEndAsync();
            JObject json = JObject.Parse(content);
            return new PatchService().ApplyPatch(json, original, actions);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PatchableAttribute : Attribute
    {
    }
}