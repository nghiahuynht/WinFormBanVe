using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinApp.Controls
{
    public class GradientPanel : Panel
    {
        private Color _colorTop = Color.MediumSlateBlue;
        private Color _colorBottom = Color.MediumPurple;
        private int _cornerRadius = 20;

        [Category("Appearance")]
        public Color ColorTop
        {
            get => _colorTop;
            set { _colorTop = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color ColorBottom
        {
            get => _colorBottom;
            set { _colorBottom = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; Invalidate(); }
        }

        public GradientPanel()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using (var path = GetRoundedRectanglePath(rect, _cornerRadius))
            using (var brush = new LinearGradientBrush(rect, _colorTop, _colorBottom, 90f))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            Rectangle arc = new Rectangle(rect.Location, new Size(d, d));

            // TL
            path.AddArc(arc, 180, 90);
            // TR
            arc.X = rect.Right - d;
            path.AddArc(arc, 270, 90);
            // BR
            arc.Y = rect.Bottom - d;
            path.AddArc(arc, 0, 90);
            // BL
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
