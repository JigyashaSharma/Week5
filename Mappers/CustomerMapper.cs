using IndustryConnect_Week5_WebApi.Dtos;
using IndustryConnect_Week5_WebApi.Models;

namespace IndustryConnect_Week5_WebApi.Mappers
{
    //Task 2: Extend the customer controller to use a dto and not the entity
    public static class CustomerMapper
    {
        public static CustomerDto EntityToCustomerDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth
            };
        }

        public static Customer CustomerDtoToEntity(CustomerDto customerDto)
        {
            return new Customer
            {
                Id = customerDto.Id,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                DateOfBirth = customerDto.DateOfBirth
            };
        }
    }
}
