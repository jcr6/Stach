// Lic:
// Stach
// Core
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
using System.Linq;
using System.Text;
using System.Diagnostics;
using TrickyUnits;

namespace Stach {
    static class Core {
        static public GINIE Config;
        static public string Platform => $"{Environment.OSVersion.Platform}";

        static public bool IN_Resource = false;
        static string __Resource="";
        static public string Resource {
            set { __Resource = value; IN_Resource = true; }
            get {
                if (!IN_Resource) return "* File System *";
                return __Resource;
            }
        }
        static public string CDirectory;

        static Core() {
            MKL.Lic    ("Stach - Core.cs","GNU General Public License 3");
            MKL.Version("Stach - Core.cs","20.01.28");
            FFS.Hello();
            Debug.WriteLine($"Running on {Platform}");
            Dirry.InitAltDrives();
            Config = GINIE.FromFile($"{Dirry.C("$AppSupport$/Stach.ini")}");
            Config.AutoSaveSource = $"{Dirry.C("$AppSupport$/Stach.ini")}";
        }
    }
}