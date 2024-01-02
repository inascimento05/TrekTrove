using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static explicit operator CustomerDto(Customer entity)
        {
            var valueDto = new CustomerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return valueDto;
        }
    }
}
