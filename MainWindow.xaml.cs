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
// Version: 20.01.28
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
using UseJCR6;

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

        void UpdateDirBox() {           
            UpdateResourceHeader();
            DirBox.Items.Clear();
            if (Core.IN_Resource) {
                var files = new List<string>();
                var dirs = new List<string>();
                foreach(TJCREntry e in Core.JCR.Entries.Values) {
                    var d = qstr.ExtractDir(e.Entry).ToUpper();
                    Debug.WriteLine($"{d} => {e.Entry}");
                    if (d == Core.CDirectory) {
                        files.Add(qstr.StripDir(e.Entry));
                    }
                    if (d != "") {
                        var dd = qstr.ExtractDir(d).ToUpper();                        
                        if (dd == Core.CDirectory && (!dirs.Contains(qstr.StripDir(d.ToUpper()))))
                            dirs.Add(qstr.StripDir(d.ToUpper()));
                        else if (d.Length>Core.CDirectory.Length && (Core.CDirectory=="" || qstr.Prefixed (d,Core.CDirectory))) {
                            var ded = qstr.Right(d.ToUpper(), d.Length-Core.CDirectory.Length);
                            var p = ded.IndexOf('/');
                            if (p>=0) {
                                var dr = ded.Substring(0, p);
                                if (dr!="" && (!dirs.Contains(dr)))
                                    dirs.Add(dr);
                            }
                        }

                    }
                }
                dirs.Sort();
                files.Sort();
                DirBox.Items.Add("../");
                foreach (string d in dirs) DirBox.Items.Add($"{d}/");
                foreach (string f in files) DirBox.Items.Add(f);
            } else {
                try {
                    DirBox.Items.Add("../");
                    foreach (string f in FileList.GetDir(Core.CDirectory, 1)) {
                        var ff = $"{Core.CDirectory}/{f}";
                        if (Directory.Exists(ff)) {
                            DirBox.Items.Add($"{f}/");
                        } else {
                            DirBox.Items.Add($"{f}");
                        }
                    }
                } catch(Exception E) {
                    Confirm.Annoy(E.Message, "Error during scanning directory", System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        #region CallBack
        public MainWindow() {
            try {
                InitializeComponent();
                MKL.Version("Stach - MainWindow.xaml.cs","20.01.28");
                MKL.Lic    ("Stach - MainWindow.xaml.cs","GNU General Public License 3");
                Title = $"Stach - (c) {MKL.CYear(2020)} Jeroen P. Broks";
                ExampleSwap.Text = Core.Config[Platform, "ExampleSwap"];
                foreach(string ad in Dirry.AltDriveList) {
                    List_System.Items.Add(ad);
                }
                var startd = Directory.GetCurrentDirectory().Replace('\\', '/');
                Core.IN_Resource = false;
                Core.CDirectory =startd;
                UpdateDirBox();
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

        private void DirBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        }

        private void DirBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DirBox.SelectedItem == null) return;
            var item = (string)DirBox.SelectedItem;
            //var d = item.Content.ToString();
            Debug.WriteLine($"Double-clicked {item}");
            if (!qstr.Suffixed(item, "/")) {
                if (Core.IN_Resource)
                    return;
                var rf = $"{Core.CDirectory}/{item}";
                var rc = JCR6.Recognize(rf);
                Debug.WriteLine($"Analysing {rf} => {rc}");
                if (rc == "NONE") return;
                Core.IN_Resource = true;
                Core.Resource = rf;
                Core.CDirectory = "";
                UpdateDirBox();
                return;
            }
            if (Core.IN_Resource) {
                if (item == "../") {
                    if (Core.CDirectory=="") {
                        Directory.SetCurrentDirectory(qstr.ExtractDir(Core.Resource));
                        Core.IN_Resource = false;
                        Core.CDirectory = Directory.GetCurrentDirectory();
                        UpdateDirBox();
                        return;
                    }
                    Core.CDirectory = qstr.ExtractDir(Core.CDirectory);
                    UpdateDirBox();
                    return;
                } else {
                    if (Core.CDirectory == "")
                        Core.CDirectory = item.Substring(0, item.Length - 1);
                    else
                        Core.CDirectory += $"/{item.Substring(0, item.Length - 1)}";
                    UpdateDirBox();
                    return;
                }
            } else {
                Directory.SetCurrentDirectory(item);
                Core.CDirectory = Directory.GetCurrentDirectory();
                UpdateDirBox();
            }
        }

        private void List_System_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (List_System.SelectedItem == null) return;
            var item = (string)List_System.SelectedItem;
            var D = Dirry.AD($"{item}:");
            Debug.WriteLine(D);
            Directory.SetCurrentDirectory(D);
            Core.IN_Resource = false;
            Core.CDirectory = Directory.GetCurrentDirectory();
            UpdateDirBox();
        }
    }
}