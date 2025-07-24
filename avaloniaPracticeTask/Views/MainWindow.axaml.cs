using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace avaloniaPracticeTask.Views
{
    public partial class MainWindow : Window
    {
        private enum DrawingMode { Line, FreeDraw, Eraser }
        private DrawingMode currentMode = DrawingMode.Line;
        private IBrush currentBrush = Brushes.Black;

        private double brushThickness = 2;
        private double eraserThickness = 20;

        // Для режима линий
        private Point? lineStartPoint;
        private Line? currentLine;

        // Для свободного рисования и ластика
        private Path? currentPath;
        private PathFigure? currentFigure;
        private bool isDrawing = false;

        // Стеки для undo/redo
        private Stack<Control> undoStack = new Stack<Control>();
        private Stack<Control> redoStack = new Stack<Control>();

        public MainWindow()
        {
            InitializeComponent();

            LineModeButton.Click += (s, e) => currentMode = DrawingMode.Line;
            FreeDrawModeButton.Click += (s, e) => currentMode = DrawingMode.FreeDraw;
            EraserModeButton.Click += (s, e) => currentMode = DrawingMode.Eraser;
            ClearButton.Click += ClearButton_Click;
            ColorComboBox.SelectionChanged += ColorComboBox_SelectionChanged;

            // Обработчики undo/redo
            UndoButton.Click += UndoButton_Click;
            RedoButton.Click += RedoButton_Click;

            // Инициализация слайдеров
            BrushThicknessSlider.ValueChanged += (s, e) => brushThickness = BrushThicknessSlider.Value;
            EraserThicknessSlider.ValueChanged += (s, e) => eraserThickness = EraserThicknessSlider.Value;

            DrawingCanvas.PointerPressed += DrawingCanvas_PointerPressed;
            DrawingCanvas.PointerReleased += DrawingCanvas_PointerReleased;
            DrawingCanvas.PointerMoved += DrawingCanvas_PointerMoved;
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)ColorComboBox.SelectedItem;
            var colorName = selectedItem.Content.ToString();

            currentBrush = colorName switch
            {
                "Red" => Brushes.Red,
                "Green" => Brushes.Green,
                "Blue" => Brushes.Blue,
                "Yellow" => Brushes.Yellow,
                "Orange" => Brushes.Orange,
                "Purple" => Brushes.Purple,
                "Pink" => Brushes.Pink,
                _ => Brushes.Black // Default
            };
        }

        private void DrawingCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(DrawingCanvas);

            if (currentMode == DrawingMode.Line)
            {
                lineStartPoint = point;
                currentLine = new Line
                {
                    StartPoint = point,
                    EndPoint = point,
                    Stroke = currentBrush,
                    StrokeThickness = brushThickness, // Используем регулируемую толщину кисти
                    StrokeLineCap = PenLineCap.Round
                };
                DrawingCanvas.Children.Add(currentLine);
            }
            else if ((currentMode == DrawingMode.FreeDraw || currentMode == DrawingMode.Eraser) && e.GetCurrentPoint(DrawingCanvas).Properties.IsLeftButtonPressed)
            {
                isDrawing = true;

                var brush = currentMode == DrawingMode.Eraser ? Brushes.White : currentBrush;
                var thickness = currentMode == DrawingMode.Eraser ? eraserThickness : brushThickness;

                currentPath = new Path
                {
                    Stroke = brush,
                    StrokeThickness = thickness,
                    StrokeLineCap = PenLineCap.Round,
                    StrokeJoin = PenLineJoin.Round,
                    Data = new PathGeometry()
                };

                currentFigure = new PathFigure
                {
                    StartPoint = point,
                    IsClosed = false
                };

                ((PathGeometry)currentPath.Data).Figures.Add(currentFigure);
                DrawingCanvas.Children.Add(currentPath);
            }
        }

        private void DrawingCanvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            var point = e.GetPosition(DrawingCanvas);

            if (currentMode == DrawingMode.Line && lineStartPoint != null && currentLine != null)
            {
                currentLine.EndPoint = point;
            }
            else if ((currentMode == DrawingMode.FreeDraw || currentMode == DrawingMode.Eraser) && isDrawing && currentFigure != null)
            {
                if (currentFigure.Segments.Count == 0 ||
                    ((LineSegment)currentFigure.Segments[^1]).Point != point)
                {
                    currentFigure.Segments.Add(new LineSegment { Point = point });
                    DrawingCanvas.InvalidateVisual();
                }
            }
        }

        private void DrawingCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (currentMode == DrawingMode.Line)
            {
                if (currentLine != null)
                {
                    undoStack.Push(currentLine);
                    redoStack.Clear(); // Новое действие очищает redo
                }
                lineStartPoint = null;
                currentLine = null;
            }
            else if (currentMode == DrawingMode.FreeDraw || currentMode == DrawingMode.Eraser)
            {
                if (currentPath != null)
                {
                    undoStack.Push(currentPath);
                    redoStack.Clear(); // Новое действие очищает redo
                }
                isDrawing = false;
                currentPath = null;
                currentFigure = null;
            }
        }

        private void UndoButton_Click(object? sender, RoutedEventArgs e)
        {
            if (undoStack.Count > 0)
            {
                var item = undoStack.Pop();
                DrawingCanvas.Children.Remove(item);
                redoStack.Push(item);
            }
        }

        private void RedoButton_Click(object? sender, RoutedEventArgs e)
        {
            if (redoStack.Count > 0)
            {
                var item = redoStack.Pop();
                DrawingCanvas.Children.Add(item);
                undoStack.Push(item);
            }
        }

        private void ClearButton_Click(object? sender, RoutedEventArgs e)
        {
            DrawingCanvas.Children.Clear();
            undoStack.Clear();
            redoStack.Clear();
            currentLine = null;
            currentPath = null;
            currentFigure = null;
            isDrawing = false;
            lineStartPoint = null;
        }
    }
}