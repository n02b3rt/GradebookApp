using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GradebookApp.Data;
using GradebookApp.Models;
using GradebookApp.Services;
using GradebookApp.Views;
using Microsoft.EntityFrameworkCore;

namespace GradebookApp.ViewModels
{
    public class ConnectionViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _host;
        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                OnPropertyChanged();
                ValidateProperty(nameof(Host), value);
            }
        }

        private string _port;
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
                ValidateProperty(nameof(Port), value);
            }
        }

        private string _databaseName;
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                OnPropertyChanged();
                ValidateProperty(nameof(DatabaseName), value);
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ValidateProperty(nameof(Username), value);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidateProperty(nameof(Password), value);
            }
        }

        public ICommand ConnectCommand { get; }

        public ConnectionViewModel()
        {
            ConnectCommand = new RelayCommand(Connect, CanConnect);
        }

        private void Connect(object obj)
        {
            var connectionString = $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password}";

            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseNpgsql(connectionString);

                using var context = new AppDbContext(optionsBuilder.Options);
                if (context.Database.CanConnect())
                {
                    var config = new DatabaseConfig
                    {
                        Host = Host,
                        Port = Port,
                        DatabaseName = DatabaseName,
                        Username = Username,
                        Password = Password
                    };

                    AppSettingsService.SaveDatabaseConfig(config);

                    var mainWindow = new MainWindow();
                    mainWindow.Show();

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is ConnectionWindow)?
                        .Close();
                }
                else
                {
                    MessageBox.Show("Nie udało się połączyć z bazą danych.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia: {ex.Message}");
            }
        }

        private bool CanConnect(object obj)
        {
            return !HasErrors;
        }

        private readonly Dictionary<string, List<string>> _errors = new();
        public bool HasErrors => _errors.Any();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        private void ValidateProperty(string propertyName, string value)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(value))
                errors.Add("Pole jest wymagane");

            if (propertyName == nameof(Port) && !int.TryParse(value, out _))
                errors.Add("Port musi być liczbą");

            if (propertyName == nameof(Password) && string.IsNullOrWhiteSpace(value))
                errors.Add("Hasło jest wymagane");

            if (errors.Any())
            {
                _errors[propertyName] = errors;
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
