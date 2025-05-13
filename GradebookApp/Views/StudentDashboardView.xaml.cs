using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GradebookApp.Models;

namespace GradebookApp.Views
{
    public partial class StudentDashboardView : Window
    {
        private Student currentStudent;
        private List<Grade> allGrades;

        public StudentDashboardView(Student student)
        {
            InitializeComponent();
            currentStudent = student;

            WelcomeTextBlock.Text = $"Witaj, {currentStudent.FullName}";
            LoadData();
            DisplaySubjectsWithGrades();
        }

        private void LoadData()
        {
            // Przykładowe dane – potem podmień na EF z bazy
            allGrades = new List<Grade>
            {
                new Grade { Subject = new Subject { Name = "Matematyka" }, Value = 5, Date = DateTime.Today },
                new Grade { Subject = new Subject { Name = "Matematyka" }, Value = 4, Date = DateTime.Today.AddDays(-2) },
                new Grade { Subject = new Subject { Name = "Fizyka" }, Value = 3, Date = DateTime.Today }
            };

            // Filtruj tylko oceny tego ucznia (gdy dane będą z bazy)
            allGrades = allGrades.Where(g => g.StudentId == currentStudent.Id).ToList();
        }

        private void DisplaySubjectsWithGrades()
        {
            var grouped = allGrades
                .GroupBy(g => g.Subject.Name)
                .ToList();

            foreach (var subjectGroup in grouped)
            {
                var subjectName = subjectGroup.Key;
                var grades = subjectGroup.ToList();
                double average = grades.Average(g => g.Value);

                var subjectBlock = new StackPanel
                {
                    Margin = new Thickness(0, 0, 0, 10)
                };

                subjectBlock.Children.Add(new TextBlock
                {
                    Text = $"{subjectName} – Średnia: {average:F2}",
                    FontSize = 18,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 0, 5)
                });

                var gradeList = new WrapPanel();
                foreach (var grade in grades)
                {
                    gradeList.Children.Add(new TextBlock
                    {
                        Text = grade.Value.ToString(),
                        Margin = new Thickness(5, 0, 5, 0),
                        FontSize = 16
                    });
                }

                subjectBlock.Children.Add(gradeList);
                SubjectsPanel.Children.Add(subjectBlock);
            }

            if (!grouped.Any())
            {
                SubjectsPanel.Children.Add(new TextBlock
                {
                    Text = "Brak ocen.",
                    FontSize = 16,
                    FontStyle = FontStyles.Italic
                });
            }
        }
    }
}
