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
using System.Windows.Shapes;

namespace pax4props
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            sldrSoloSndVol.Value = Properties.Settings.Default.SoloVolume ;
            sldrGroupSndVol.Value = Properties.Settings.Default.GroupVolume ;
            cbCheckForUpdates.IsChecked = Properties.Settings.Default.CheckForUpdates;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.SoloVolume = (int)sldrSoloSndVol.Value;
            Properties.Settings.Default.GroupVolume = (int)sldrGroupSndVol.Value;
            Properties.Settings.Default.CheckForUpdates = cbCheckForUpdates.IsChecked.Value;
            Properties.Settings.Default.Save();
            Globals.IsNoiseEnabled = (bool)cbPropellerNoiseSim.IsChecked;
            Globals.isHypoxemiaEnabled = (bool)cbHypoxemiaSim.IsChecked;
        }
    }
}
