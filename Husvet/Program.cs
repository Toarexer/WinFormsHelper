﻿using System;
using System.Windows.Forms;

namespace Husvet
{
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
