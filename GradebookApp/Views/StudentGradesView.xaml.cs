using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GradebookApp.Models;

namespace GradebookApp.Views
{
    public partial class StudentGradesView : Window
    {
        public StudentGradesView(Student student, List<Grade> grades)
        {
            InitializeComponent();

            // Powitanie / nagłówek
            StudentNameTextBlock.Text = $"Oceny ucznia: {student.FullName}";

            // Przypisanie danych do tabeli
            GradesDataGrid.ItemsSource = grades;

            // Obliczenie średniej
            if (grades != null && grades.Any())
            {
                double avg = grades.Average(g => g.Value);
                AverageTextBlock.Text = $"Średnia ocen: {avg:F2}";
            }
            else
            {
                AverageTextBlock.Text = "Brak ocen.";
            }
        }
    }
}
