namespace ExampleApiWeb.Api.v1.Contracts
{
    public class CustomerFilterDto
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}