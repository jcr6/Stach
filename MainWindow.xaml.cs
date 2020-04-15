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
// Version: 20.03.27
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

        static Dictionary<string, Label> EntryLink = new Dictionary<string, Label>();
        public sealed class CELink {
            Label this[string k] {
                get {
                    if (!EntryLink.ContainsKey(k)) return null;
                    return EntryLink[k];
                }
            }
        }
        public CELink ELink = new CELink();

        void ViewNothing() {
            Viewer.Navigate($"file://{qstr.ExtractDir(MKL.MyExe)}/Nothing.html");
        }

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
                foreach (TJCREntry e in Core.JCR.Entries.Values) {
                    var d = qstr.ExtractDir(e.Entry).ToUpper();
                    //Debug.WriteLine($"{d} => {e.Entry}");
                    if (d == Core.CDirectory) {
                        files.Add(qstr.StripDir(e.Entry));
                    }
                    /*
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
                    */
                }
                var drl = Core.DirList;
                foreach (string dr in drl) {
                    if (dr != "" && qstr.ExtractDir(dr) == Core.CDirectory && (!dirs.Contains(qstr.StripDir(dr)))) dirs.Add(qstr.StripDir(dr));
                }
                dirs.Sort();
                files.Sort();
                DirBox.Items.Add("../");
                foreach (string d in dirs) DirBox.Items.Add($"{d}/");
                foreach (string f in files) DirBox.Items.Add(f);
            } else {
                try {
                    ResType.Content = $"{Platform}:File Sytem";
                    ResTableSTG.Content = "N/A";
                    DirBox.Items.Add("../");
                    foreach (string f in FileList.GetDir(Core.CDirectory, 1)) {
                        var ff = $"{Core.CDirectory}/{f}";
                        if (Directory.Exists(ff)) {
                            DirBox.Items.Add($"{f}/");
                        } else {
                            DirBox.Items.Add($"{f}");
                        }
                    }
                } catch (Exception E) {
                    Confirm.Annoy(E.Message, "Error during scanning directory", System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        void RegisterEntryFields() {
            EntryLink["__Entry"] = Entry_Entry;
            EntryLink["__Size"] = Entry_Size;
            EntryLink["__CSize"] = Entry_CSize;
            EntryLink["__Storage"] = Entry_Storage;
            EntryLink["__Offset"] = Entry_Offset;
            EntryLink["__MD5HASH"] = Entry_MD5Hash;
            EntryLink["__JCR6FOR"] = Entry_JCR6FOR;
            EntryLink["__Author"] = Entry_Author;
            EntryLink["__Notes"] = Entry_Notes;
        }

        #region CallBack
        public MainWindow() {
            try {
                InitializeComponent();
                MKL.Version("Stach - MainWindow.xaml.cs","20.03.27");
                MKL.Lic    ("Stach - MainWindow.xaml.cs","GNU General Public License 3");
                Title = $"Stach - (c) {MKL.CYear(2020)} Jeroen P. Broks";
                ExampleSwap.Text = Core.Config[Platform, "ExampleSwap"];
                foreach(string ad in Dirry.AltDriveList) {
                    List_System.Items.Add(ad);
                }
                var startd = Directory.GetCurrentDirectory().Replace('\\', '/');
                RegisterEntryFields();
                Core.IN_Resource = false;
                Core.CDirectory =startd;
                UpdateDirBox();
                RenewUsed();
                ViewNothing();
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
            Entry_Alias.Items.Clear();
            Entry_Ratio.Content = "N/A";
            Entry_Main.Content = "--";
            if (DirBox.SelectedItem == null) {
                foreach (string EL in EntryLink.Keys) EntryLink[EL].Content = "--";
                return;
            }
            var item = (string)DirBox.SelectedItem;
            if (Core.IN_Resource) {
                var ename = $"{Core.CDirectory}/{item}";
                while (ename!="" && ename[0] == '/') ename = ename.Substring(1);
                if (qstr.Suffixed(ename, "/")){
                    foreach (string EL in EntryLink.Keys) EntryLink[EL].Content = "N/A";
                    Entry_Entry.Content = ename;
                    Entry_Type.Content = "Directory";
                } else {
                    var E = Core.JCR.Entries[ename.ToUpper()];
                    foreach(TJCREntry ECHK in Core.JCR.Entries.Values) {
                        if (ECHK != E && ECHK.MainFile == E.MainFile && ECHK.Offset == E.Offset) Entry_Alias.Items.Add(ECHK.Entry);
                    }
                    foreach (string k in E.databool.Keys) {
                        if (EntryLink.ContainsKey(k)) {
                            if (E.databool[k]) EntryLink[k].Content = "Yes"; else EntryLink[k].Content = "No";
                        }
                    }
                    foreach (string k in E.dataint.Keys) {
                        if (EntryLink.ContainsKey(k)) {
                            EntryLink[k].Content = $"{E.dataint[k]}";
                        }
                    }
                    foreach (string k in E.datastring.Keys) {
                        if (EntryLink.ContainsKey(k)) {
                            EntryLink[k].Content = $"{E.datastring[k]}";
                        }
                    }
                    Entry_Main.Content = E.MainFile;
                    Entry_Ratio.Content = E.Ratio;
                }

            }
        }

        void RenewUsed() {
            List_Used.Items.Clear();
            foreach (string u in Core.Config.List(Platform, "Used")) List_Used.Items.Add(u);
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
                Core.Config.ListAddNew(Platform,"Used",rf);
                RenewUsed();
                UpdateDirBox();
                ResTableSTG.Content = Core.JCR.FATstorage;
                ResType.Content = rc;
                return;
            }
            if (Core.IN_Resource) {
                if (item == "../") {
                    if (Core.CDirectory=="") {
                        Directory.SetCurrentDirectory(qstr.ExtractDir(Core.Resource));
                        Core.IN_Resource = false;
                        Core.CDirectory = Directory.GetCurrentDirectory().Replace('\\','/');
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
                Core.CDirectory = Directory.GetCurrentDirectory().Replace('\\','/');
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
            Core.CDirectory = Directory.GetCurrentDirectory().Replace('\\','/');
            UpdateDirBox();
        }

        private void Butt_Extract_Click(object sender, RoutedEventArgs e) {
            if (DirBox .SelectedItem == null) { Debug.WriteLine("No item, so no extract"); return; }
            var item = (string)DirBox.SelectedItem; Debug.WriteLine($"Item: {item}"); if (item =="" || qstr.Suffixed(item,"/")) { Debug.WriteLine("Item is directory, so no extract"); return; }
            var xas = FFS.RequestFile(true); if (xas == "") return;
            var ix = qstr.ExtractExt(item).ToLower();
            var ex = qstr.ExtractExt(xas).ToLower();
            if (ix!=ex) {
                switch (Confirm.YNC($"Original extention is \"{ix}\", and given extension is \"{ex}\". This does not match. Add this extension?")) {
                    case -1:
                        return;
                    case 0:
                        break;
                    case 1:
                        xas += $".{ix}";
                        break;
                    default:
                        throw new Exception("Invalid output from Confirm.YNC");
                }
            }
            if (Core.IN_Resource) {
                try {
                    var data = Core.JCR.JCR_B($"{Core.CDirectory}/{item}");
                    if (data == null) throw new Exception($"Unable to get data!\nJCR6 reported: {JCR6.JERROR}");
                    QuickStream.SaveBytes(xas, data);
                } catch (Exception E) {
                    Confirm.Annoy(E.Message, "Error!", System.Windows.Forms.MessageBoxIcon.Error);
                }
            } else {
                try {
                    File.Copy($"{Core.CDirectory}/{item}", xas, true);
                } catch (Exception E) {
                    Confirm.Annoy(E.Message, "Error!", System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void List_Used_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (List_Used.SelectedItem == null) return;
            var item = (string)List_Used.SelectedItem;
            var rf = $"{item}";
            var rc = JCR6.Recognize(rf);
            Debug.WriteLine($"Analysing {rf} => {rc}");
            if (rc == "NONE") {
                Confirm.Annoy($"{item} was not recognized");
                return;
            }
            Core.IN_Resource = true;
            Core.Resource = rf;
            Core.CDirectory = "";
            //Core.Config.ListAddNew(Platform, "Used", rf);
            //RenewUsed();
            UpdateDirBox();
            ResTableSTG.Content = Core.JCR.FATstorage;
            ResType.Content = rc;
            return;

        }
    }
}