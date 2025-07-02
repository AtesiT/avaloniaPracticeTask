using Avalonia.Controls;
using Avalonia.Interactivity;
using Tmds.DBus.Protocol;
using System.Linq;

namespace avaloniaPracticeTask.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            animals.ItemsSource = new string[]
                {"cat", "camel", "cow", "chameleon", "mouse", "lion"}
            .OrderBy(x => x);
        }
        private void Button_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Ваш код обработки клика
            message.Text = "Button clicked!";
        }
    }
}