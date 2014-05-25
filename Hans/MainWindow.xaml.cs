using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWavePlayer _waveOut;
        private AudioFileReader _audioFileReader;

        public MainWindow()
        {
            InitializeComponent();
            _waveOut = new WaveOut();
            _audioFileReader = new AudioFileReader("Another One Bites the Dust - Queen.mp3");
            _waveOut.Init(_audioFileReader);
            _waveOut.Play();
        }
    }
}
