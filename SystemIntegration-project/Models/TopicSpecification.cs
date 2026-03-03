using Models;

namespace SystemIntegration_project.Models;

public class TopicSpecifications
{
    public List<TopicSpecification> TopicSpecificationsList { get; set; } = new List<TopicSpecification>();
    
    
    public class TopicSpecification
    {
        public string TopicName { get; private set; }
        public string ExchangeName { get; private set; }
        public Predicate<FlightInfo> Filter { get; private set; }

        public TopicSpecification(string topicName, string exchangeName, Predicate<FlightInfo> filter)
        {
            TopicName = topicName;
            ExchangeName = exchangeName;
            Filter = filter;
        }  
    }
    
}