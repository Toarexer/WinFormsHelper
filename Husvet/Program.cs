using System;
using System.Drawing;
using System.Windows.Forms;

namespace Husvet
{
    static class Helper
    {
        static Point lastLocation;
        static Size originalSize;
        static Control dragged, selected;
        static bool resizeMode;

        static void SetText(Control control)
        {
            if (control == null)
                return;
            string name = control.Name.Length == 0 ? control.GetType().ToString() : control.Name;
            control.FindForm().Text = resizeMode ? $"{name} - W: {control.Width} H: {control.Height}" : $"{name} - X: {control.Location.X} Y: {control.Location.Y}";
        }

        static void MouseDown(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
            dragged = selected;
            originalSize = dragged.Size;
            dragged.FindForm().Cursor = Cursors.Hand;
            SetText(selected);
        }

        static void MouseMove(object sender, MouseEventArgs e)
        {
            SetText(selected);

            if (dragged == null)
                return;

            if (resizeMode)
            {
                dragged.Width = originalSize.Width + e.Location.X - lastLocation.X;
                dragged.Height = originalSize.Height + e.Location.Y - lastLocation.Y;
            }
            else
            {
                Point location = dragged.Location;
                location.Offset(e.Location.X - lastLocation.X, e.Location.Y - lastLocation.Y);
                dragged.Location = location;
            }
        }

        static void MouseUp(object sender, MouseEventArgs e)
        {
            dragged.FindForm().Cursor = Cursors.Default;
            dragged = null;
        }

        static void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                resizeMode = !resizeMode;
                SetText(selected);
            }
        }

        static void MouseEnter(object sender, EventArgs e)
        {
            selected = (Control)sender;
        }

        public static void AddHelper(Form form)
        {
            foreach (Control control in form.Controls)
            {
                control.MouseDown += MouseDown;
                control.MouseMove += MouseMove;
                control.MouseUp += MouseUp;
                control.KeyDown += KeyDown;
                control.MouseEnter += MouseEnter;
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HusvetForm form = new HusvetForm();
            Helper.AddHelper(form);
            Application.Run(form);
        }
    }
}
