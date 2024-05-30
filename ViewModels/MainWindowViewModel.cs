using Avalonia.Media.Imaging;
using ReactiveUI;
using System.IO;

namespace UnitConverter.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public Bitmap _logo;

        public string Heading1 => "Currency";
        public string Heading2 => "Converter";
        public string CombinedHeading => $"{Heading1}\n{Heading2}";
        
        public Bitmap Logo
        {
            get => _logo;
            set => this.RaiseAndSetIfChanged(ref _logo, value);
        }

        public MainWindowViewModel()
        {
            string logoPath = "Assets/images/logo.png";
            if (File.Exists(logoPath))
            {
                Logo = new Bitmap(logoPath);
            }
        }
    }
}