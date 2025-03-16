using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2.RenderingGrafico
{
    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(62, 62, 66); // Colore selezione voce
        public override Color MenuBorder => Color.FromArgb(28, 28, 28); // Bordo del menu
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(51, 51, 55); // Gradiente pressione inizio
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(51, 51, 55); // Gradiente pressione fine
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(62, 62, 66); // Gradiente selezione inizio
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(62, 62, 66); // Gradiente selezione fine
        public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 48); // Sfondo dropdown
    }
}
