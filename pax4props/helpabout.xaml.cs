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
using System.Reflection;
using System.Diagnostics;

namespace pax4props
{
    /// <summary>
    /// Interaction logic for helpabout.xaml
    /// </summary>
    public partial class HelpAbout : Window
    {
        public HelpAbout()
        {
            InitializeComponent();
            LblVersion.Content += Assembly.GetEntryAssembly().GetName().Version.ToString();
            TBDisclaimer.Text = @"This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.";
        }

        private void BtnDoc_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("readme.html"); //TODO: safer way to show help
        }
    }
}
