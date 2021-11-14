using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WindowsFormsHelper
{
    static class Helper
    {
        class PropertiesForm : Form
        {
            bool ignoreEvent = false;

            public PropertiesForm(Control control)
            {
                DataGridView data = new DataGridView();
                data.Dock = DockStyle.Fill;
                data.RowHeadersVisible = false;
                data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                data.CellValueChanged += (object sender, DataGridViewCellEventArgs e) =>
                {
                    if (ignoreEvent)
                        return;
                    string propertyName = data[0, e.RowIndex].Value.ToString();
                    PropertyInfo propertyInfo = control.GetType().GetProperty(propertyName);
                    object value = data[1, e.RowIndex].Value;

                    void setValueText()
                    {
                        ignoreEvent = true;
                        data[1, e.RowIndex].Value = propertyInfo.GetValue(control);
                        ignoreEvent = false;
                    }

                    try
                    {
                        string string_value = value.ToString();

                        // Set struct
                        if (propertyInfo.PropertyType.IsValueType && !propertyInfo.PropertyType.IsEnum && string_value.StartsWith("{") && string_value.EndsWith("}"))
                        {
                            object struct_object = propertyInfo.GetValue(control);
                            foreach (string v in string_value.Trim('{', '}').Split(','))
                            {
                                string[] s = v.Split('=');
                                PropertyInfo struct_propetyInfo = struct_object.GetType().GetProperty(s[0].Trim(' '));
                                struct_propetyInfo.SetValue(struct_object, Convert.ChangeType(s[1].Trim(' '), struct_propetyInfo.PropertyType));
                            }
                            value = struct_object;
                        }

                        // Set enum
                        if (propertyInfo.PropertyType.IsEnum)
                            value = Enum.Parse(propertyInfo.PropertyType, string_value);

                        propertyInfo.SetValue(control, Convert.ChangeType(value, propertyInfo.PropertyType));
                        // Format text
                        setValueText();
                    }
                    catch (Exception exception)
                    {
                        string caption = exception.Message == "Property set method not found." ? "Failed to set Property" : "Convertion Error";
                        MessageBox.Show(exception.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Restore text
                        setValueText();
                    }
                };

                DataGridViewColumn names = new DataGridViewTextBoxColumn(), values = new DataGridViewTextBoxColumn();
                names.HeaderText = "Name";
                values.HeaderText = "Value";
                names.ReadOnly = true;
                names.AutoSizeMode = values.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                values.SortMode = DataGridViewColumnSortMode.NotSortable;
                
                data.Columns.AddRange(names, values);
                foreach (PropertyInfo property in control.GetType().GetProperties())
                    data.Rows.Add(new object[] { property.Name, property.GetValue(control) });

                Controls.Add(data);
                Size = new Size(names.GetPreferredWidth(names.AutoSizeMode, true) + values.GetPreferredWidth(values.AutoSizeMode, true) + 36, 327);
                Text = "Properties of " + (control.Name.Length == 0 ? control.GetType().ToString() : control.Name);
            }
        }

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
                dragged.Location = new Point(dragged.Location.X + e.Location.X - lastLocation.X, dragged.Location.Y + e.Location.Y - lastLocation.Y);
        }

        static void MouseUp(object sender, MouseEventArgs e)
        {
            dragged.FindForm().Cursor = Cursors.Default;
            dragged = null;
        }

        static void KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    resizeMode = !resizeMode;
                    SetText(selected);
                    break;

                case Keys.F3:
                    new PropertiesForm(selected).ShowDialog();
                    break;
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
}
