using AvaloniaApplication2.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaApplication2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}