namespace Finops.Models
{
    public class MetricAverage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Location { get; set; }
        public DateTime TimeCreated { get; set; }

        public Dictionary<string, double> Average { get; set; }
    }
}
