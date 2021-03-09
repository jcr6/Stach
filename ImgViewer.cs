// Lic:
// Stach
// Image Viewer for Stach
// 
// 
// 
// (c) Jeroen P. Broks, 2020, 2021
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
// Version: 21.03.09
// EndLic
using TrickyUnits;

namespace Stach {
    class ImgViewer {

        const string HTMLTemplate = @"<!DOCTYPE html> 
<html>  
    <head>  
        <title>Image View</title>  
        <style>  
            body {
                color: #ffb400;
                background-color:#000000;
                font-family: Arial;
            }
            .gfg { 
                width:auto; 
                text-align:center; 
                padding:20px; 
            } 
            img { 
                width: 100%; 
                height: 100%; 
                object-fit: contain; 
            } 
        </style>  
    </head>  
    <body>  
        <h1><HEADER></h1>
        <div class='gfg'> 
                <p id='my-image' ><img src='<URI>'></p> 
        </div> 
    </body>  
</html>";

        static public void ViewImg(byte[] Buffer,string File) {
            var ext = qstr.ExtractExt(File).ToLower();
            QuickStream.SaveBytes($"{Core.ViewSwap}/Image.{ext}", Buffer);
            QuickStream.SaveString($"{Core.ViewSwap}/ViewFile.html", HTMLTemplate.Replace("<HEADER>", $"Viewing: {File}").Replace("<URI>", $"{Core.ViewSwap}/Image.{ext}"));
        }

    }
}