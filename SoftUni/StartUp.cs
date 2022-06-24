using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            //Console.WriteLine(GetEmployeesFullInformation(softUniContext));
            Console.WriteLine(GetEmployeesWithSalaryOver50000(softUniContext));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }
            sb.ToString().TrimEnd();

            return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context
                            .Employees
                            .Where(x => x.Salary > 50000)
                            .OrderBy(x => x.FirstName)
                            .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            sb.ToString().TrimEnd();

            return sb.ToString();
        }
    }
}
