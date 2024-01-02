using System.Text.Json.Serialization;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos
{
    public class UpdateCustomerDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
