using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Allow managed code to call unmanaged functions that are implemented in a DLL
using System.Runtime.InteropServices;

namespace winForms1
{
    public partial class Form1 : Form
    {
        private double c = 10;

        [DllImport("D:\\nightowl\\cplus\\WriteToSign\\Debug\\CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CP5200_Program_Create(ushort width, ushort height, byte colour);

        [DllImport("D:\\nightowl\\cplus\\WriteToSign\\Debug\\win32ConsoleApp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Add(double a, double b);

        public Form1()
        {
            InitializeComponent();
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
 

            var Object = CP5200_Program_Create(206, 66, 0x01);

        }
    }
}
