using Hans.General;
using Hans.Library;
using Hans.Properties;
using System.Windows;
using System.Windows.Controls;

namespace Hans
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly HansAudioPlayer _hansAudioPlayer;

        public SettingsWindow(HansAudioPlayer hansAudioPlayer)
        {
            _hansAudioPlayer = hansAudioPlayer;
            InitializeComponent();
        }

        private void ButtonSaveClose_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.SongDownloads.ParalelDownloads = (int)SliderParalelDownloads.Value;
            Hide();
        }

        private void SliderParalelDownloads_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelSliderParalelDownloadsValue != null)
            {
                LabelSliderParalelDownloadsValue.Content = ((int)e.NewValue).ToString();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}