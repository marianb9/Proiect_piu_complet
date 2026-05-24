using System.Windows;
using System.Windows.Controls;
using Manager_PIU.manager;
using Modele_PIU.enums;
using Modele_PIU.models;

namespace Proiect_PIU
{
    /// <summary>
    /// MVVM — View (code-behind minimal, fara logica de business)
    /// Toata validarea e in MasinaViewModel (IDataErrorInfo)
    /// Toata notificarea e in MasinaViewModel (INotifyPropertyChanged)
    /// </summary>
    public partial class AdaugaMasinaWindow : Window
    {
        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();

        //ViewModel — sursa de date pentru binding
        private MasinaViewModel _vm = new MasinaViewModel();

        public AdaugaMasinaWindow()
        {
            InitializeComponent();
            //Setam DataContext la ViewModel — toate binding-urile din XAML folosesc acest obiect
            this.DataContext = _vm;
        }

        //BUTON ADAUGA 
        //Nu mai facem validare manuala — EsteValid din ViewModel se ocupa
        //Butonul e dezactivat automat cat timp EsteValid = false
        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            //Preluam culoarea si dotarile (nu sunt legate prin binding — raman cu cod)
            Culoare culoare = GetCuloareSelectata();
            Optiuni dotari = GetDotariSelectate();

            Proprietar proprietar = new Proprietar(
                _vm.NumeProprietar.Trim(),
                _vm.CNP.Trim(),
                _vm.Telefon.Trim()
            );

            Masina masina = new Masina(
                _vm.Marca.Trim(),
                _vm.Model.Trim(),
                int.Parse(_vm.An.Trim()),
                _vm.NrInmatriculare.Trim().ToUpper(),
                proprietar,
                culoare,
                dotari
            );

            _manager.AdaugaMasina(masina);
            panelSucces.Visibility = Visibility.Visible;
            ResetFormular();
        }

        //BUTON RESET
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
            panelSucces.Visibility = Visibility.Collapsed;
        }

        //HELPERS
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

        private void ResetFormular()
        {
            //Cream un ViewModel nou — toate proprietatile se reseteaza
            //DataContext se actualizeaza, UI-ul se curata automat prin binding
            _vm = new MasinaViewModel();
            this.DataContext = _vm;

            rbAlb.IsChecked = true;
            ckbAer.IsChecked = false;
            ckbNavigatie.IsChecked = false;
            ckbCutie.IsChecked = false;
            ckbSenzori.IsChecked = false;

            txtMarca.Focus();
        }
    }
}
