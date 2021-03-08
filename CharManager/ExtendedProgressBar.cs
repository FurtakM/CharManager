using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CharManager
{
    class ExtendedProgressBar : ProgressBar
    {
        public ExtendedProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            rec.Height -= 3;

            LinearGradientBrush brush = new LinearGradientBrush(rec, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }

    }
}
