namespace IndustryConnect_Week5_WebApi.Dtos
{
    //Task 2: Extend the customer controller to use a dto and not the entity
    public class CustomerDto
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int Age
        {
            get
            {
                return DateTime.Now.Year - DateOfBirth.Value.Year;
            }
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
