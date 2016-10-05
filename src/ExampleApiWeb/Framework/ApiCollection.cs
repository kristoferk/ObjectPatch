using System.Collections.Generic;

namespace ExampleApiWeb.Framework
{
    public class ApiCollection<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalHits { get; set; }
    }
}