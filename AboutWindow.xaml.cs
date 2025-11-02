using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BartsTOK
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.Loaded += AboutWindow_Loaded;
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try pack URI first (resource)
                try
                {
                    var res = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/loading.gif"));
                    if (res != null)
                    {
                        var decoder = new System.Windows.Media.Imaging.GifBitmapDecoder(res.Stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        if (decoder.Frames.Count > 0)
                        {
                            imgBart.Source = decoder.Frames[0];
                        }
                    }
                }
                catch { }

                // Fallback: load from app folder
                if (imgBart.Source == null)
                {
                    var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "loading.gif");
                    if (File.Exists(path))
                    {
                        using (var fs = File.OpenRead(path))
                        {
                            var decoder = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                            if (decoder.Frames.Count > 0)
                            {
                                imgBart.Source = decoder.Frames[0];
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch { }
            e.Handled = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
