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
        // Constante pentru limite de validare
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

            int codEroare = ValideazaDateMasina();
            if (codEroare != 0)
                return;

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
                Culoare.Alb,
                Optiuni.Niciuna
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

        // ===================== VALIDARE ====================
        private int ValideazaDateMasina()
        {
            int codEroare = 0;

            // Resetam toate erorile vizuale
            ResetErori();

            // Marca
            if (string.IsNullOrWhiteSpace(txtMarca.Text) || txtMarca.Text.Trim().Length > MAX_LUNGIME_MARCA)
            {
                SetEroare(lblMarca, txtMarca, errMarca);
                codEroare = 1;
            }

            // Model
            if (string.IsNullOrWhiteSpace(txtModel.Text) || txtModel.Text.Trim().Length > MAX_LUNGIME_MODEL)
            {
                SetEroare(lblModel, txtModel, errModel);
                codEroare = 2;
            }

            // An fabricatie
            bool anValid = int.TryParse(txtAn.Text.Trim(), out int an) && an >= AN_MINIM && an <= AN_MAXIM;
            if (!anValid)
            {
                SetEroare(lblAn, txtAn, errAn);
                codEroare = 3;
            }

            // Nr inmatriculare (ex: SV-01-XYZ, B-123-ABC)
            string nr = txtNr.Text.Trim().ToUpper();
            bool nrValid = !string.IsNullOrWhiteSpace(nr) &&
                           Regex.IsMatch(nr, @"^[A-Z]{1,2}-\d{2,3}-[A-Z]{3}$");
            if (!nrValid)
            {
                SetEroare(lblNr, txtNr, errNr);
                codEroare = 4;
            }

            // Nume proprietar
            if (string.IsNullOrWhiteSpace(txtNumeProprietar.Text) || txtNumeProprietar.Text.Trim().Length > MAX_LUNGIME_NUME)
            {
                SetEroare(lblNumeProprietar, txtNumeProprietar, errNumeProprietar);
                codEroare = 5;
            }

            // CNP (13 cifre)
            bool cnpValid = Regex.IsMatch(txtCNP.Text.Trim(), @"^\d{13}$");
            if (!cnpValid)
            {
                SetEroare(lblCNP, txtCNP, errCNP);
                codEroare = 6;
            }

            // Telefon (10 cifre)
            bool telefonValid = Regex.IsMatch(txtTelefon.Text.Trim(), @"^\d{10}$");
            if (!telefonValid)
            {
                SetEroare(lblTelefon, txtTelefon, errTelefon);
                codEroare = 7;
            }

            return codEroare;
        }

        // ===================== HELPERS =====================
        private void SetEroare(Label label, TextBox textBox, TextBlock mesajEroare)
        {
            label.Foreground = _culoareEroare;
            textBox.BorderBrush = _culoareEroare;
            textBox.BorderThickness = new Thickness(2);
            mesajEroare.Visibility = Visibility.Visible;
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

        private void ResetCamp(Label label, TextBox textBox, TextBlock mesajEroare)
        {
            label.Foreground = _culoareNormala;
            textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xBD, 0xC3, 0xC7));
            textBox.BorderThickness = new Thickness(1);
            mesajEroare.Visibility = Visibility.Collapsed;
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
            ResetErori();
            txtMarca.Focus();
        }
    }
}