using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kartinki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Student> students = new ObservableCollection<Student>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(dialog.FileName));
                myImg.Source = bitmap;
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((BitmapSource)myImg.Source);

            int stride = wb.PixelWidth * 4;

            byte[] pixels = new byte[wb.PixelHeight * stride];

            wb.CopyPixels(pixels, stride, 0);

            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int index = y * stride + x * 4;
                    //красный
                    //pixels[index] = 0;     
                    //pixels[index + 1] = 0; 
                    //синий
                    //pixels[index + 1] = 0; 
                    //pixels[index + 2] = 0;
                    //зеленый
                    //pixels[index + 2] = 0;
                    //pixels[index] = 0;
                    //негатив
                    //pixels[index] = (byte)(255 - pixels[index]);         
                    //pixels[index + 1] = (byte)(255 - pixels[index + 1]); 
                    //pixels[index + 2] = (byte)(255 - pixels[index + 2]);

                    //задание 2
                    byte b = pixels[index];
                    byte g = pixels[index + 1];
                    byte r = pixels[index + 2];
                    if (r == 255)
                    {
                        pixels[index] = 255;     // B
                        pixels[index + 1] = 255; // G
                        pixels[index + 2] = 255; // R
                    }
                    else
                    {
                        pixels[index] = 0;       // B
                        pixels[index + 1] = 0;   // G
                        pixels[index + 2] = 0;   // R
                    }
                }
                wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
                myImg.Source = wb;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG|*.png|JPEG|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                BitmapEncoder encoder;

                if (dialog.FilterIndex == 1)
                    encoder = new PngBitmapEncoder();   
                else
                    encoder = new JpegBitmapEncoder();  
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)myImg.Source));
                using FileStream stream = new FileStream(dialog.FileName, FileMode.Create);
                encoder.Save(stream);
            }

        }
    }
}