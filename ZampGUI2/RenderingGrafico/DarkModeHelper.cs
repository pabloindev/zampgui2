using System.Runtime.InteropServices;

namespace ZampGUI2.RenderingGrafico
{
    public class DarkModeHelper
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_CAPTION_COLOR = 35;

        public static void SetDarkMode(IntPtr handle, bool darkMode)
        {
            int value = darkMode ? 1 : 0;
            DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
        }

        public static void SetCaptionColor(IntPtr handle, Color color)
        {
            int colorValue = ColorToInt(color);
            DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, ref colorValue, sizeof(int));
        }

        private static int ColorToInt(Color color)
        {
            return (color.R) | (color.G << 8) | (color.B << 16);
        }
    }
}
