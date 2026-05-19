using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Manager_PIU.manager;
using Modele_PIU.models;

namespace Proiect_PIU
{
    public partial class MainWindow : Window
    {
        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();

        public MainWindow()
        {
            InitializeComponent();
            IncarcaMasini();
        }

        // ===== INCARCARE DATE REALE DIN FISIER =====
        private void IncarcaMasini()
        {
            List<Masina> masini = _manager.GetToateMasinile();
            dgMasini.ItemsSource = null;
            dgMasini.ItemsSource = masini;
            lblStatus.Content = "Total masini incarcate: " + masini.Count;
        }

        // ===== NAVIGARE MENIU =====
        private void BtnMeniuLista_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanel("lista");
            IncarcaMasini();
        }

        private void BtnMeniuAdauga_Click(object sender, RoutedEventArgs e)
        {
            new AdaugaMasinaWindow().ShowDialog();
            IncarcaMasini(); // reincarca dupa adaugare
        }

        private void BtnMeniuInspectii_Click(object sender, RoutedEventArgs e)
        {
            new GestionareInspectiiWindow().ShowDialog();
            IncarcaMasini();
        }

        private void BtnMeniuModifica_Click(object sender, RoutedEventArgs e)
        {
            new ModificaMasinaWindow().ShowDialog();
            IncarcaMasini();
        }

        private void BtnReincarca_Click(object sender, RoutedEventArgs e)
        {
            IncarcaMasini();
        }

        private void BtnMeniuCauta_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanel("cauta");
            txtCautare.Focus();
        }

        // ===== CAUTARE =====
        // In MainWindow.xaml.cs, inlocuieste metodele de cautare cu astea:

        private void TxtCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cauta live daca ai minim 2 caractere
            if (txtCautare.Text.Trim().Length >= 2)
                Cauta();
            else if (string.IsNullOrWhiteSpace(txtCautare.Text))
            {
                dgRezultate.Visibility = Visibility.Collapsed;
                lblMesajCautare.Visibility = Visibility.Visible;
                lblMesajCautare.Content = "Introduceti un termen de cautare.";
                lblRezultateHeader.Content = "REZULTATE";
            }
        }

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            Cauta();
        }

        private void TxtCautare_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Cauta();
        }

        private void Cauta()
        {
            string termen = txtCautare.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(termen))
            {
                lblMesajCautare.Content = "Introduceti un termen de cautare.";
                lblMesajCautare.Visibility = Visibility.Visible;
                dgRezultate.Visibility = Visibility.Collapsed;
                return;
            }

            List<Masina> toateMasinile = _manager.GetToateMasinile();
            List<Masina> rezultate = new List<Masina>();

            // Criteriu selectat din ComboBox
            int criteriu = cmbCriteriuCautare.SelectedIndex;

            foreach (Masina m in toateMasinile)
            {
                bool gasit = false;

                switch (criteriu)
                {
                    case 0: // Nr. Inmatriculare
                        gasit = m.NrInmatriculare.ToUpper().Contains(termen);
                        break;
                    case 1: // Marca
                        gasit = m.Marca.ToUpper().Contains(termen);
                        break;
                    case 2: // Model
                        gasit = m.Model.ToUpper().Contains(termen);
                        break;
                    case 3: // Proprietar
                        gasit = m.Proprietar != null &&
                                m.Proprietar.Nume.ToUpper().Contains(termen);
                        break;
                }

                if (gasit)
                    rezultate.Add(m);
            }

            if (rezultate.Count > 0)
            {
                dgRezultate.ItemsSource = rezultate;
                dgRezultate.Visibility = Visibility.Visible;
                lblMesajCautare.Visibility = Visibility.Collapsed;
                lblRezultateHeader.Content = "REZULTATE (" + rezultate.Count + " masini gasite)";
                lblStatus.Content = "Gasite " + rezultate.Count + " masini.";
            }
            else
            {
                lblMesajCautare.Content = "Nu a fost gasita nicio masina pentru \"" + txtCautare.Text.Trim() + "\".";
                lblMesajCautare.Visibility = Visibility.Visible;
                dgRezultate.Visibility = Visibility.Collapsed;
                lblRezultateHeader.Content = "REZULTATE";
                lblStatus.Content = "Niciun rezultat.";
            }
        }



        // ===== HELPER NAVIGARE =====
        private void AfiseazaPanel(string panel)
        {
            panelLista.Visibility = panel == "lista" ? Visibility.Visible : Visibility.Collapsed;
            panelCauta.Visibility = panel == "cauta" ? Visibility.Visible : Visibility.Collapsed;

            btnMeniuLista.Style = panel == "lista" ? (Style)FindResource("BtnMeniuActiv") : (Style)FindResource("BtnMeniu");
            btnMeniuCauta.Style = panel == "cauta" ? (Style)FindResource("BtnMeniuActiv") : (Style)FindResource("BtnMeniu");
            btnMeniuAdauga.Style = (Style)FindResource("BtnMeniu");
            btnMeniuInspectii.Style = (Style)FindResource("BtnMeniu");
            btnMeniuModifica.Style = (Style)FindResource("BtnMeniu");
        }
    }
}
