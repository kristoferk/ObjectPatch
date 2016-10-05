using SimpleObjectPatch;
using System;

namespace ExampleApiWeb.Model
{
    public class Customer
    {
        public int Id { get; set; }

        [Patchable]
        public string Name { get; set; }

        [Patchable]
        public string Description { get; set; } = "Default description";

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
    }

    public enum TestType
    {
        None = 0,
        Start = 1
    }
}