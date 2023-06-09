using Microsoft.Win32;
using PeopleDepartment.CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeopleDepartment.ViewerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DepartmentReport[] Reports;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PhdList.Items.Clear();
            EmployeeList.Items.Clear();

            var rep = Reports[DepartmentsBox.SelectedIndex];
            var students = rep.PhDStudents;
            var employees = rep.Employees;

            Head.Content = rep.Head.DisplayName;
            Deputy.Content = rep.Deputy.DisplayName;
            Secretary.Content = rep.Secretary.DisplayName;

            foreach (var student in students)
            {
                PhdList.Items.Add(student);
            }
            PhDCount.Content = students.Count().ToString();

            foreach (var employee in employees)
            {
                EmployeeList.Items.Add(employee);
            }
            EmployeeCount.Content = employees.Count().ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "C:\\Users\\kucera8\\Documents\\FriProS\\82eab0da";
            fileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            fileDialog.ShowDialog();

            if (!fileDialog.FileName.Any()) return;
            var filePath = fileDialog.FileName;
            FileInfo fi = new(filePath);

            // Get reports from csv file
            PersonCollection personCollection = new PersonCollection();
            personCollection.LoadFromCsv(fi);

            Reports = personCollection.GetDepartmentReports();

            // Fill combobox with dep names
            foreach (var report in Reports)
            {
                DepartmentsBox.Items.Add(report.Department);
            }

            DepartmentsBox.SelectedIndex = 0; // Select first department

            var rep = Reports[DepartmentsBox.SelectedIndex];
            var students = rep.PhDStudents;
            var employees = rep.Employees;

            Head.Content = rep.Head.DisplayName;
            Deputy.Content = rep.Deputy.DisplayName;
            Secretary.Content = rep.Secretary.DisplayName;

            foreach (var student in students)
            {
                PhdList.Items.Add(student); 
            }
            PhDCount.Content = students.Count().ToString();

            foreach (var employee in employees)
            {
                EmployeeList.Items.Add(employee);
            }
            EmployeeCount.Content = employees.Count().ToString();
        }
    }
}
