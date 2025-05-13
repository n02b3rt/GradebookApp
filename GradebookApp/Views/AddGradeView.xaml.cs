using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GradebookApp.Models;

namespace GradebookApp.Views
{
    public partial class AddGradeView : Window
    {
        private List<Class> allClasses;
        private List<Student> allStudents;
        private List<Subject> allSubjects;
        private List<Grade> existingGrades;

        public AddGradeView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Przykładowe dane – do testów, później zamień na dane z EF

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

            allSubjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Matematyka" },
                new Subject { Id = 2, Name = "Fizyka" }
            };

            existingGrades = new List<Grade>(); // ← załadujesz z bazy w przyszłości

            ClassComboBox.ItemsSource = allClasses;
            SubjectComboBox.ItemsSource = allSubjects;
            DatePicker.SelectedDate = DateTime.Now;
        }

        private void ClassComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ClassComboBox.SelectedItem is Class selectedClass)
            {
                var filtered = allStudents
                    .Where(s => s.ClassId == selectedClass.ClassId)
                    .ToList();

                StudentComboBox.ItemsSource = filtered;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var student = StudentComboBox.SelectedItem as Student;
            var subject = SubjectComboBox.SelectedItem as Subject;
            var gradeItem = GradeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            var date = DatePicker.SelectedDate;

            if (student == null || subject == null || gradeItem == null || date == null)
            {
                MessageBox.Show("Uzupełnij wszystkie pola.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int gradeValue = int.Parse(gradeItem.Content.ToString());

            // 🔍 Walidacja: sprawdź, czy już istnieje taka ocena
            bool alreadyExists = existingGrades.Any(g =>
                g.StudentId == student.Id &&
                g.SubjectId == subject.Id &&
                g.Date.Date == date.Value.Date);

            if (alreadyExists)
            {
                MessageBox.Show("Uczeń ma już ocenę z tego przedmiotu w wybranym dniu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ✅ Zapisz ocenę (tu dodasz EF.SaveChanges() w przyszłości)
            MessageBox.Show($"Zapisano ocenę {gradeValue} dla {student.FullName} z {subject.Name} ({date:yyyy-MM-dd})");

            this.Close();
        }
    }
}
