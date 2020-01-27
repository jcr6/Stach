// Lic:
// Stach
// Main Window
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 20.01.27
// EndLic
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrickyUnits;

namespace Stach {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        #region Quick Link
        string Platform => Core.Platform;
        #endregion

        #region Quick features
        bool AutoConfigUpdate = false;
        #endregion

        void UpdateResourceHeader() {
            //Debug.WriteLine($"{Core.Resource}");
            txt_Resource.Text = Core.Resource.Replace('\\', '/');
            txt_Directory.Text = Core.CDirectory.Replace('\\','/');
            Debug.WriteLine("Updating Resource Header");
        }

        #region CallBack
        public MainWindow() {
            try {
                InitializeComponent();
                MKL.Version("Stach - MainWindow.xaml.cs", "20.01.27");
                MKL.Lic("Stach - MainWindow.xaml.cs", "GNU General Public License 3");
                Title = $"Stach - (c) {MKL.CYear(2020)} Jeroen P. Broks";
                ExampleSwap.Text = Core.Config[Platform, "ExampleSwap"];
                foreach(string ad in Dirry.AltDriveList) {
                    List_System.Items.Add(ad);
                }
                var startd = Directory.GetCurrentDirectory().Replace('\\', '/');
                Core.IN_Resource = false;
                Core.CDirectory =startd;
                UpdateResourceHeader();
            } catch (Exception e) {
#if DEBUG
                Confirm.Annoy($"{e}\n\n{e.StackTrace}\n\nI will try to continue, but expect things NOT to go so well!", "Start up error!", System.Windows.Forms.MessageBoxIcon.Error);

#else
                Confirm.Annoy($"{e.Message}\n\nI will try to continue, but expect things NOT to go so well!", "Start up error!", System.Windows.Forms.MessageBoxIcon.Error);
#endif
            } finally {
                AutoConfigUpdate = true;
            }
        }

        private void BrowseExample(object sender, RoutedEventArgs e) {
            var file = FFS.RequestDir();
            if (file != "") ExampleSwap.Text = file;
        }

        private void ExampleSwap_TextChanged(object sender, TextChangedEventArgs e) {
            if (AutoConfigUpdate) {
                Core.Config[Platform, "ExampleSwap"] = ExampleSwap.Text.Replace('\\','/');
            }
        }
        #endregion
    }
}