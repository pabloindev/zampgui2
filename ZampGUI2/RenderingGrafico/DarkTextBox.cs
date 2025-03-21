using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;



public class DarkTextBox : TextBox
{
    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        SetWindowTheme(this.Handle, "DarkMode_Explorer", null);
    }

    public DarkTextBox()
    {
        this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.ForeColor = System.Drawing.Color.White;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Multiline = true;
        this.ScrollBars = ScrollBars.Vertical;
    }
}

