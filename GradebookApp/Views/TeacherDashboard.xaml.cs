using System.Windows;

namespace GradebookApp.Views
{
    public partial class TeacherDashboard : Window
    {
        public TeacherDashboard()
        {
            InitializeComponent();
        }

        private void SubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Przedmioty");
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Oceny");
        }

        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uczniowie");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
