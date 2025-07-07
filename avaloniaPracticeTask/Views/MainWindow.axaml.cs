using avaloniaPracticeTask.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Linq;
using System.Diagnostics;

namespace avaloniaPracticeTask.Views
{
    public partial class MainWindow : Window
    {
        private EllipseItem? _selectedEllipse;
        private bool _isDragging;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel viewModel || sender is not Control control) return;

            if (e.GetCurrentPoint(control).Properties.IsLeftButtonPressed)
            {
                var pos = e.GetCurrentPoint(control).Position;
                Debug.WriteLine($"������� ����: X={pos.X}, Y={pos.Y}");

                var hitEllipse = viewModel.Ellipses.FirstOrDefault(ellipse =>
                    IsPointInEllipse(pos, ellipse, 20, 20));

                if (hitEllipse != null)
                {
                    Debug.WriteLine($"������ ������: X={hitEllipse.X}, Y={hitEllipse.Y}");
                    _selectedEllipse = hitEllipse;
                    _isDragging = true;
                }
                else
                {
                    Debug.WriteLine("������ �� ������, �������� ������");
                    viewModel.CreateEllipse(pos); // ����������: CreateElipse -> CreateEllipse
                }
            }
        }

        private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isDragging && _selectedEllipse != null && DataContext is MainWindowViewModel viewModel && sender is Control control)
            {
                var pos = e.GetCurrentPoint(control).Position;
                if (e.GetCurrentPoint(control).Properties.IsLeftButtonPressed)
                {
                    viewModel.UpdateEllipsePosition(_selectedEllipse, pos); // ����������: UpdateElipsePosition -> UpdateEllipsePosition
                    Debug.WriteLine($"�������������� �������: X={pos.X}, Y={pos.Y}");
                }
                else
                {
                    _isDragging = false;
                    _selectedEllipse = null;
                    Debug.WriteLine("�������������� �����������: ����� ������ ��������");
                }
            }
        }

        private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _selectedEllipse = null;
                Debug.WriteLine("�������������� �����������: ���� ��������");
            }
        }

        private bool IsPointInEllipse(Avalonia.Point point, EllipseItem ellipse, double width, double height)
        {
            // ����� ������� (TranslateTransform ������������� ������� ����� ����)
            double centerX = ellipse.X + width / 2;
            double centerY = ellipse.Y + height / 2;

            // �������� ��������� ����� � ������ �� ��������� �������
            double dx = point.X - centerX;
            double dy = point.Y - centerY;
            bool isInEllipse = (dx * dx) / ((width / 2) * (width / 2)) + (dy * dy) / ((height / 2) * (height / 2)) <= 1;

            Debug.WriteLine($"�������� �������: X={ellipse.X}, Y={ellipse.Y}, CenterX={centerX}, CenterY={centerY}, ClickX={point.X}, ClickY={point.Y}, IsInEllipse={isInEllipse}");
            return isInEllipse;
        }
    }
}