using PeopleDepartment.CommonLibrary;

namespace PeopleDepartment.ReportConsoleApp
{
    

    internal class Program
    {
        static void Main(string input, string template, string output = null)
        {
            //Console.WriteLine(template);
            //Console.WriteLine(input);
            //Console.WriteLine(output);

            // Arguments for 1st version: --input people-fri.csv --template templ-dep-sk.txt
            // Arguments for 2nd version: --input people-fri.csv --template templ-dep-en.txt --output Reporty
            
            FileInfo fi = new(input);

            if (!fi.Exists || !File.Exists(template))
            {
                Console.Error.WriteLine("File does not exist!");
            }

            PersonCollection collection = new();
            collection.LoadFromCsv(fi);
            var reports = collection.GetDepartmentReports();

            if (output != null)
            {
                var outputInfo = Directory.CreateDirectory(output); // Create new directory if not created

                foreach (var report in reports)
                {
                    var text = File.ReadAllText(template); // Read whole template

                    text = text.Replace("[[Department]]", report.Department);

                    if (report.Head is null)
                    {
                        text = text.Replace("[[Head]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Head]]", report.Head.DisplayName);
                    }

                    if (report.Deputy is null)
                    {
                        text = text.Replace("[[Deputy]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Deputy]]", report.Deputy.DisplayName);
                    }

                    if (report.Secretary is null)
                    {
                        text = text.Replace("[[Secretary]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Secretary]]", report.Secretary.DisplayName);
                    }

                    text = text.Replace("[[NumberOfEmployees]]", report.NumberOfEmployees.ToString());
                    text = text.Replace("[[NumberOfProfessors]]", report.NumberOfProfessors.ToString());
                    text = text.Replace("[[NumberOfAssociateProfessors]]", report.NumberOfAssociateProfessors.ToString());
                    text = text.Replace("[[NumberOfPhDStudents]]", report.NumberOfPhDStudents.ToString());

                    var textEmp = "";
                    foreach (var employee in report.Employees)
                    {
                        textEmp += employee.ToFormattedString();
                        textEmp += Environment.NewLine;
                    }

                    var textStud = "";
                    foreach (var student in report.PhDStudents)
                    {
                        textStud += student.ToFormattedString();
                        textStud += Environment.NewLine;
                    }

                    text = text.Replace("[[Employees]]", textEmp);
                    text = text.Replace("[[PhDStudents]]", textStud);

                    var path = outputInfo.FullName + "\\" + report.Department + ".txt";

                    var file = File.Create(path);
                    file.Close();

                    File.WriteAllText(path, text);

                    
                }


            }

            // Print to console
            if (output == null)
            {
                foreach (var report in reports)
                {
                    var text = File.ReadAllText(template); // Read whole template

                    text = text.Replace("[[Department]]", report.Department);

                    if (report.Head is null)
                    {
                        text = text.Replace("[[Head]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Head]]", report.Head.DisplayName);
                    }

                    if (report.Deputy is null)
                    {
                        text = text.Replace("[[Deputy]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Deputy]]", report.Deputy.DisplayName);
                    }

                    if (report.Secretary is null)
                    {
                        text = text.Replace("[[Secretary]]", "");
                    }
                    else
                    {
                        text = text.Replace("[[Secretary]]", report.Secretary.DisplayName);
                    }

                    text = text.Replace("[[NumberOfEmployees]]", report.NumberOfEmployees.ToString());
                    text = text.Replace("[[NumberOfProfessors]]", report.NumberOfProfessors.ToString());
                    text = text.Replace("[[NumberOfAssociateProfessors]]", report.NumberOfAssociateProfessors.ToString());
                    text = text.Replace("[[NumberOfPhDStudents]]", report.NumberOfPhDStudents.ToString());

                    var textEmp = "";
                    foreach (var employee in report.Employees)
                    {
                        textEmp += employee.ToFormattedString();
                        textEmp += Environment.NewLine;
                    }

                    var textStud = "";
                    foreach (var student in report.PhDStudents)
                    {
                        textStud += student.ToFormattedString();
                        textStud += Environment.NewLine;
                    }

                    text = text.Replace("[[Employees]]", textEmp);
                    text = text.Replace("[[PhDStudents]]", textStud);

                    Console.WriteLine(text);
                }
            }
        }
    }
}