// Lic:
// Stach
// Kitty Code Viewer
// 
// 
// 
// (c) Jeroen P. Broks, 2020
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
// Version: 20.06.01
// EndLic
???using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using TrickyUnits;
using Kitty;

namespace Stach {
    class KittyViewer : KittyOutput {
        Dictionary<ConsoleColor, string> Cols = new Dictionary<ConsoleColor, string>();
        StringBuilder sb = new StringBuilder();
        string File = "";

        public KittyViewer() {
            Cols[ConsoleColor.Black] = "#000000";
            Cols[ConsoleColor.DarkBlue] = "#0000ff";
            Cols[ConsoleColor.DarkCyan] = "#007f7f";
            Cols[ConsoleColor.DarkGray] = "#303030";
            Cols[ConsoleColor.DarkGreen] = "#007f00";
            Cols[ConsoleColor.DarkMagenta] = "#7f007f";
            Cols[ConsoleColor.DarkRed] = "#7f0000";
            Cols[ConsoleColor.DarkYellow] = "#7f7f00";
            Cols[ConsoleColor.Gray] = "#7f7f7f";
            Cols[ConsoleColor.Green] = "#00ff00";
            Cols[ConsoleColor.Magenta] = "#ff00ff";
            Cols[ConsoleColor.Red] = "#ff0000";
            Cols[ConsoleColor.White] = "#ffffff";
            Cols[ConsoleColor.Yellow] = "#ffff00";
            Cols[ConsoleColor.Blue] = "#007fff";
            Cols[ConsoleColor.Cyan] = "#00ffff";
        }

        override public string ToString() {
            var ret = "";
            if (File != "") ret = $"Viewing: {File}<p>";
            return $"<html><body style='background-color:black; color:white'>{ret}<pre>{sb}</pre></body></html>";
        }

        public override void Write(string a) {
            sb.Append($"<span style='color: {Cols[ForegroundColor]}; background-color: {Cols[BackgroundColor]}'>{a}</span>");
        }

        public override void WriteLine(string a) => Write($"{a}\n");

        static bool LineNumbers => Core.Config[Core.Platform, "LINENUMBERS"] != "FALSE";
        static bool InitDone = false;

        static public void Init() {
            InitDone = true;
            MKL.Lic    ("Stach - KittyViewer.cs","GNU General Public License 3");
            MKL.Version("Stach - KittyViewer.cs","20.06.01");
            Debug.WriteLine("Init Kitty Source View Drivers");
            KittyHigh.Init();
            new KittyBlitzMax();
            new KittyHighBASIC();
            new KittyHighBlitzBasic();
            new KittyHighBrainFuck();
            new KittyHighC();
            new KittyHighCobra();
            new KittyHighCS();
            new KittyHighGINI();
            new KittyHighGo();
            new KittyHighHtml();
            new KittyHighINI();
            new KittyHighJava();
            new KittyHighJavaScript();
            new KittyHighJSON();
            new KittyHighLua();
            new KittyHighNIL();
            new KittyHighPascal();
            new KittyHighPython();
            new KittyHighSASKIA();
            new KittyHighScyndi();
            new KittyHighVB();
            new KittyHighWhiteSpace();
            new KittyHighXml();
            //new KittyNiks();            
        }

        static public void View(string file,string showme) {
            if (!InitDone) Init();
            Debug.WriteLine("Setting up view");
            var KV = new KittyViewer();
            var ext = qstr.ExtractExt(file).ToLower();
            var odir = Core.Config[Core.Platform, "VIEWSWAP"];
            Debug.WriteLine("Linking to Kitty");
            KittyHigh.Console = (KittyOutput)KV;
            if (KittyHigh.Langs.ContainsKey(ext))
                KittyHigh.Langs[ext].Show(showme, LineNumbers);
            else
                KittyHigh.Langs["OTHER"].Show(showme, LineNumbers);
            Directory.CreateDirectory(odir);
            QuickStream.SaveString($"{odir}/ViewFile.html", KV.ToString());

        }
    }
}