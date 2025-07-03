using avaloniaPracticeTask.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;

namespace avaloniaPracticeTask.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Canvas_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            var pos = e.GetCurrentPoint((Control)sender).Position;
            if (DataContext is not MainWindowViewModel viewModel) return;
            viewModel.CreateElipse(pos);

        }
    }
}