using avaloniaPracticeTask.Models;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System;
using System.ComponentModel;
using Avalonia;

namespace avaloniaPracticeTask.ViewModels
{
    public class EllipseItem : INotifyPropertyChanged
    {
        private double _x;
        private double _y;

        public double X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainWindowViewModel : ReactiveObject
    {
        private string _newAnimalName = "";
        public string NewAnimalName
        {
            get => _newAnimalName;
            set => this.RaiseAndSetIfChanged(ref _newAnimalName, value);
        }

        public ObservableCollection<Animal> Animals { get; } = new();
        public ObservableCollection<EllipseItem> Ellipses { get; } = new();

        public MainWindowViewModel()
        {
            // Инициализация начальными значениями
            var initialAnimals = new string[] { }
                .OrderBy(x => x)
                .Select(name => new Animal(name, $"Description of {name}"));

            foreach (var animal in initialAnimals)
            {
                Animals.Add(animal);
            }
        }

        public void AddAnimal()
        {
            if (!string.IsNullOrWhiteSpace(NewAnimalName))
            {
                Animals.Add(new Animal(NewAnimalName, $"Description of {NewAnimalName}"));
                NewAnimalName = "";
            }
        }

        public void CreateEllipse(Avalonia.Point point)
        {
            Ellipses.Add(new EllipseItem { X = point.X, Y = point.Y });
        }

        public void UpdateEllipsePosition(EllipseItem ellipse, Avalonia.Point point)
        {
            ellipse.X = point.X;
            ellipse.Y = point.Y;
        }
    }
}