using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleObjectPatch.Extensions;
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
        private readonly decimal? _version;
        public T Data { get; set; }

        public PatchObject(object input) : base(FromObject(input))
        {
            Data = Create();
        }

        public PatchObject(object input, decimal? version) : base(FromObject(input))
        {
            _version = version;
            Data = CreateForVersion(version);
        }

        public PatchObject(JObject obj) : base(obj)
        {
            Data = Create();
        }

        //, string version

        public T Create(params Expression<Func<T, object>>[] actions)
        {
            return CreateForVersion(null, actions);
        }

        public T CreateForVersion(decimal? version, params Expression<Func<T, object>>[] actions)
        {
            var original = Activator.CreateInstance<T>();
            return new PatchService().ApplyPatch(this, version, original, actions);
        }

        public T Patch(T original, params Expression<Func<T, object>>[] actions)
        {
            T outObject = original.Copy();
            return new PatchService().ApplyPatch(this, _version, outObject, actions);
        }

        public void Patch(ref T original, params Expression<Func<T, object>>[] actions)
        {
            new PatchService().ApplyPatch(this, _version, original, actions);
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