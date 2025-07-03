using AvaloniaApplication2.Models;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System;

namespace AvaloniaApplication2.ViewModels
{
    public class EllipseItem
    {
        public double X { get; set; }
        public double Y { get; set; }
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

            // Инициализация эллипсов
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                Ellipses.Add(new EllipseItem
                {
                    X = random.Next(0, 400), //  ширину  канваса
                    Y = random.Next(0, 400)  //  высоту  канваса
                });
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
    }
}
