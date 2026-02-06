using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace GestioneProdotti
{
    public partial class Form1 : Form
    {
        // Lista che contiene tutti i prodotti
        List<Prodotto> prodotti = new List<Prodotto>();

        public Form1()
        {
            InitializeComponent();
        }

        // CREATE - Aggiunta prodotto
        private void btnAggiungi_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            decimal prezzo;

            if (nome == "" || !decimal.TryParse(txtPrezzo.Text, out prezzo))
            {
                MessageBox.Show("Inserisci nome e prezzo validi");
                return;
            }

            prodotti.Add(new Prodotto(nome, prezzo));
            MessageBox.Show("Prodotto aggiunto!");

            txtNome.Clear();
            txtPrezzo.Clear();
        }

        // READ - Visualizzazione prodotti
        private void btnVisualizza_Click(object sender, EventArgs e)
        {
            lstProdotti.Items.Clear();

            foreach (Prodotto p in prodotti)
            {
                lstProdotti.Items.Add(p);
            }
        }

        // UPDATE - Modifica prodotto tramite nome
        private void btnModifica_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            decimal nuovoPrezzo;

            if (!decimal.TryParse(txtPrezzo.Text, out nuovoPrezzo))
            {
                MessageBox.Show("Prezzo non valido");
                return;
            }

            Prodotto prodotto = prodotti.FirstOrDefault(p => p.Nome == nome);

            if (prodotto != null)
            {
                prodotto.Prezzo = nuovoPrezzo;
                MessageBox.Show("Prodotto modificato!");
            }
            else
            {
                MessageBox.Show("Prodotto non trovato");
            }
        }

        // DELETE - Eliminazione prodotto tramite nome
        private void btnElimina_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;

            Prodotto prodotto = prodotti.FirstOrDefault(p => p.Nome == nome);

            if (prodotto != null)
            {
                prodotti.Remove(prodotto);
                MessageBox.Show("Prodotto eliminato!");
            }
            else
            {
                MessageBox.Show("Prodotto non trovato");
            }
        }
    }
}
