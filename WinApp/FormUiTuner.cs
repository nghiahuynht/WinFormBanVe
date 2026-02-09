using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinApp
{
    public partial class FormUiTuner : Form
    {
        private readonly FormBanVe _target;
        private readonly UiTuneSettings _s = new UiTuneSettings();

        // UI controls
        private NumericUpDown nudCounterRightPad;
        private NumericUpDown nudCounterY;
        private Button btnCounterColor;

        private NumericUpDown nudTicketRowHeight;
        private NumericUpDown nudGroupHeaderHeight;
        private NumericUpDown nudRowPaddingY;

        private Button btnApply;
        private Button btnReset;
        private CheckBox chkLive;

        public FormUiTuner(FormBanVe target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));

            // ❌ bỏ dòng này vì bạn không có Designer.cs
            // InitializeComponent();

            BuildUi();
            LoadDefaultsToUi();
            ApplyToTarget();
        }

        private void BuildUi()
        {
            Text = "UI TUNER - FormBanVe";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(520, 360);

            var root = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                BackColor = Color.White
            };
            Controls.Add(root);

            int y = 10;

            // ===== COUNTER SECTION =====
            var lbl1 = new Label
            {
                Parent = root,
                Text = "Counter label (ĐẾM SỐ NÈ)",
                AutoSize = true,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(10, y)
            };
            y += 32;

            nudCounterRightPad = MakeNud(root, "RightPad (tăng = xích qua trái):", 10, y, 0, 300, 50);
            y += 40;

            nudCounterY = MakeNud(root, "Y (cao/thấp):", 10, y, 0, 200, 14);
            y += 40;

            btnCounterColor = new Button
            {
                Parent = root,
                Text = "Đổi màu Counter",
                Location = new Point(10, y),
                Size = new Size(170, 34)
            };
            btnCounterColor.Click += (s, e) =>
            {
                using var cd = new ColorDialog { Color = _s.CounterForeColor };
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    _s.CounterForeColor = cd.Color;
                    btnCounterColor.ForeColor = cd.Color;
                    LiveApply();
                }
            };

            y += 52;

            // ===== GRID SECTION =====
            var lbl2 = new Label
            {
                Parent = root,
                Text = "Grid (khoảng cách dòng vé)",
                AutoSize = true,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(10, y)
            };
            y += 32;

            nudTicketRowHeight = MakeNud(root, "Ticket row height:", 10, y, 24, 80, 32);
            y += 40;

            nudGroupHeaderHeight = MakeNud(root, "Group header height:", 10, y, 24, 90, 38);
            y += 40;

            nudRowPaddingY = MakeNud(root, "Row padding Y (đệm chữ):", 10, y, 0, 20, 4);
            y += 52;

            // ===== ACTIONS =====
            chkLive = new CheckBox
            {
                Parent = root,
                Text = "Live apply (đổi là áp dụng luôn)",
                AutoSize = true,
                Location = new Point(10, y),
                Checked = true
            };

            btnApply = new Button
            {
                Parent = root,
                Text = "APPLY",
                Location = new Point(300, y - 6),
                Size = new Size(90, 34)
            };
            btnApply.Click += (s, e) => ApplyToTarget();

            btnReset = new Button
            {
                Parent = root,
                Text = "RESET",
                Location = new Point(400, y - 6),
                Size = new Size(90, 34)
            };
            btnReset.Click += (s, e) =>
            {
                _s.ResetDefaults();
                LoadDefaultsToUi();
                ApplyToTarget();
            };

            // live events
            nudCounterRightPad.ValueChanged += (_, __) => { _s.CounterRightPad = (int)nudCounterRightPad.Value; LiveApply(); };
            nudCounterY.ValueChanged += (_, __) => { _s.CounterY = (int)nudCounterY.Value; LiveApply(); };
            nudTicketRowHeight.ValueChanged += (_, __) => { _s.TicketRowHeight = (int)nudTicketRowHeight.Value; LiveApply(); };
            nudGroupHeaderHeight.ValueChanged += (_, __) => { _s.GroupHeaderHeight = (int)nudGroupHeaderHeight.Value; LiveApply(); };
            nudRowPaddingY.ValueChanged += (_, __) => { _s.RowPaddingY = (int)nudRowPaddingY.Value; LiveApply(); };
        }

        private void LoadDefaultsToUi()
        {
            nudCounterRightPad.Value = _s.CounterRightPad;
            nudCounterY.Value = _s.CounterY;
            nudTicketRowHeight.Value = _s.TicketRowHeight;
            nudGroupHeaderHeight.Value = _s.GroupHeaderHeight;
            nudRowPaddingY.Value = _s.RowPaddingY;

            btnCounterColor.ForeColor = _s.CounterForeColor;
        }

        private void LiveApply()
        {
            if (chkLive.Checked) ApplyToTarget();
        }

        private void ApplyToTarget()
        {
            _s.ApplyTo(_target);
        }

        private static NumericUpDown MakeNud(Control parent, string text, int x, int y, int min, int max, int val)
        {
            _ = new Label
            {
                Parent = parent,
                Text = text,
                AutoSize = true,
                Location = new Point(x, y + 6)
            };

            var nud = new NumericUpDown
            {
                Parent = parent,
                Location = new Point(260, y),
                Size = new Size(110, 27),
                Minimum = min,
                Maximum = max,
                Value = val
            };
            return nud;
        }

        // ================= SETTINGS (KHÔNG ĐỤNG PRIVATE FIELD) =================
        private sealed class UiTuneSettings
        {
            public int CounterRightPad { get; set; } = 50;     // tăng -> xích trái
            public int CounterY { get; set; } = 14;            // cao/thấp
            public Color CounterForeColor { get; set; } = Color.Red;

            public int TicketRowHeight { get; set; } = 32;
            public int GroupHeaderHeight { get; set; } = 38;
            public int RowPaddingY { get; set; } = 4;

            public void ResetDefaults()
            {
                CounterRightPad = 50;
                CounterY = 14;
                CounterForeColor = Color.Red;

                TicketRowHeight = 32;
                GroupHeaderHeight = 38;
                RowPaddingY = 4;
            }

            public void ApplyTo(FormBanVe f)
            {
                if (f == null) return;

                // 1) grid
                var gv = FindFirst<DataGridView>(f);
                if (gv != null)
                {
                    // padding cho chữ trong cell (tạo cảm giác “giãn dòng”)
                    gv.DefaultCellStyle.Padding = new Padding(
                        gv.DefaultCellStyle.Padding.Left,
                        RowPaddingY,
                        gv.DefaultCellStyle.Padding.Right,
                        RowPaddingY
                    );

                    foreach (DataGridViewRow r in gv.Rows)
                    {
                        if (r == null) continue;

                        bool isGroupHeader = r.Tag != null && r.Tag.GetType().Name == "GroupHeaderTag";
                        r.Height = isGroupHeader ? GroupHeaderHeight : TicketRowHeight;
                    }

                    gv.Invalidate();
                }

                // 2) counter label (scan controls, không đụng private)
                var leftCard = FindLeftCardPanel(f);
                var counter = FindCounterLabel(f);

                if (leftCard != null && counter != null)
                {
                    counter.ForeColor = CounterForeColor;
                    counter.BackColor = Color.Transparent;
                    counter.AutoSize = true;

                    int x = leftCard.ClientSize.Width - counter.PreferredWidth - CounterRightPad;
                    x = Math.Max(x, 52 + 260); // né title bên trái (giữ logic như bạn đang làm)

                    counter.Location = new Point(x, CounterY);
                    counter.BringToFront();
                }
            }

            private static Panel FindLeftCardPanel(FormBanVe f)
            {
                // Tìm panel/card đang chứa DataGridView menu
                var gv = FindFirst<DataGridView>(f);
                if (gv == null) return null;

                Control p = gv.Parent;
                while (p != null)
                {
                    if (p is Panel panel)
                    {
                        // card trái có title “GIỎ HÀNG / ĐƠN ĐANG BÁN”
                        bool hasTitle = panel.Controls.OfType<Label>()
                            .Any(l => (l.Text ?? "").Contains("GIỎ HÀNG / ĐƠN ĐANG BÁN"));

                        if (hasTitle) return panel;
                    }
                    p = p.Parent;
                }
                return null;
            }

            private static Label FindCounterLabel(FormBanVe f)
            {
                // ưu tiên đúng tên designer
                var byName = FindFirstByName<Label>(f, "lblCounterCartNum");
                if (byName != null) return byName;

                // fallback theo text
                var byText = FindFirstByPredicate<Label>(f, lb => (lb.Text ?? "").Contains("ĐẾM SỐ NÈ"));
                if (byText != null) return byText;

                return null;
            }

            private static T FindFirst<T>(Control root) where T : Control
            {
                foreach (Control c in root.Controls)
                {
                    if (c is T t) return t;
                    if (c.HasChildren)
                    {
                        var x = FindFirst<T>(c);
                        if (x != null) return x;
                    }
                }
                return null;
            }

            private static T FindFirstByName<T>(Control root, string name) where T : Control
            {
                foreach (Control c in root.Controls)
                {
                    if (c is T t && string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase))
                        return t;

                    if (c.HasChildren)
                    {
                        var x = FindFirstByName<T>(c, name);
                        if (x != null) return x;
                    }
                }
                return null;
            }

            private static T FindFirstByPredicate<T>(Control root, Func<T, bool> predicate) where T : Control
            {
                foreach (Control c in root.Controls)
                {
                    if (c is T t && predicate(t)) return t;

                    if (c.HasChildren)
                    {
                        var x = FindFirstByPredicate<T>(c, predicate);
                        if (x != null) return x;
                    }
                }
                return null;
            }
        }
    }
}
