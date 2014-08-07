using Hans.Components.General;
using System.Windows;

namespace Hans
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly HansAudioPlayer _hansAudioPlayer;

        /// <summary>
        /// Creates a new instance of the settings window
        /// </summary>
        /// <param name="hansAudioPlayer"></param>
        public SettingsWindow(HansAudioPlayer hansAudioPlayer)
        {
            _hansAudioPlayer = hansAudioPlayer;
            InitializeComponent();
        }

        /// <summary>
        /// Gets called when the save close button gets clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveClose_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.SongDownloads.ParalelDownloads = (int)SliderParalelDownloads.Value;
            Hide();
        }

        /// <summary>
        /// Gets called when the paralel downloads slider value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderParalelDownloads_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelSliderParalelDownloadsValue != null)
            {
                LabelSliderParalelDownloadsValue.Content = ((int)e.NewValue).ToString();
            }
        }

        /// <summary>
        /// Gets called when the window is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}