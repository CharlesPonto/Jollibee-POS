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

        //transaction variables
        private decimal totalAmount;
        private decimal change; 
        private decimal subTotal;

        //menu variable
        private string productName;
        private decimal productPrice;
        public Form1()
        {
            InitializeComponent();
        }

        //numpad
        private void numpadClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                txtAmountPaid.Text += clickedButton.Text;

                string amountText = txtAmountPaid.Text.Replace("₱", "");
                if (decimal.TryParse(amountText, out decimal amount))
                {
                    totalAmount = amount;
                }
            }
        }
        private void numpadClearClick(object sender, EventArgs e)
        {
            txtAmountPaid.Text = "₱";
        }

        //order click
        private void order_Click(object sender, EventArgs e)
        {
            //use the parent if clicked (label or image)
            Panel clickedPanel = sender as Panel ?? (sender as Control).Parent as Panel;

            string productName = "";
            decimal productPrice = 0;

            //get name and price
            foreach (Control control in clickedPanel.Controls)
            {
                if (control is Label label)
                {
                    if ((string)label.Tag == "Name")
                    {
                        productName = label.Text;
                    }

                    if ((string)label.Tag == "Price")
                    {
                        string priceText = label.Text.Replace("₱", "").Trim();
                        productPrice = Convert.ToDecimal(priceText);
                    }
                }
            }
            MessageBox.Show($"Name: {productName}\nPrice: {productPrice}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //iterates to my table (menu)
            foreach (Control control in menuTableLayout.Controls)
            {
                if (control is Panel menuPanel)
                {
                    menuPanel.Click += order_Click;

                    //(labels + image) -> order_Click event
                    foreach (Control child in menuPanel.Controls)
                    {
                        child.Click += order_Click;
                    }
                }
            }
        }
    }
}
