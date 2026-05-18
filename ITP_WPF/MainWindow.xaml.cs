using ITP_WPF;
using Manager_PIU.manager;
using Modele_PIU.enums;
using Modele_PIU.models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Proiect_PIU
{
    public partial class MainWindow : Window
    {
        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();

        public MainWindow()
        {
            InitializeComponent();
            AfiseazaMasina();
        }

        // ===================== AFISARE DETALII =====================
        private void AfiseazaMasina()
        {
            Proprietar proprietar = new Proprietar("Popescu Ion", "1850312374521", "0721 123 456");
            Inspector inspector = new Inspector("Ionescu Mihai", "ITP-2024-007");
            Masina masina = new Masina("Dacia", "Logan", 2019, "SV-01-XYZ", proprietar, Culoare.Alb, Optiuni.AerConditionat | Optiuni.Navigatie);

            InspectieITP inspectie = new InspectieITP(new DateTime(2024, 3, 15), Rezultat.Admis, inspector, "");
            masina.IstoricInspectii.Add(inspectie);

            lblNrInmatriculare.Content = masina.NrInmatriculare;
            lblMarcaModel.Content = masina.Marca + " " + masina.Model;
            lblAnFabricatie.Content = masina.AnFabricatie.ToString();
            lblCuloare.Content = masina.CuloareMasina.ToString();
            lblDotari.Content = masina.Dotari == Optiuni.Niciuna ? "Standard" : masina.Dotari.ToString();

            lblProprietarNume.Content = masina.Proprietar.Nume;
            lblProprietarCNP.Content = masina.Proprietar.CNP;
            lblProprietarTelefon.Content = masina.Proprietar.Telefon;

            if (masina.IstoricInspectii.Count > 0)
            {
                InspectieITP ultima = masina.IstoricInspectii[masina.IstoricInspectii.Count - 1];
                lblDataInspectie.Content = ultima.DataEfectuare.ToString("dd.MM.yyyy");
                lblInspector.Content = ultima.InspectorCareAAprobat.Nume;

                if (ultima.Rezultat == Rezultat.Admis)
                {
                    lblRezultat.Content = "ADMIS";
                    lblRezultat.Foreground = new SolidColorBrush(Color.FromRgb(0x1E, 0x84, 0x49));
                    badgeRezultat.Background = new SolidColorBrush(Color.FromRgb(0xD5, 0xF5, 0xE3));
                    lblValabilPana.Content = ultima.DataExpirare.ToString("dd.MM.yyyy");
                    lblValabilPana.Foreground = new SolidColorBrush(Color.FromRgb(0x27, 0xAE, 0x60));
                }
                else
                {
                    lblRezultat.Content = "RESPINS";
                    lblRezultat.Foreground = new SolidColorBrush(Color.FromRgb(0x92, 0x2B, 0x21));
                    badgeRezultat.Background = new SolidColorBrush(Color.FromRgb(0xFA, 0xDB, 0xD8));
                    lblValabilPana.Content = "-";

                    if (!string.IsNullOrEmpty(ultima.Defectiuni))
                    {
                        lblDefectiuni.Content = ultima.Defectiuni;
                        panelDefectiuni.Visibility = Visibility.Visible;
                    }
                }

                VerificaExpirare(ultima.DataExpirare, ultima.Rezultat);
            }
        }

        private void VerificaExpirare(DateTime dataExpirare, Rezultat rezultat)
        {
            if (rezultat == Rezultat.Respins)
            {
                lblAvertizare.Content = "Masina a fost respinsa la ultima inspectie ITP!";
                panelAvertizare.Visibility = Visibility.Visible;
                return;
            }

            int zile = (dataExpirare - DateTime.Today).Days;
            if (zile < 0)
            {
                lblAvertizare.Content = "ITP-ul este expirat din " + dataExpirare.ToString("dd.MM.yyyy") + "!";
                panelAvertizare.Visibility = Visibility.Visible;
                lblValabilPana.Foreground = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C));
            }
            else if (zile <= 30)
            {
                lblAvertizare.Content = "ITP-ul expira in " + zile + " zile. Programati reinspectia!";
                panelAvertizare.Visibility = Visibility.Visible;
                lblValabilPana.Foreground = new SolidColorBrush(Color.FromRgb(0xF3, 0x9C, 0x12));
            }
        }

        // ===================== NAVIGARE MENIU =====================
        private void BtnMeniuDetalii_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanel("detalii");
        }

        private void BtnMeniuAdauga_Click(object sender, RoutedEventArgs e)
        {
            new AdaugaMasinaWindow().ShowDialog();
        }

        private void BtnMeniuCauta_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanel("cauta");
            txtCautare.Focus();
        }

        private void AfiseazaPanel(string panel)
        {
            panelDetalii.Visibility = panel == "detalii" ? Visibility.Visible : Visibility.Collapsed;
            panelCauta.Visibility = panel == "cauta" ? Visibility.Visible : Visibility.Collapsed;

            btnMeniuDetalii.Style = panel == "detalii"
                ? (Style)FindResource("BtnMeniuActiv") : (Style)FindResource("BtnMeniu");
            btnMeniuCauta.Style = panel == "cauta"
                ? (Style)FindResource("BtnMeniuActiv") : (Style)FindResource("BtnMeniu");
            btnMeniuAdauga.Style = (Style)FindResource("BtnMeniu");
        }

        // ===================== CAUTARE =====================
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
                lblMesajCautare.Content = "Introduceți un număr de înmatriculare.";
                lblMesajCautare.Visibility = Visibility.Visible;
                dgRezultate.Visibility = Visibility.Collapsed;
                return;
            }

            Masina gasita = _manager.CautaDupaNrInmatriculare(termen);

            if (gasita != null)
            {
                dgRezultate.ItemsSource = new List<Masina> { gasita };
                dgRezultate.Visibility = Visibility.Visible;
                lblMesajCautare.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblMesajCautare.Content = "Nu a fost găsită nicio mașină cu numărul \"" + termen + "\".";
                lblMesajCautare.Visibility = Visibility.Visible;
                dgRezultate.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnMeniuModifica_Click(object sender, RoutedEventArgs e)
        {
            new ModificaMasinaWindow().ShowDialog();
        }
        private void BtnMeniuInspectii_Click(object sender, RoutedEventArgs e)
        {
            new GestionareInspectiiWindow().ShowDialog();
        }
    }
}
