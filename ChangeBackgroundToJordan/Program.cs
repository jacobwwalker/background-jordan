using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ChangeBackgroundToJordan
{
    class Program
    {
        /* Some code taken from Neil N's response to a Stack Overflow question:
         * http://stackoverflow.com/questions/1061678/change-desktop-wallpaper-using-code-in-net
         */

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        static Image GetBackgroundImage()
        {
            Assembly thisExe = Assembly.GetExecutingAssembly();
            Stream backgroundImage = thisExe.GetManifestResourceStream(
                "ChangeBackgroundToJordan.wallpaper.bmp");

            return Image.FromStream(backgroundImage);
        }
        
        static void Main()
        {
            Image wallpaperImage = Program.GetBackgroundImage();
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            wallpaperImage.Save(tempPath, ImageFormat.Bmp);

            // Sets the wallpaper to be centred and untiled
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            registryKey.SetValue("WallpaperStyle", "1");
            registryKey.SetValue("TileWallpaper", "0");

            SystemParametersInfo(0x14, 0, tempPath, 0x01 | 0x02);
        }
    }
}
