using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Manager_PIU.manager;
using Modele_PIU.enums;
using Modele_PIU.models;

namespace Proiect_PIU
{
    public partial class ModificaMasinaWindow : Window
    {
        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();
        private Masina _masinaSelectata = null;

        public ModificaMasinaWindow()
        {
            InitializeComponent();
            IncarcaMasini();
        }

        // ===================== INCARCARE COMBOBOX =====================
        private void IncarcaMasini()
        {
            List<Masina> masini = _manager.GetToateMasinile();

            if (masini.Count == 0)
            {
                lblNiciuMasina.Visibility = Visibility.Visible;
                cmbMasini.IsEnabled = false;
                return;
            }

            cmbMasini.ItemsSource = null;
            cmbMasini.ItemsSource = masini;
        }

        //SELECTIE MASINA (ComboBox) 
        private void CmbMasini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _masinaSelectata = cmbMasini.SelectedItem as Masina;
            if (_masinaSelectata == null) return;

            //Afisam panelurile
            panelEditare.Visibility = Visibility.Visible;
            panelProprietar.Visibility = Visibility.Visible;
            panelIstoric.Visibility = Visibility.Visible;
            btnActualizeaza.IsEnabled = true;
            panelSucces.Visibility = Visibility.Collapsed;

            //Populam campurile
            txtMarca.Text = _masinaSelectata.Marca;
            txtModel.Text = _masinaSelectata.Model;
            txtAn.Text = _masinaSelectata.AnFabricatie.ToString();
            txtNr.Text = _masinaSelectata.NrInmatriculare;

            //Proprietar
            if (_masinaSelectata.Proprietar != null)
            {
                txtNumeProprietar.Text = _masinaSelectata.Proprietar.Nume;
                txtCNP.Text = _masinaSelectata.Proprietar.CNP;
                txtTelefon.Text = _masinaSelectata.Proprietar.Telefon;
            }

            //Culoare (RadioButton)
            SetCuloare(_masinaSelectata.CuloareMasina);

            //Dotari (CheckBox)
            ckbAer.IsChecked = _masinaSelectata.Dotari.HasFlag(Optiuni.AerConditionat);
            ckbNavigatie.IsChecked = _masinaSelectata.Dotari.HasFlag(Optiuni.Navigatie);
            ckbCutie.IsChecked = _masinaSelectata.Dotari.HasFlag(Optiuni.CutieAutomata);
            ckbSenzori.IsChecked = _masinaSelectata.Dotari.HasFlag(Optiuni.SenzoriParcare);

            //Istoric inspectii (ListBox)
            IncarcaIstoric(_masinaSelectata);

            //DatePicker, data curenta implicit
            dtpDataInspectie.SelectedDate = DateTime.Today;
        }

        //listbox istoric
        private void IncarcaIstoric(Masina masina)
        {
            lstIstoric.Items.Clear();

            if (masina.IstoricInspectii == null || masina.IstoricInspectii.Count == 0)
            {
                lstIstoric.Items.Add("Nicio inspecție înregistrată.");
                return;
            }

            foreach (InspectieITP inspectie in masina.IstoricInspectii)
            {
                string rezultat = inspectie.Rezultat == Rezultat.Admis ? "ADMIS" : "RESPINS";
                lstIstoric.Items.Add(
                    inspectie.DataEfectuare.ToString("dd.MM.yyyy") +
                    " — " + rezultat +
                    " (exp: " + inspectie.DataExpirare.ToString("dd.MM.yyyy") + ")"
                );
            }
        }

        //buton care actualizeaza
        private void BtnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (_masinaSelectata == null) return;

            // Preluam valorile modificate
            _masinaSelectata.Marca = txtMarca.Text.Trim();
            _masinaSelectata.Model = txtModel.Text.Trim();

            if (int.TryParse(txtAn.Text.Trim(), out int an))
                _masinaSelectata.AnFabricatie = an;

            _masinaSelectata.CuloareMasina = GetCuloareSelectata();
            _masinaSelectata.Dotari = GetDotariSelectate();

            if (_masinaSelectata.Proprietar != null)
            {
                _masinaSelectata.Proprietar.Nume = txtNumeProprietar.Text.Trim();
                _masinaSelectata.Proprietar.CNP = txtCNP.Text.Trim();
                _masinaSelectata.Proprietar.Telefon = txtTelefon.Text.Trim();
            }

            //Adaugam noua inspectie daca s-a ales o data
            if (dtpDataInspectie.SelectedDate.HasValue)
            {
                DateTime dataInspectie = dtpDataInspectie.SelectedDate ?? DateTime.Today;
                Rezultat rezultat = (cmbRezultat.SelectedIndex == 0) ? Rezultat.Admis : Rezultat.Respins;
                Inspector inspector = new Inspector("Inspector", "N/A");
                InspectieITP nouaInspectie = new InspectieITP(dataInspectie, rezultat, inspector, "");
                _masinaSelectata.IstoricInspectii.Add(nouaInspectie);
            }

            //salvam modif in fis
            ModificaMasinaInFisier(_masinaSelectata);

            panelSucces.Visibility = Visibility.Visible;
            IncarcaIstoric(_masinaSelectata);
            IncarcaMasini();
        }

        //modif in fisier
        private void ModificaMasinaInFisier(Masina masinaModificata)
        {
            List<Masina> toateMasinile = _manager.GetToateMasinile();

            for (int i = 0; i < toateMasinile.Count; i++)
            {
                if (toateMasinile[i].NrInmatriculare.ToUpper() ==
                    masinaModificata.NrInmatriculare.ToUpper())
                {
                    toateMasinile[i] = masinaModificata;
                    break;
                }
            }

            //rescrie fisierul
            using (StreamWriter sw = new StreamWriter("Masini.txt", false))
            {
                foreach (Masina m in toateMasinile)
                {
                    sw.WriteLine(m.ConversieLaSirPentruFisier());
                }
            }
        }

        //butonul anuleaza
        private void BtnAnuleaza_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //helpers
        private Culoare GetCuloareSelectata()
        {
            if (rbRosu.IsChecked == true) return Culoare.Rosu;
            if (rbNegru.IsChecked == true) return Culoare.Negru;
            if (rbAlbastru.IsChecked == true) return Culoare.Albastru;
            if (rbGri.IsChecked == true) return Culoare.Gri;
            return Culoare.Alb;
        }

        private Optiuni GetDotariSelectate()
        {
            Optiuni dotari = Optiuni.Niciuna;
            if (ckbAer.IsChecked == true) dotari |= Optiuni.AerConditionat;
            if (ckbNavigatie.IsChecked == true) dotari |= Optiuni.Navigatie;
            if (ckbCutie.IsChecked == true) dotari |= Optiuni.CutieAutomata;
            if (ckbSenzori.IsChecked == true) dotari |= Optiuni.SenzoriParcare;
            return dotari;
        }

        private void SetCuloare(Culoare culoare)
        {
            rbRosu.IsChecked = culoare == Culoare.Rosu;
            rbAlb.IsChecked = culoare == Culoare.Alb;
            rbNegru.IsChecked = culoare == Culoare.Negru;
            rbAlbastru.IsChecked = culoare == Culoare.Albastru;
            rbGri.IsChecked = culoare == Culoare.Gri;
        }
    }
}
