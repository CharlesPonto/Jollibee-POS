using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jollibee_POS
{
    public partial class Form1 : Form
    {
        private List<MenuItem> cart = new List<MenuItem>();

        // Transaction 
        private decimal amountPaid = 0.00m;
        private decimal change = 0.00m;
        private decimal subTotal = 0.00m;

        // Menu 
        private string productName;
        private decimal productPrice;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Attach click events to menu items (labels, images)
            foreach (Control control in menuTableLayout.Controls)
            {
                if (control is Panel menuPanel)
                {
                    menuPanel.Click += order_Click;
                    foreach (Control child in menuPanel.Controls)
                    {
                        child.Click += order_Click;
                    }
                }
            }
        }

        // Numpad Events
        private void numpadClick(object sender, EventArgs e)
        {
            if (transactionAmountPaid.Text.Contains("₱0.00"))
            {
                transactionAmountPaid.Text = "₱";
            }

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                transactionAmountPaid.Text += clickedButton.Text;

                string amountText = transactionAmountPaid.Text.Replace("₱", "");
                if (decimal.TryParse(amountText, out decimal amount))
                {
                    amountPaid = amount;
                    transactionAmountPaid.Text = "₱" + amount.ToString("N0");
                }
            }

            calculateTransaction();
        }

        private void numpadClearClick(object sender, EventArgs e)
        {
            amountPaid = 0;
            transactionAmountPaid.Text = "₱0.00";

            calculateTransaction();
        }

        // Order & Cart Handling
        private void order_Click(object sender, EventArgs e)
        {
            // use the parent if clicked (label or image)
            Panel clickedPanel = sender as Panel ?? (sender as Control).Parent as Panel;

            // get name and price
            foreach (Control control in clickedPanel.Controls)
            {
                if (control is Label label)
                {
                    if ((string)label.Tag == "Name")
                        productName = label.Text;

                    if ((string)label.Tag == "Price")
                    {
                        string priceText = label.Text.Replace("₱", "").Trim();
                        productPrice = Convert.ToDecimal(priceText);
                    }
                }
            }

            // Add to cart or increase qty
            var existingItem = cart.FirstOrDefault(item => item.Name == productName);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                cart.Add(new MenuItem { Name = productName, Price = productPrice, Quantity = 1 });
            }

            RefreshCartGrid();
            calculateCartOrders();
        }

        private void RefreshCartGrid()
        {
            cartGrid.Rows.Clear();

            foreach (var item in cart)
            {
                cartGrid.Rows.Add(item.Name, item.Quantity, "₱" + item.Price.ToString("N2"));
            }
            cartGrid.ClearSelection();
        }

        // Cart Edit & Clear
        private void cardGrid_editRowQty(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow clickedRow = cartGrid.Rows[e.RowIndex];

                updateQtyInput.Enabled = true;
                updateQtyBtn.Enabled = true;

                updateQtyInput.Value = Convert.ToInt32(clickedRow.Cells["OrderQty"].Value);
            }
        }
        private void updateQtyBtn_Click(object sender, EventArgs e)
        {
            if (cartGrid.CurrentRow != null)
            {
                UpdateRowQty(cartGrid.CurrentRow);
            }
        }

        public void UpdateRowQty(DataGridViewRow clickedRow)
        {
            int rowIndex = clickedRow.Index;
            int newQty = Convert.ToInt32(updateQtyInput.Value);

            cart[rowIndex].Quantity = newQty;
            clickedRow.Cells["OrderQty"].Value = newQty;

            calculateCartOrders();
            resetUpdateQtyState();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            cartGrid.Rows.Clear();
            cart.Clear();

            calculateCartOrders();
            resetUpdateQtyState();
            resetTransaction();
        }

        // Reset Helpers
        public void resetUpdateQtyState()
        {
            cartGrid.ClearSelection();
            updateQtyInput.Enabled = false;
            updateQtyBtn.Enabled = false;
            updateQtyInput.Value = 0;
        }

        public void resetTransaction()
        {
            subTotal = 0.00m;
            amountPaid = 0.00m;
            change = 0.00m;

            transactionTotal.Text = $"₱{subTotal:N2}";
            transactionChange.Text = $"₱{change:N2}";
            transactionAmountPaid.Text = $"₱{amountPaid:N2}";
        }

        // Calculations
        public void calculateCartOrders()
        {
            subTotal = 0.00m;

            foreach (var item in cart)
            {
                subTotal += item.Total;
            }

            cartTotal.Text = $"₱{subTotal}";
            cartGrid.ClearSelection();

            calculateTransaction();
        }

        public void calculateTransaction()
        {
            transactionTotal.Text = $"₱{subTotal:N2}";

            if (amountPaid > subTotal)
            {
                change = amountPaid - subTotal;
                transactionChange.Text = $"₱{change:N2}";
            }
            else
            {
                change = 0.00m;
                transactionChange.Text = $"₱{change:N2}";
            }
        }

        // Payment & Receipt
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (subTotal > amountPaid)
            {
                decimal shortage = subTotal - amountPaid;
                MessageBox.Show($"Insufficient amount paid.\nCustomer is short by ₱{shortage}",
                                "Payment Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              JOLLIBAI POS RECEIPT");
            sb.AppendLine(" =============================================");
            sb.AppendLine("             Date: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "\n");
            sb.AppendLine(string.Format(" {0,-5}{1,-20}{2,10}{3,10}",
                "Qty", "Item", "Price", "Total"));
            sb.AppendLine(" =============================================");

            foreach (var item in cart)
            {
                sb.AppendLine(string.Format(" {0,-5}{1,-20}{2,10}{3,10}",
                    item.Quantity,
                    item.Name,
                    "₱" + item.Price.ToString("N2"),
                    "₱" + (item.Quantity * item.Price).ToString("N2")));
            }

            sb.AppendLine(" =============================================");
            sb.AppendLine(string.Format(" {0,-30}{1,15}", "Subtotal:", "₱" + subTotal.ToString("N2")));
            sb.AppendLine(string.Format(" {0,-30}{1,15}", "Amount Paid:", "₱" + amountPaid.ToString("N2")));
            sb.AppendLine(string.Format(" {0,-30}{1,15}", "Change:", "₱" + change.ToString("N2")));
            sb.AppendLine(" =============================================");
            sb.AppendLine("\n        THANK YOU FOR DINING WITH JOLLIBAI!");
            sb.AppendLine("\n                OFFICIAL RECEIPT");

            ReceiptForm receiptForm = new ReceiptForm(sb.ToString());
            receiptForm.ShowDialog();

            // Reset after payment
            cartGrid.Rows.Clear();
            cart.Clear();

            calculateCartOrders();
            resetUpdateQtyState();
            resetTransaction();
        }
    }
}
