using System;
using System.IO;
using System.Windows.Forms;

namespace ImtiajSuperStore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            xGrid.Columns.Add("A", "Item-Name");
            xGrid.Columns.Add("B", "UPrice");
            xGrid.Columns.Add("C", "Qty");
            xGrid.Columns.Add("D", "Total");
            xGrid.Columns[0].Width = 200;
            xGrid.Columns[1].Width = 50;
            xGrid.Columns[2].Width = 50;
            xGrid.Columns[3].Width = 50;
            GetProducts();
        }
        private void Calc_Bill()
        {
            int Sum = 0;
            for (int i = 0; i < xGrid.Rows.Count-1; i++)
            {
                Sum = Sum + int.Parse(xGrid.Rows[i].Cells[3].Value.ToString());
            }
            lblBill.Text = Sum.ToString();
        }
        private void GetProducts()
        {
            AutoCompleteStringCollection Arr = new AutoCompleteStringCollection();
            StreamReader SR = new StreamReader("Products.txt");
            while (!SR.EndOfStream)
            {
                Arr.Add(SR.ReadLine());
            }
            SR.Close();
            txtSearch.AutoCompleteCustomSource = Arr;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] A = txtSearch.Text.Split('#');
                lblName.Text = A[0];
                lblPrice.Text = A[1];
                txtQty.Focus();
            }
        }
        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int P = int.Parse(lblPrice.Text);
                int Q = int.Parse(txtQty.Text);
                int T = P * Q;
                xGrid.Rows.Add(lblName.Text, lblPrice.Text, txtQty.Text, T);
                Calc_Bill();
                txtSearch.Clear();
                txtSearch.Focus();
            }
        }
        private void xGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult DR= MessageBox.Show("Are you sure to delete item?","Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (DR == DialogResult.Yes)
            {
                int i = xGrid.CurrentRow.Index;
                xGrid.Rows.RemoveAt(i);
                Calc_Bill();            
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (xGrid.Rows.Count > 1)
            {               
                string Invoice = DateTime.Now.ToString("ddMMMyyyyHHmmss");
                StreamWriter SW = new StreamWriter(Invoice+".txt");
                int Col = xGrid.Columns.Count;
                int Row = xGrid.Rows.Count - 1;
                for (int R = 0; R < Row; R++)
                {
                    for (int C = 0; C < Col; C++)
                    {
                        SW.Write(xGrid.Rows[R].Cells[C].Value.ToString()+"\t");
                    }
                    SW.WriteLine("\r\n---------------------------------------");
                }
                SW.WriteLine("Total Bill: " + lblBill.Text);
                SW.Flush();
                SW.Close();
                xGrid.Rows.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
