// Lic:
// Stach
// Hex viewer for Stach
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
// Version: 20.06.02
// EndLic
using System;
using System.IO;
using System.Text;
using TrickyUnits;
using Kitty;

namespace Stach {
    static class HexViewer {

        static KittyOutput Console;

        static public bool IsBinary(string check) {
            var i = check.IndexOf((char)26);
            return i >= 0 && i <= check.Length - 1;
        }

        static public void View(string dat, string file) => View(dat.ToCharArray(), file);

        static public void View(char[] dat, string file) {
            //var s = new StringBuilder($"<h2>File: {file}</h2><pre>");
            var t = new StringBuilder("");
            var p = 0;
            Console = new KittyViewer();
            Console.WriteLine($"<h2>File: {file}</h2><pre>");
            for (int i = 0; i < dat.Length; ++i) {
                p = i % 16;
                var b = (byte)dat[i];
                if (p == 0) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"  {t}\n");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(Fmt.sprintf("%09X | ", i));
                    t.Clear();
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.Write(Fmt.sprintf("%02X ", (byte)dat[i]));
                Console.Write(("0" + b.ToString("X")).Substring(0, 2) + " ");
                if (b >= 32 && b < 127)
                    t.Append(dat[i]);
                else
                    t.Append(".");
            }
            for (int i = p; i < 15; ++i) {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("   ");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {t}");
            Directory.CreateDirectory(Core.ViewSwap);
            QuickStream.SaveString($"{Core.ViewSwap}/ViewFile.html", Console.ToString());
            Console = null;
        }

    }
}