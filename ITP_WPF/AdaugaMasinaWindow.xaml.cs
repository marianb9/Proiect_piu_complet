using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Manager_PIU.manager;
using Modele_PIU.enums;
using Modele_PIU.models;

namespace Proiect_PIU
{
    public partial class AdaugaMasinaWindow : Window
    {
        private const int MAX_LUNGIME_MARCA = 30;
        private const int MAX_LUNGIME_MODEL = 30;
        private const int MAX_LUNGIME_NUME = 50;
        private const int LUNGIME_CNP = 13;
        private const int LUNGIME_TELEFON = 10;
        private const int AN_MINIM = 1900;
        private const int AN_MAXIM = 2025;

        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();

        private readonly SolidColorBrush _culoareNormala = new SolidColorBrush(Color.FromRgb(0x2C, 0x3E, 0x50));
        private readonly SolidColorBrush _culoareEroare = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C));

        public AdaugaMasinaWindow()
        {
            InitializeComponent();
        }

        // ===================== BUTON ADAUGA =====================
        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            panelSucces.Visibility = Visibility.Collapsed;

            if (ValideazaDateMasina() != 0)
                return;

            Culoare culoareSelectata = GetCuloareSelectata();
            Optiuni dotariSelectate = GetDotariSelectate();

            Proprietar proprietar = new Proprietar(
                txtNumeProprietar.Text.Trim(),
                txtCNP.Text.Trim(),
                txtTelefon.Text.Trim()
            );

            Masina masina = new Masina(
                txtMarca.Text.Trim(),
                txtModel.Text.Trim(),
                int.Parse(txtAn.Text.Trim()),
                txtNr.Text.Trim().ToUpper(),
                proprietar,
                culoareSelectata,
                dotariSelectate
            );

            _manager.AdaugaMasina(masina);
            panelSucces.Visibility = Visibility.Visible;
            ResetFormular();
        }

        // ===================== BUTON RESET =====================
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
            panelSucces.Visibility = Visibility.Collapsed;
        }

        // ===================== PRELUARE CULOARE (RadioButton) =====================
        private Culoare GetCuloareSelectata()
        {
            if (rbRosu.IsChecked == true) return Culoare.Rosu;
            if (rbNegru.IsChecked == true) return Culoare.Negru;
            if (rbAlbastru.IsChecked == true) return Culoare.Albastru;
            if (rbGri.IsChecked == true) return Culoare.Gri;
            return Culoare.Alb; // implicit
        }

        // ===================== PRELUARE DOTARI (CheckBox - Flags enum) =====================
        private Optiuni GetDotariSelectate()
        {
            Optiuni dotari = Optiuni.Niciuna;
            if (ckbAer.IsChecked == true) dotari |= Optiuni.AerConditionat;
            if (ckbNavigatie.IsChecked == true) dotari |= Optiuni.Navigatie;
            if (ckbCutie.IsChecked == true) dotari |= Optiuni.CutieAutomata;
            if (ckbSenzori.IsChecked == true) dotari |= Optiuni.SenzoriParcare;
            return dotari;
        }

        // ===================== VALIDARE =====================
        private int ValideazaDateMasina()
        {
            int codEroare = 0;
            ResetErori();

            if (string.IsNullOrWhiteSpace(txtMarca.Text) || txtMarca.Text.Trim().Length > MAX_LUNGIME_MARCA)
            { SetEroare(lblMarca, txtMarca, errMarca); codEroare = 1; }

            if (string.IsNullOrWhiteSpace(txtModel.Text) || txtModel.Text.Trim().Length > MAX_LUNGIME_MODEL)
            { SetEroare(lblModel, txtModel, errModel); codEroare = 2; }

            bool anValid = int.TryParse(txtAn.Text.Trim(), out int an) && an >= AN_MINIM && an <= AN_MAXIM;
            if (!anValid)
            { SetEroare(lblAn, txtAn, errAn); codEroare = 3; }

            string nr = txtNr.Text.Trim().ToUpper();
            if (!Regex.IsMatch(nr, @"^[A-Z]{1,2}-\d{2,3}-[A-Z]{3}$"))
            { SetEroare(lblNr, txtNr, errNr); codEroare = 4; }

            if (string.IsNullOrWhiteSpace(txtNumeProprietar.Text) || txtNumeProprietar.Text.Trim().Length > MAX_LUNGIME_NUME)
            { SetEroare(lblNumeProprietar, txtNumeProprietar, errNumeProprietar); codEroare = 5; }

            if (!Regex.IsMatch(txtCNP.Text.Trim(), @"^\d{13}$"))
            { SetEroare(lblCNP, txtCNP, errCNP); codEroare = 6; }

            if (!Regex.IsMatch(txtTelefon.Text.Trim(), @"^\d{10}$"))
            { SetEroare(lblTelefon, txtTelefon, errTelefon); codEroare = 7; }

            return codEroare;
        }

        // ===================== HELPERS =====================
        private void SetEroare(Label label, TextBox textBox, TextBlock mesaj)
        {
            label.Foreground = _culoareEroare;
            textBox.BorderBrush = _culoareEroare;
            textBox.BorderThickness = new Thickness(2);
            mesaj.Visibility = Visibility.Visible;
        }

        private void ResetErori()
        {
            ResetCamp(lblMarca, txtMarca, errMarca);
            ResetCamp(lblModel, txtModel, errModel);
            ResetCamp(lblAn, txtAn, errAn);
            ResetCamp(lblNr, txtNr, errNr);
            ResetCamp(lblNumeProprietar, txtNumeProprietar, errNumeProprietar);
            ResetCamp(lblCNP, txtCNP, errCNP);
            ResetCamp(lblTelefon, txtTelefon, errTelefon);
        }

        private void ResetCamp(Label label, TextBox textBox, TextBlock mesaj)
        {
            label.Foreground = _culoareNormala;
            textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xBD, 0xC3, 0xC7));
            textBox.BorderThickness = new Thickness(1);
            mesaj.Visibility = Visibility.Collapsed;
        }

        private void ResetFormular()
        {
            txtMarca.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtAn.Text = string.Empty;
            txtNr.Text = string.Empty;
            txtNumeProprietar.Text = string.Empty;
            txtCNP.Text = string.Empty;
            txtTelefon.Text = string.Empty;
            rbAlb.IsChecked = true;
            ckbAer.IsChecked = false;
            ckbNavigatie.IsChecked = false;
            ckbCutie.IsChecked = false;
            ckbSenzori.IsChecked = false;
            ResetErori();
            txtMarca.Focus();
        }
    }
}
