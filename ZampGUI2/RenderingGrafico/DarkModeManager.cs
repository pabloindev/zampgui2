using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ZampGUI2.RenderingGrafico
{
    public static class DarkModeManager
    {
        [DllImport("uxtheme.dll", EntryPoint = "#138")]
        private static extern bool SetPreferredAppMode(PreferredAppMode mode);

        [DllImport("uxtheme.dll", EntryPoint = "#135")]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private enum PreferredAppMode
        {
            Default,
            AllowDark,
            ForceDark,
            ForceLight,
            Max
        }

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_CAPTION_COLOR = 35;

        public static void EnableDarkMode(Form form)
        {
            // Abilita la dark mode a livello di applicazione
            SetPreferredAppMode(PreferredAppMode.ForceDark);

            // Imposta la barra del titolo in modalità scura
            int darkMode = 1;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));

            // Imposta un colore personalizzato per la caption
            int captionColor = ColorToInt(Color.FromArgb(32, 32, 32));
            DwmSetWindowAttribute(form.Handle, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));

            // Applica il tema scuro ai controlli con scrollbar
            ApplyDarkScrollbarsToAll(form);
        }

        private static int ColorToInt(Color color)
        {
            return (color.R) | (color.G << 8) | (color.B << 16);
        }

        private static void ApplyDarkScrollbarsToAll(Control control)
        {
            try
            {
                // Applica specificamente a controlli che supportano scrollbar
                if (control is TextBox || control is RichTextBox ||
                    control is ListBox || control is ComboBox ||
                    control is Panel || control is DataGridView)
                {
                    SetWindowTheme(control.Handle, "Explorer", null);

                    // Per TextBox e RichTextBox, possiamo anche personalizzare ulteriormente
                    if (control is TextBox txt)
                    {
                        txt.BackColor = Color.FromArgb(45, 45, 45);
                        txt.ForeColor = Color.White;
                        txt.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else if (control is RichTextBox rtb)
                    {
                        rtb.BackColor = Color.FromArgb(45, 45, 45);
                        rtb.ForeColor = Color.White;
                        rtb.BorderStyle = BorderStyle.FixedSingle;
                    }
                }

                // Applica ricorsivamente ai controlli figli
                foreach (Control child in control.Controls)
                {
                    ApplyDarkScrollbarsToAll(child);
                }
            }
            catch (Exception)
            {
                // Gestisci eventuali errori silenziosamente
            }
        }
    }
}
