using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Husvet
{
    public partial class HusvetForm : Form
    {
        public HusvetForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e, string textBoxText, string ListBoxText)
        {
            if (textBoxText == "Ide írd a neved!")
                MessageBox.Show("Kérlek, írd be a neved!");
            else
                MessageBox.Show($"{textBoxText} {ListBoxText}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedIndex == 0)
                MessageBox.Show("Valódi nemedet válaszd ki!");
            else
                MessageBox.Show(((ListBox)sender).SelectedItem.ToString());
        }
    }
}
