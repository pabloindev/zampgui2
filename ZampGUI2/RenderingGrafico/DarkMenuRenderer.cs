using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2.RenderingGrafico
{
    public class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuRenderer() : base(new DarkColorTable()) { }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // Non chiamare base.OnRenderItemCheck per evitare il rendering predefinito

            // Dimensioni del check
            Rectangle checkRect = new Rectangle(
                e.ImageRectangle.Left + 2,
                e.ImageRectangle.Top + 2,
                e.ImageRectangle.Width - 4,
                e.ImageRectangle.Height - 4);

            // Disegna lo sfondo del check (colore scuro)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            {
                e.Graphics.FillRectangle(brush, e.ImageRectangle);
            }

            // Disegna il simbolo di check (colore bianco)
            using (Pen pen = new Pen(Color.White, 2F))
            {
                // Disegna il check
                Point[] checkPoints = {
                new Point(checkRect.Left, checkRect.Top + checkRect.Height / 2),
                new Point(checkRect.Left + checkRect.Width / 3, checkRect.Bottom - 2),
                new Point(checkRect.Right, checkRect.Top)
            };

                e.Graphics.DrawLines(pen, checkPoints);
            }
        }
    }
}
