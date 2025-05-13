using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GradebookApp.Models;

namespace GradebookApp.Views
{
    public partial class StudentListView : Window
    {
        private List<Class> allClasses;
        private List<Student> allStudents;

        public StudentListView()
        {
            InitializeComponent();
            LoadData();
            RefreshStudentList();
        }

        private void LoadData()
        {
            // Dane przykładowe — podmień na bazę później
            allClasses = new List<Class>
            {
                new Class { ClassId = 1, Name = "1A" },
                new Class { ClassId = 2, Name = "2B" }
            };

            allStudents = new List<Student>
            {
                new Student { Id = 1, FirstName = "Jan", LastName = "Kowalski", Class = allClasses[0], ClassId = 1 },
                new Student { Id = 2, FirstName = "Anna", LastName = "Nowak", Class = allClasses[0], ClassId = 1 },
                new Student { Id = 3, FirstName = "Tomek", LastName = "Zieliński", Class = allClasses[1], ClassId = 2 }
            };

            ClassComboBox.ItemsSource = allClasses;
        }

        private void ClassComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RefreshStudentList();
        }

        private void RefreshStudentList()
        {
            var selectedClass = ClassComboBox.SelectedItem as Class;

            var filtered = selectedClass == null
                ? allStudents
                : allStudents.Where(s => s.ClassId == selectedClass.ClassId).ToList();

            StudentsDataGrid.ItemsSource = filtered;
        }

        private void ShowGradesButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is Student selectedStudent)
            {
                var gradesWindow = new StudentGradesView(selectedStudent);
                gradesWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz ucznia z listy.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
