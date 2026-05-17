using System;
using System.Windows;
using System.Windows.Media;
using Modele_PIU.models;
using Modele_PIU.enums;
//lab 6 - implementare UI WPF pentru afisarea detaliata a unei masini, inclusiv istoricul inspectiilor ITP, cu evidentierea rezultatelor si avertizari pentru ITP expirat sau respins.
//tema
namespace Proiect_PIU
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfiseazaMasina();
        }

        private void AfiseazaMasina()
        {
            Proprietar proprietar = new Proprietar("Popescu Ion", "1850312374521", "0721 123 456");

            Inspector inspector = new Inspector("Ionescu Mihai", "ITP-2024-007");

            Masina masina = new Masina("Dacia", "Logan", 2019, "SV-01-XYZ", proprietar, Culoare.Alb, Optiuni.Niciuna);

            InspectieITP inspectie = new InspectieITP(new DateTime(2024, 3, 15), Rezultat.Admis, inspector, "");

            masina.IstoricInspectii.Add(inspectie);

            lblNrInmatriculare.Content = masina.NrInmatriculare;
            lblMarcaModel.Content = masina.Marca + " " + masina.Model;
            lblAnFabricatie.Content = masina.AnFabricatie.ToString();
            lblCuloare.Content = masina.CuloareMasina.ToString();

            if (masina.Dotari == Optiuni.Niciuna)
                lblDotari.Content = "Standard";
            else
                lblDotari.Content = masina.Dotari.ToString();

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
                lblAvertizare.Content = "ITP-ul expira in " + zile + " zile (" + dataExpirare.ToString("dd.MM.yyyy") + "). Programati reinspectia!";
                panelAvertizare.Visibility = Visibility.Visible;
                lblValabilPana.Foreground = new SolidColorBrush(Color.FromRgb(0xF3, 0x9C, 0x12));
            }
        }

        private void BtnAdaugaMasina_Click(object sender, RoutedEventArgs e)
        {
            new AdaugaMasinaWindow().ShowDialog();
        }
    }
}