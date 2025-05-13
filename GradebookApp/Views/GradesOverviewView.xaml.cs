using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GradebookApp.Models;

namespace GradebookApp.Views
{
    public partial class GradesOverviewView : Window
    {
        private List<Class> allClasses;
        private List<Subject> allSubjects;
        private List<Grade> allGrades;
        private List<Student> allStudents;

        public GradesOverviewView()
        {
            InitializeComponent();
            LoadData();
            RefreshDataGrid();
        }

        private void LoadData()
        {
            // Tymczasowe dane testowe — zastąp później EF
            allClasses = new List<Class>
            {
                new Class { ClassId = 1, Name = "1A" },
                new Class { ClassId = 2, Name = "2B" }
            };

            allSubjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Matematyka" },
                new Subject { Id = 2, Name = "Fizyka" }
            };

            allStudents = new List<Student>
            {
                new Student { Id = 1, FirstName = "Jan", LastName = "Kowalski", ClassId = 1, Class = allClasses[0] },
                new Student { Id = 2, FirstName = "Anna", LastName = "Nowak", ClassId = 1, Class = allClasses[0] },
                new Student { Id = 3, FirstName = "Tomek", LastName = "Zieliński", ClassId = 2, Class = allClasses[1] }
            };

            allGrades = new List<Grade>
            {
                new Grade { StudentId = 1, SubjectId = 1, Value = 5, Date = DateTime.Today, Student = allStudents[0], Subject = allSubjects[0] },
                new Grade { StudentId = 1, SubjectId = 2, Value = 4, Date = DateTime.Today, Student = allStudents[0], Subject = allSubjects[1] },
                new Grade { StudentId = 2, SubjectId = 1, Value = 3, Date = DateTime.Today, Student = allStudents[1], Subject = allSubjects[0] },
            };

            ClassComboBox.ItemsSource = allClasses;
            SubjectComboBox.ItemsSource = allSubjects;
        }

        private void RefreshDataGrid()
        {
            var selectedClass = ClassComboBox.SelectedItem as Class;
            var selectedSubject = SubjectComboBox.SelectedItem as Subject;

            var filtered = allGrades.Where(g =>
                (selectedClass == null || g.Student.ClassId == selectedClass.ClassId) &&
                (selectedSubject == null || g.SubjectId == selectedSubject.Id))
                .Select(g => new
                {
                    StudentName = g.Student.FullName,
                    g.Value,
                    SubjectName = g.Subject.Name,
                    Date = g.Date.ToString("yyyy-MM-dd")
                }).ToList();

            GradesDataGrid.ItemsSource = filtered;

            if (filtered.Count > 0)
            {
                var average = filtered.Average(g => g.Value);
                AverageTextBlock.Text = $"Średnia ocen: {average:F2}";
            }
            else
            {
                AverageTextBlock.Text = "Brak ocen.";
            }
        }

        private void Filters_Changed(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
    }
}
