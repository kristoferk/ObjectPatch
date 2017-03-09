using System.Collections.Generic;

namespace ExampleApiWeb.Api.v1.Contracts
{
    public class ApiCollectionDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalHits { get; set; }
    }
}