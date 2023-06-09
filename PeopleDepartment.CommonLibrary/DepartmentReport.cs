using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDepartment.CommonLibrary
{
    public class DepartmentReport
    {
        public string Department { get; }
        public Person? Head { get; }
        public Person? Deputy { get; }
        public Person? Secretary { get; }
        public int NumberOfProfessors { get; }
        public int NumberOfAssociateProfessors { get; }
        public int NumberOfEmployees { get; }
        public int NumberOfPhDStudents { get; }
        public IEnumerable<Person> Employees { get; }
        public IEnumerable<Person> PhDStudents { get; }

        public DepartmentReport(string department, Person? head, Person? deputy, Person? secretary, int numberOfProfessors, int numberOfAssociateProfessors, IEnumerable<Person> employees, IEnumerable<Person> phdStudents)
        {
            Department = department;
            Head = head;
            Deputy = deputy;
            Secretary = secretary;
            NumberOfProfessors = numberOfProfessors;
            NumberOfAssociateProfessors = numberOfAssociateProfessors;
            Employees = employees;
            NumberOfEmployees = Employees.Count();
            PhDStudents = phdStudents;
            NumberOfPhDStudents = PhDStudents.Count();
        }
    }
}
