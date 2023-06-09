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
                string firstName = values[0];
                string lastName = values[1];
                string displayName = values[2];
                string position = values[3];
                string email = values[4];

                string department = values[5];
                List<string> departments = new();

                var deps = department.Split(',');
                foreach (var dep in deps)
                {
                    departments.Add(dep);
                }

                Person person = new(firstName, lastName, displayName, position, email, departments);
                Persons.Add(person);
            }
            reader.Close();
        }

        public DepartmentReport[] GetDepartmentReports()
        {
            // Get number of departments to generate reports of
            List<string> departments = new();
            foreach (var person in Persons)
            {
                foreach (var dep in person.Departments)
                {
                    if (!departments.Contains(dep)) // remove duplicities of departments
                    {
                        departments.Add(dep);
                    }
                }
            }
            var depCount = departments.Count();

            // Return value of this method
            DepartmentReport[] departmentReports = new DepartmentReport[depCount];

            // Order departments alphabetically
            departments.Sort();

            int indexDep = 0;
            foreach (var department in departments)
            {
                string depName = department;
                Person? head = null;
                Person? deputy = null;
                Person? secretary = null;
                int numberOfProfessors;
                int numberOfAssociateProfessors;
                List<Person> employees = new(); // Ine ako doktorand
                List<Person> phdStudents = new(); // Je doktorand

                foreach (var p in Persons)
                {
                    if (p.Departments.Contains(depName) && !p.Position.Equals("doktorand"))
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
                    if (p.Departments.Contains(depName) && p.Position.Equals("doktorand"))
                    {
                        // Add PhD students to dept.
                        phdStudents.Add(p);
                    }
                }

                // Get number of professors = contains title prof. before name
                // Get number of associate professors = contains doc. before name
                int countProf = 0;
                int countAssoc = 0;
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
                numberOfProfessors = countProf;
                numberOfAssociateProfessors = countAssoc;

                // Sort Lists of employees and PhdStudents
                List<Person> sortedEmp = employees.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToList();
                List<Person> sortedStud = phdStudents.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToList();

                DepartmentReport report = new(depName, head, deputy, secretary, numberOfProfessors, numberOfAssociateProfessors, sortedEmp, sortedStud);
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
