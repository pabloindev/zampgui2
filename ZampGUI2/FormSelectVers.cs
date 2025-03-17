using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZampGUI2.helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZampGUI2
{
    public partial class FormSelectVers : Form
    {
        public List<string> list { get; set; }
        public string selectedValue { get; set; }

        public FormSelectVers(List<string> list, string selectedValue)
        {
            InitializeComponent();
            this.list = list;
            this.selectedValue = selectedValue;

            foreach (var item in list)
            {
                comboBoxVersion.Items.Add(item);
            }
            comboBoxVersion.SelectedItem = selectedValue;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedValue = comboBoxVersion.SelectedItem?.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxVersion_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Imposta il colore di sfondo e del testo per ogni elemento
            e.DrawBackground();

            if (e.Index >= 0)
            {
                System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
                string text = combo.Items[e.Index].ToString();

                // Colore di sfondo e testo
                Brush brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? Brushes.DarkGray // Colore quando l'elemento è selezionato
                    : Brushes.Black; // Colore di sfondo predefinito

                e.Graphics.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawString(text, e.Font, Brushes.White, e.Bounds);
            }

            e.DrawFocusRectangle();
        }
    }
}
