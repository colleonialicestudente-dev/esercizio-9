using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Esercizio9
{
    public class MainForm : Form
    {
        private readonly List<Product> _products = new List<Product>();

        private TextBox txtName;
        private TextBox txtPrice;
        private Button btnCreate;
        private Button btnRead;
        private Button btnUpdate;
        private Button btnDelete;
        private ListView listViewProducts;
        private ColumnHeader colName;
        private ColumnHeader colPrice;
        private Label lblName;
        private Label lblPrice;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtName = new TextBox { Left = 16, Top = 28, Width = 200, Name = "txtName" };
            lblName = new Label { Left = 16, Top = 8, Text = "Nome:", AutoSize = true };

            txtPrice = new TextBox { Left = 230, Top = 28, Width = 100, Name = "txtPrice" };
            lblPrice = new Label { Left = 230, Top = 8, Text = "Prezzo:", AutoSize = true };

            btnCreate = new Button { Left = 16, Top = 64, Width = 100, Text = "Crea", Name = "btnCreate" };
            btnRead = new Button { Left = 122, Top = 64, Width = 100, Text = "Visualizza", Name = "btnRead" };
            btnUpdate = new Button { Left = 230, Top = 64, Width = 100, Text = "Modifica", Name = "btnUpdate" };
            btnDelete = new Button { Left = 336, Top = 64, Width = 100, Text = "Elimina", Name = "btnDelete" };

            listViewProducts = new ListView
            {
                Left = 16,
                Top = 100,
                Width = 420,
                Height = 240,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Name = "listViewProducts"
            };

            colName = new ColumnHeader { Text = "Nome", Width = 260 };
            colPrice = new ColumnHeader { Text = "Prezzo", Width = 140 };

            listViewProducts.Columns.AddRange(new[] { colName, colPrice });

            btnCreate.Click += BtnCreate_Click;
            btnRead.Click += BtnRead_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            listViewProducts.DoubleClick += ListViewProducts_DoubleClick;

            ClientSize = new System.Drawing.Size(456, 360);
            Controls.AddRange(new Control[] { lblName, txtName, lblPrice, txtPrice, btnCreate, btnRead, btnUpdate, btnDelete, listViewProducts });
            Text = "Gestione Prodotti - CRUD";
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (!TryReadInput(out string name, out decimal price))
                return;

            if (_products.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Esiste già un prodotto con lo stesso nome.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _products.Add(new Product(name, price));
            ClearInputs();
            RefreshListView();
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!TryReadInput(out string name, out decimal price))
                return;

            var product = _products.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
            if (product == null)
            {
                MessageBox.Show("Prodotto non trovato.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            product.Price = price;
            ClearInputs();
            RefreshListView();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Inserire il nome del prodotto da eliminare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = _products.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
            if (product == null)
            {
                MessageBox.Show("Prodotto non trovato.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"Eliminare il prodotto \"{product.Name}\"?", "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _products.Remove(product);
                ClearInputs();
                RefreshListView();
            }
        }

        private void ListViewProducts_DoubleClick(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0)
                return;

            var item = listViewProducts.SelectedItems[0];
            txtName.Text = item.SubItems[0].Text;
            txtPrice.Text = item.SubItems[1].Text.Replace("€", "").Trim();
        }

        private bool TryReadInput(out string name, out decimal price)
        {
            name = txtName.Text.Trim();
            price = 0m;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Inserire il nome del prodotto.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.CurrentCulture, out price))
            {
                // try invariant culture (comma/point differences)
                if (!decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out price))
                {
                    MessageBox.Show("Prezzo non valido. Usa formato numerico (es. 12.50 o 12,50).", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (price < 0)
            {
                MessageBox.Show("Il prezzo non può essere negativo.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void RefreshListView()
        {
            listViewProducts.Items.Clear();
            foreach (var p in _products)
            {
                var item = new ListViewItem(p.Name);
                item.SubItems.Add(p.Price.ToString("C"));
                listViewProducts.Items.Add(item);
            }
        }

        private void ClearInputs()
        {
            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtName.Focus();
        }
    }
}