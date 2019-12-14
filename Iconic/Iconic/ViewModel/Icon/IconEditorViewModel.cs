using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Iconic.ViewModel.Icon
{
    public class IconEditorViewModel : WindowViewModel
    {
        public IconEditorViewModel(Window window, Models.Icon.Icon icon = null) : base(window)
        {
            Title = "";
            mWindow = window;
            WindowMinimumHeight = 600;
            WindowMinimumWidth = 350;

            Icon = icon;

            SizeChangedCommand = new RelayCommand(p => ApplyCanvas());
            SaveIconCommand = new RelayCommand(p => SaveIcon());
            ApplyCanvas();
        }

        #region Commands

        public ICommand SizeChangedCommand { get; set; }
        public ICommand SaveIconCommand { get; set; }

        #endregion

        #region Properties

        public Models.Icon.Icon Icon { get; set; }
        public Path Path { get; set; } = new Path();
        public Canvas Canvas { get; set; }
        public double CurrentSize { get; set; } = 24;
        public double MaximumSize { get; set; } = 250;
        public Color SelectedColor { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Apply size or color changes
        /// </summary>
        public void ApplyCanvas()
        {
            Canvas = new Canvas();

            var path = new Path();

            path.Data = Geometry.Parse(Icon.Data);
            path.Height = CurrentSize;
            path.Width = CurrentSize;
            path.Stretch = Stretch.Uniform;

            if (SelectedColor.A == 0 && SelectedColor.R == 0 && SelectedColor.G == 0 && SelectedColor.B == 0)
            {
                path.Fill = new SolidColorBrush(Color.FromRgb(0,0,0));
            }
            else
            {
                path.Fill = new SolidColorBrush(SelectedColor);
            }

            Canvas.Children.Add(path);
        }

        /// <summary>
        /// Save canvas as png file
        /// source: https://stackoverflow.com/a/21413246/6940144
        /// </summary>
        public void SaveIcon()
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select Folder",
                RootFolder = Environment.SpecialFolder.Desktop
            };

            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(Canvas);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, PixelFormats.Default);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(Canvas);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }

                rtb.Render(dv);

                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    BitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                    pngEncoder.Save(ms);

                    ms.Close();

                    System.IO.File.WriteAllBytes(folderBrowserDialog.SelectedPath + $"\\{Icon.Name}.png", ms.ToArray());
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}