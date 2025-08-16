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
    public partial class ReceiptForm : Form
    {
        public ReceiptForm(String receiptText)
        {
            InitializeComponent();
            rtbReceipt.Text = receiptText;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
