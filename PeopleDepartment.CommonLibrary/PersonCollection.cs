using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDepartment.CommonLibrary
{
    public class PersonCollection : IEnumerable<Person>
    {
        public List<Person> Persons = new();
        

        public void Add(Person person)
        {
            Persons.Add(person);
        }

        public void Remove(Person person)
        {
            Persons.Remove(person);
        }

        public void LoadFromCsv(FileInfo csvFile)
        {
            using var reader = new StreamReader(csvFile.FullName);
            reader.ReadLine(); // skip first line

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line!.Split(';');

                // FirstName;LastName;DisplayName;Position;Email;Department
                var firstName = values[0];
                var lastName = values[1];
                var displayName = values[2];
                var position = values[3];
                var email = values[4];

                var department = values[5];

                var deps = department.Split(',');
                List<string> departments = deps.ToList();

                Person person = new(firstName, lastName, displayName, position, email, departments);
                Persons.Add(person);
            }
            reader.Close();
        }

        public DepartmentReport[] GetDepartmentReports()
        {
            // Get number of departments to generate reports of
            List<string> departments = new();
            foreach (var dep in Persons.SelectMany(person => person.Departments.Where(dep => !departments.Contains(dep))))
            {
                departments.Add(dep);
            }

            // Return value of this method
            var departmentReports = new DepartmentReport[departments.Count];

            // Order departments alphabetically
            departments.Sort();

            var indexDep = 0;
            foreach (var department in departments)
            {
                Person? head = null;
                Person? deputy = null;
                Person? secretary = null;
                List<Person> employees = new(); // Ine ako doktorand
                List<Person> phdStudents = new(); // Je doktorand

                foreach (var p in Persons)
                {
                    if (p.Departments.Contains(department) && !p.Position!.Equals("doktorand"))
                    {
                        // Add employees to dept.
                        employees.Add(p);

                        // Get head of dept.
                        if (p.Position.Equals("vedúci"))
                        {
                            head = p;
                        }
                        // Get deputy of dept.
                        if (p.Position.Equals("zástupca vedúceho"))
                        {
                            deputy = p;
                        }
                        if (p.Position.Equals("sekretárka"))
                        {
                            secretary = p;
                        }
                    }
                    if (p.Departments.Contains(department) && p.Position!.Equals("doktorand"))
                    {
                        // Add PhD students to dept.
                        phdStudents.Add(p);
                    }
                }

                // Get number of professors = contains title prof. before name
                // Get number of associate professors = contains doc. before name
                var countProf = 0;
                var countAssoc = 0;
                foreach (var employee in employees)
                {
                    if (employee.DisplayName.Contains("prof."))
                    {
                        countProf++;
                    }
                    if (employee.DisplayName.Contains("doc."))
                    {
                        countAssoc++;
                    }
                }
                var numberOfProfessors = countProf;
                var numberOfAssociateProfessors = countAssoc;

                // Sort Lists of employees and PhdStudents
                List<Person> sortedEmp = employees.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToList();
                List<Person> sortedStud = phdStudents.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToList();

                DepartmentReport report = new(department, head, deputy, secretary, numberOfProfessors, numberOfAssociateProfessors, sortedEmp, sortedStud);
                departmentReports[indexDep] = report;
                indexDep++;
            }

            return departmentReports;
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return Persons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
