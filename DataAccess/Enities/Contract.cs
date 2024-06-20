#nullable disable
namespace DataAccess.Enities
{
    public class Contract
    {
        public int Id { get; set; }
        public string ContractType { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public int EmployeeEmployersId { get; set; }
    }
}
