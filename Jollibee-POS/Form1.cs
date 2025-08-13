using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jollibee_POS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // for numpad
        private void numpadClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                txtAmountPaid.Text += clickedButton.Text;
            }
        }

        private void numpadClearClick(object sender, EventArgs e)
        {
            txtAmountPaid.Text = "₱";
        }

     
    }
}
