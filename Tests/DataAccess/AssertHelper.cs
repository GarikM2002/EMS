using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Enities;

namespace Tests.DataAccess
{
    public static class AssertHelper
    {
        public static void AssertContractsAreEqual(Contract expected, Contract actual)
        {
            Assert.Equal(expected.EmployeeEmployersId, actual.EmployeeEmployersId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Salary, actual.Salary);
            Assert.Equal(expected.EndDate.HasValue ? expected.EndDate.Value.Date : expected.EndDate,
                        actual.EndDate.HasValue ? actual.EndDate.Value.Date : actual.EndDate);
            Assert.Equal(expected.StartDate.Date, actual.StartDate.Date);
            Assert.Equal(expected.ContractTypeId, actual.ContractTypeId);
        }

        public static void AssertEmployeesAreEqual(Employee expected, Employee actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
            Assert.Equal(expected.Department, actual.Department);
        }

        public static void AssertEmployersAreEqual(Employer expected, Employer actual, bool checkIsDeleted = true)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Department, actual.Department);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
            Assert.Equal(expected.Email, actual.Email);
            if (checkIsDeleted)
                Assert.Equal(expected.IsDeleted, actual.IsDeleted);
            Assert.Equal(expected.PasswordHash, actual.PasswordHash);
            Assert.Equal(expected.PasswordSalt, actual.PasswordSalt);
        }
    }
}
