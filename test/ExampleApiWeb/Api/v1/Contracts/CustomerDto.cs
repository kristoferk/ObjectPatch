using SimpleObjectPatch;
using System;

namespace ExampleApiWeb.Api.v1.Contracts
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Patchable]
        public string Name { get; set; }

        [Patchable]
        public string Description { get; set; } = "Default description";

        public string Notes { get; set; } = "Default notes";

        [Patchable(false)]
        public string Alias { get; set; }

        [Patchable(1.5)]
        public string NewAttributeIn15 { get; set; }

        [Patchable(0, 1.5)]
        public string RemovedAttributeIn15 { get; set; }

        [Patchable(false)]
        public int Page { get; set; } = 1;
        
        public int PageSize { get; set; } = 1000;

        public TestType TestType { get; set; }

        public TestType? TestType2 { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTimeOffset? StartOffset { get; set; }

        public DateTimeOffset EndOffset { get; set; }

        public ObjectReference Parent { get; set; }
    }

    
    public class ObjectReference
    {
        public int Id { get; set; }

        [Patchable(false)]
        public int Name { get; set; }
    }

    public enum TestType
    {
        None = 0,
        Start = 1
    }
}