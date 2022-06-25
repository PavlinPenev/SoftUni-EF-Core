using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();

            // Task 03
            Console.WriteLine(GetEmployeesFullInformation(softUniContext));
            
            // Task 04
            Console.WriteLine(GetEmployeesWithSalaryOver50000(softUniContext));
            
            // Task 05
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(softUniContext));
            
            //Task 06
            Console.WriteLine(AddNewAddressToEmployee(softUniContext));
            
            // Task 07
            Console.WriteLine(GetEmployeesInPeriod(softUniContext));
            
            //Task 08
            Console.WriteLine(GetAddressesByTown(softUniContext));
            
            // Task 09
            Console.WriteLine(GetEmployee147(softUniContext));
            
            // Task 10
            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(softUniContext));
            
            // Task 11
            Console.WriteLine(GetLatestProjects(softUniContext));
            
            // Task 12
            Console.WriteLine(IncreaseSalaries(softUniContext));
            
            // Task 13
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(softUniContext));
            
            // Task 14
            Console.WriteLine(DeleteProjectById(softUniContext));
            
            // Task 15
            Console.WriteLine(RemoveTown(softUniContext));
        }
        #region Task 03
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
        #endregion

        #region Task 04
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
        #endregion

        #region Task 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                        .Include(x => x.Department)
                        .Where(x => x.Department.Name == "Research and Development")
                        .OrderBy(x => x.Salary)
                        .ThenByDescending(x => x.FirstName)
                        .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:f2}");
            }
            sb.ToString().TrimEnd();

            return sb.ToString();
        }
        #endregion

        #region Task 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(address);
            context.SaveChanges();

            nakov.AddressId = address.AddressId;
            context.SaveChanges();

            var addresses = context.Employees
                                .OrderByDescending(x => x.AddressId)
                                .Take(10).
                                Select(x => x.Address.AddressText)
                                .ToList();

            return string.Join(Environment.NewLine, addresses);
        }
        #endregion

        #region Task 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                                    .Include(x => x.Manager)
                                    .Include(x => x.EmployeesProjects)
                                    .ThenInclude(x => x.Project)
                                    .Where(e => e.EmployeesProjects
                                                .Any(ep => ep.Project.StartDate.Year >= 2001 
                                                        && ep.Project.StartDate.Year <= 2003))
                                    .Take(10)
                                    .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine(
                    string.Format(
                        "{0} {1} - Manager: {2} {3}",
                        employee.FirstName,
                        employee.LastName,
                        employee.Manager.FirstName,
                        employee.Manager.LastName));

                foreach (var project in employee.EmployeesProjects)
                {
                    sb.AppendLine(
                        string.Format(
                            "--{0} - {1} - {2}",
                            project.Project.Name,
                            project.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            project.Project.EndDate.HasValue ? project.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"));
                }
            }
            return sb.ToString().TrimEnd();
        }
        #endregion

        #region Task 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                                .Include(x => x.Town)
                                .Include(x => x.Employees)
                                .OrderByDescending(x => x.Employees.Count())
                                .ThenBy(x => x.Town.Name)
                                .ThenBy(x => x.AddressText)
                                .Take(10)
                                .ToList();

            return String.Join(
                    Environment.NewLine, 
                    addresses.Select(x => 
                        $"{x.AddressText}, {x.Town.Name} - {x.Employees.Count()} employees")
                    );
        }
        #endregion

        #region Task 09
        public static string GetEmployee147(SoftUniContext context)
        {
            var employeeId = 147;

            var employee = context.Employees
                            .Include(x => x.EmployeesProjects)
                            .ThenInclude(x => x.Project)
                            .FirstOrDefault(x => x.EmployeeId == employeeId);

            var sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.EmployeesProjects.OrderBy(x => x.Project.Name))
            {
                sb.AppendLine(project.Project.Name);
            }

            return sb.ToString().TrimEnd();
        }
        #endregion

        #region Task 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                                .Include(x => x.Employees)
                                .ThenInclude(x => x.Manager)
                                .Where(x => x.Employees.Count() > 5)
                                .OrderBy(x => x.Employees.Count())
                                .ThenBy(x => x.Name)
                                .ToList();

            var sb = new StringBuilder();
            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.Manager.FirstName} {department.Manager.LastName}");

                foreach (var employee in department.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        #endregion

        #region Task 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var lastTenProjects = context.Projects
                                        .OrderByDescending(x => x.StartDate)
                                        .Take(10)
                                        .OrderBy(x => x.Name)
                                        .ToList();

            var sb = new StringBuilder();
            foreach (var project in lastTenProjects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }
        #endregion

        #region Task 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departmentsArray = new List<string>()
            { 
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employeesToPromote = context.Employees
                                        .Include(x => x.Department)
                                        .Where(x => departmentsArray.Contains(x.Department.Name))
                                        .OrderBy(x => x.FirstName)
                                        .ThenBy(x => x.LastName)
                                        .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employeesToPromote)
            {
                employee.Salary *= 1.12M;

                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        #endregion

        #region Task 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees.ToList();

            return string
                .Join(
                        Environment.NewLine,
                        employees
                        .Where(x => x.FirstName.StartsWith("sa", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(x => x.FirstName)
                        .ThenBy(x => x.LastName)
                        .Select(x => 
                            $"{x.FirstName} {x.LastName} - {x.JobTitle} - (${x.Salary:f2})")
                    );
        }
        #endregion

        #region Task 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectId = 2;
            var projectsToDelete = context.EmployeesProjects.Where(x => x.ProjectId == projectId);

            context.EmployeesProjects.RemoveRange(projectsToDelete);
            context.SaveChanges();

            var project = context.Projects.Find(projectId);
            context.Projects.Remove(project);
            context.SaveChanges();

            return String.Join(
                Environment.NewLine,
                context.Projects
                    .Take(10)
                    .Select(x => x.Name));
        }
        #endregion

        #region Task 15
        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns.FirstOrDefault(x => x.Name == "Seattle");

            var addresses = context.Addresses
                                    .Where(x => x.TownId == town.TownId);

            var employees = context.Employees
                                    .Where(x => addresses.Any(y => y.AddressId == x.AddressId))
                                    .ToList();
            employees.ForEach(x => x.AddressId = null);

            context.SaveChanges();

            var deletedTownsCount = addresses.Count();
            context.Addresses.RemoveRange(addresses);
            context.SaveChanges();

            context.Towns.Remove(town);
            context.SaveChanges();

            return $"{deletedTownsCount} addresses in Seattle were deleted";
        }
        #endregion
    }
}