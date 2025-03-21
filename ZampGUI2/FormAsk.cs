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
using ZampGUI2.RenderingGrafico;

namespace ZampGUI2
{
    public partial class FormAsk : Form
    {
        public string tipoSalvataggio { get; set; }
        public string valoreInserito { get; private set; }
        public IniFile config {  get; set; }

        public FormAsk(string tipoSalvataggio, IniFile config)
        {
            InitializeComponent();
            this.tipoSalvataggio = tipoSalvataggio;
            this.config = config;

            if (this.tipoSalvataggio == "http_port")
            {
                label1.Text = "Please insert new http port";
                textBox1.Text = config.Read("Porte", "httpPort");
            }
            else if (this.tipoSalvataggio == "editor")
            {
                label1.Text = "Please insert full path for the editor (default 'notepad.exe')";
                textBox1.Text = config.Read("ImpostazioniGenerali", "editor");
            }
            
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            if (this.tipoSalvataggio == "http_port")
            {
                if(int.TryParse(textBox1.Text, out _))
                {
                    valoreInserito = textBox1.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Number");
                }
            }
            else if (this.tipoSalvataggio == "editor")
            {
                if (textBox1.Text.ToLower() == "notepad.exe" || textBox1.Text.ToLower() == "notepad" || System.IO.File.Exists(textBox1.Text))
                {
                    valoreInserito = textBox1.Text.Trim();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The file does not exist");
                }
            }
                
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            //Per la barra del titolo (title bar) scura

            base.OnHandleCreated(e);

            // Attiva la dark mode per la barra del titolo
            DarkModeHelper.SetDarkMode(this.Handle, true);

            // Imposta un colore personalizzato per la barra del titolo
            DarkModeHelper.SetCaptionColor(this.Handle, Color.FromArgb(32, 32, 32));
        }
    }
}
