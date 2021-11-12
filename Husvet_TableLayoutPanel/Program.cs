using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Husvet_TableLayoutPanel
{
    class LabeledTextBox : TableLayoutPanel
    {
        Label _label;
        TextBox _textbox;

        public Label Label => _label;
        public TextBox TextBox => _textbox;

        public LabeledTextBox(Label label, TextBox textbox)
        {
            _label = label;
            _textbox = textbox;
            Init();
        }

        public LabeledTextBox()
        {
            _label = new Label();
            _textbox = new TextBox();
            _label.Text = ":";
            Init();
        }

        void Init()
        {
            _label.TextAlign = ContentAlignment.MiddleRight;
            _label.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _textbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _label.Padding = _label.Margin = _textbox.Margin = Padding.Empty;

            // BackColor = Color.Red;
            Dock = DockStyle.Fill;
            ColumnCount = 2;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            Controls.Add(_label);
            Controls.Add(_textbox);
        }
    }

    static class Program
    {
        static TableLayoutPanel AddTableLayoutPanel(Form form, params object[] rows)
        {
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.SuspendLayout();
            tlp.Dock = DockStyle.Fill;
            tlp.TabStop = false;
            tlp.RowCount = rows.Length;
            tlp.ColumnCount = rows.Max(x => ((object[])x).Length);

            for (int row = 0; row < tlp.RowCount; row++)
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tlp.RowCount));

            for (int col = 0; col < tlp.ColumnCount; col++)
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tlp.ColumnCount));

            for (int row = 0; row < tlp.RowCount; row++)
                for (int col = 0; col < ((object[])rows[row]).Length; col++)
                    tlp.Controls.Add((Control)((object[])rows[row])[col], col, row);

            form.Controls.Add(tlp);
            tlp.ResumeLayout(false);
            return tlp;
        }

        static Stream StreamFromAssembly(string name) => Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Form1 form = new Form1();
            form.Text = "Első próbálkozás";
            form.Size = new Size(600, 400);

            Label lbl1 = new Label(), lbl2 = new Label();    
            lbl1.Text = "Első próbálkozás";
            lbl1.Dock = DockStyle.Fill;
            lbl1.TextAlign = ContentAlignment.TopCenter;       
            lbl2.Text = "Hogy hívnak?";
            lbl2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl2.TextAlign = ContentAlignment.MiddleRight;

            TextBox tbx1 = new TextBox();
            tbx1.Text = "Ide írd a neved!";
            tbx1.Dock = DockStyle.Top;

            ListBox lbx1 = new ListBox();
            lbx1.Items.AddRange(new object[] { "Neme?", "hölgy", "úr" });
            lbx1.Dock = DockStyle.Top;
            lbx1.SelectedIndex = 0;
            lbx1.Size = new Size(lbx1.Width, 32);
            lbx1.SelectedIndexChanged += (object sender, EventArgs e) => { MessageBox.Show(lbx1.SelectedIndex == 1 ? "Valódi nemedet válaszd ki!" : lbx1.SelectedItem.ToString()); };

            PictureBox pbx1 = new PictureBox();
            pbx1.Image = new Bitmap(StreamFromAssembly("Husvet_TableLayoutPanel.Husvet.jpg"));
            pbx1.Dock = DockStyle.Fill;
            pbx1.SizeMode = PictureBoxSizeMode.StretchImage;

            Button btn1 = new Button(), btn2 = new Button();
            btn1.Text = "Kész";
            btn1.Click += (object sender, EventArgs e) => { MessageBox.Show(tbx1.Text == "Ide írd a neved!" ? "Kérlek, írd be a neved!" : $"{tbx1.Text} {lbx1.SelectedItem}"); };
            btn1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btn2.Text = "Kilép";
            btn2.Click += (object sender, EventArgs e) => { Environment.Exit(0); };
            btn2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            TableLayoutPanel tbl = AddTableLayoutPanel(form,
                new object[] { lbl1 },
                // new object[] { lbl2, tbx1, lbx1 },
                new object[] { new LabeledTextBox(lbl2, tbx1), lbx1 },
                new object[] { pbx1, btn1 },
                new object[] { },
                new object[] { btn2 });

            tbl.Padding = new Padding(10);
            tbl.SetColumnSpan(lbl1, 2); // 3
            //tbl.SetColumnSpan(pbx1, 2);
            tbl.SetRowSpan(pbx1, 3);

            Application.Run(form);
        }
    }
}
