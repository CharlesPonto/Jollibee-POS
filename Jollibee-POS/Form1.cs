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
            //get name and price
            foreach (Control control in myPanel.Controls)
            {
                if (control is Label label)
                {
                    if(label.Name == "pName")
                    {
                        productName = label.Text;
                    }
                    
                    if(label.Name == "pPrice")
                    {
                        string priceText = label.Text.Replace("₱", "");
                        productPrice = Convert.ToDecimal(priceText);
                    }
               }
            }

            MessageBox.Show($"Name: {productName}\nPrice: {productPrice}");
        }
    }
}
