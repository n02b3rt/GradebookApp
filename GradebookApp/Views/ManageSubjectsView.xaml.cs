using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using GradebookApp.Models; // zakładam, że masz model Subject

namespace GradebookApp.Views
{
    public partial class ManageSubjectsView : Window
    {
        public ObservableCollection<Subject> Subjects { get; set; }

        public ManageSubjectsView()
        {
            InitializeComponent();

            // Przykladowe dane – do podmiany na dane z bazy
            Subjects = new ObservableCollection<Subject>
            {
                new Subject { Id = 1, Name = "Matematyka", TeacherId = 101 },
                new Subject { Id = 2, Name = "Fizyka", TeacherId = 101 }
            };

            SubjectsDataGrid.ItemsSource = Subjects;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dodaj przedmiot");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectsDataGrid.SelectedItem is Subject selected)
                MessageBox.Show($"Edytuj: {selected.Name}");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectsDataGrid.SelectedItem is Subject selected)
            {
                if (MessageBox.Show($"Usunąć {selected.Name}?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Subjects.Remove(selected);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

