namespace OOP_Lab1_v._02
{
    public partial class Form1 : Form
    {

        const int border = 100;
        public Cell[,] table = new Cell[border,border];
        public int _COLUMNS = 0;
        public int _ROWS = 0;
        
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < border; i++)
                for (int j = 0; j < border; j++)
                    table[i, j] = new Cell();

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update the balance column whenever the value of any cell changes.
            string temp = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _COLUMNS = 5; _ROWS = 5;
            Cell[,] table = new Cell[border,border];
            char c = 'A';
            string temp = null;
            for (int i = 0; i < _COLUMNS; i++)
            {
                temp += c;
                dataGridView1.Columns.Add(Name, temp);
                c++;
                temp = null;
            }
            for (int i = 0; i < _ROWS; i++)
            {
                dataGridView1.Rows.Add();
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
            dataGridView1.AllowUserToAddRows = false;
        }

        public void addNewValue(string exp, int row, int col)
        {
            table[row, col].cValue = exp;
            //MessageBox.Show("row = " + row.ToString() + " col = " + col.ToString() + " table cv = " + table[row, col].cValue);
            Parser parser = new Parser();
            string rr;
            rr = parser.ParseExpression(exp, table);
            dataGridView1[col, row].Value = rr;
            UpdateCells();
        }

        public void UpdateCells()
        {
            for(int i = 0; i < 100; i ++)
            {
                for(int j = 0; j < 100; j ++)
                {
                    Parser parser = new Parser();
                    if (table[i, j].cValue != null)
                    {
                        string rr = parser.ParseExpression(table[i, j].cValue, table);
                        dataGridView1[j, i].Value = rr;
                    }
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            string temp = (string)textBox1.Text;
            addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("tcv " + table[0, 0].cValue);
        }
    }
}