using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;

namespace SimpleObjectPatch
{
    public class PatchObjectConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            return Activator.CreateInstance(objectType, jObject);            
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

    [JsonConverter(typeof(PatchObjectConverter))]
    public class PatchObject<T> : JObject where T : class, new()
    {
        public T Data { get; set; }

        public PatchObject(object obj) : base(FromObject(obj))
        {
            Data = Create();
        }

        public PatchObject(JObject obj) : base(obj)
        {
            Data = Create();
        }

        public T Create(params Expression<Func<T, object>>[] actions)
        {
            var original = Activator.CreateInstance<T>();
            return new PatchService().ApplyPatch(this, original, actions);
        }

        public T Patch(T original, params Expression<Func<T, object>>[] actions)
        {
            return new PatchService().ApplyPatch(this, original, actions);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PatchableAttribute : Attribute
    {
        public readonly bool Patchable;
        public readonly double? From;
        public readonly double? To;

        public PatchableAttribute(bool patchable=true)
        {
            Patchable = patchable;
            From = null;
            To = null;
        }

        public PatchableAttribute(double fromVersion, double toVersion)
        {
            Patchable = true;
            From = fromVersion;
            To = toVersion;
        }

        public PatchableAttribute(double fromVersion)
        {
            Patchable = true;
            From = fromVersion;
            To = null;
        }
    }
}