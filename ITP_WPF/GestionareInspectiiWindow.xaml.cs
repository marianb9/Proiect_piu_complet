using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Manager_PIU.manager;
using Modele_PIU.enums;
using Modele_PIU.models;

namespace Proiect_PIU
{
    public partial class GestionareInspectiiWindow : Window
    {
        private readonly ManagerMasiniFisier _manager = new ManagerMasiniFisier();

        // ObservableCollection notifica automat DataGrid-ul la Add/Remove
        private ObservableCollection<InspectieViewModel> _inspectii =
            new ObservableCollection<InspectieViewModel>();

        private Masina _masinaSelectata = null;
        private bool _modEditare = false;
        private int _indexEditare = -1;

        public GestionareInspectiiWindow()
        {
            InitializeComponent();
            dgInspectii.ItemsSource = _inspectii;
            IncarcaMasini();
            ResetFormular();
        }

        // READ - incarcare masini in ComboBox
        private void IncarcaMasini()
        {
            List<Masina> masini = _manager.GetToateMasinile();
            cmbMasini.ItemsSource = null;
            cmbMasini.ItemsSource = masini;

            if (masini.Count == 0)
                lblStatus.Content = "Nu exista masini salvate.";
        }

        // READ - selectare masina din ComboBox
        private void CmbMasini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _masinaSelectata = cmbMasini.SelectedItem as Masina;
            if (_masinaSelectata == null) return;

            lblInfoMasina.Content = _masinaSelectata.Marca + " " + _masinaSelectata.Model +
                                    " (" + _masinaSelectata.AnFabricatie + ")";

            _inspectii.Clear();
            if (_masinaSelectata.IstoricInspectii != null)
            {
                foreach (InspectieITP insp in _masinaSelectata.IstoricInspectii)
                    _inspectii.Add(new InspectieViewModel(insp));
            }

            lblStatus.Content = "Masina: " + _masinaSelectata.NrInmatriculare +
                                 " — " + _inspectii.Count + " inspectii.";
            ResetFormular();
        }

        // CREATE / UPDATE - salvare
        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (_masinaSelectata == null)
            {
                lblStatus.Content = "Selectati mai intai o masina!";
                return;
            }

            InspectieViewModel vm = this.DataContext as InspectieViewModel;
            if (vm == null) return;

            vm.Rezultat = cmbRezultat.SelectedIndex == 0 ? Rezultat.Admis : Rezultat.Respins;

            if (_modEditare && _indexEditare >= 0 && _indexEditare < _inspectii.Count)
            {
                // UPDATE
                _inspectii[_indexEditare] = new InspectieViewModel(vm.ToInspectieITP());
                lblStatus.Content = "Inspectie actualizata cu succes.";
            }
            else
            {
                // CREATE - ObservableCollection notifica automat DataGrid-ul
                _inspectii.Add(new InspectieViewModel(vm.ToInspectieITP()));
                lblStatus.Content = "Inspectie adaugata cu succes.";
            }

            SincronizeazaSiSalveaza();
            ResetFormular();
        }

        // UPDATE - populare formular la editare
        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            InspectieViewModel selectata = dgInspectii.SelectedItem as InspectieViewModel;
            if (selectata == null)
            {
                lblStatus.Content = "Selectati o inspectie din lista pentru a o edita.";
                return;
            }

            _modEditare = true;
            _indexEditare = dgInspectii.SelectedIndex;

            InspectieViewModel vmEditare = new InspectieViewModel(selectata.ToInspectieITP());
            this.DataContext = vmEditare;

            cmbRezultat.SelectedIndex = selectata.Rezultat == Rezultat.Admis ? 0 : 1;
            lblTitluFormular.Content = "EDITEAZA INSPECTIE";
            btnSalveaza.Content = "Actualizeaza";
            lblStatus.Content = "Mod editare activ. Modificati campurile si apasati Actualizeaza.";
        }

        // DELETE
        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            InspectieViewModel selectata = dgInspectii.SelectedItem as InspectieViewModel;
            if (selectata == null)
            {
                lblStatus.Content = "Selectati o inspectie din lista pentru a o sterge.";
                return;
            }

            MessageBoxResult confirmare = MessageBox.Show(
                "Stergeti inspectia din " + selectata.DataEfectuare.ToString("dd.MM.yyyy") + "?",
                "Confirmare stergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmare == MessageBoxResult.Yes)
            {
                _inspectii.Remove(selectata);
                SincronizeazaSiSalveaza();
                lblStatus.Content = "Inspectie stearsa.";
            }
        }

        // Activare butoane la selectie in DataGrid
        private void DgInspectii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool areSelectie = dgInspectii.SelectedItem != null;
            btnEditeaza.IsEnabled = areSelectie;
            btnSterge.IsEnabled = areSelectie;
        }

        private void BtnAnuleaza_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
        }

        // Sincronizare model + salvare fisier
        private void SincronizeazaSiSalveaza()
        {
            if (_masinaSelectata == null) return;

            _masinaSelectata.IstoricInspectii.Clear();
            foreach (InspectieViewModel vm in _inspectii)
                _masinaSelectata.IstoricInspectii.Add(vm.ToInspectieITP());

            List<Masina> toateMasinile = _manager.GetToateMasinile();
            for (int i = 0; i < toateMasinile.Count; i++)
            {
                if (toateMasinile[i].NrInmatriculare.ToUpper() ==
                    _masinaSelectata.NrInmatriculare.ToUpper())
                {
                    toateMasinile[i] = _masinaSelectata;
                    break;
                }
            }

            using (StreamWriter sw = new StreamWriter("Masini.txt", false))
            {
                foreach (Masina m in toateMasinile)
                    sw.WriteLine(m.ConversieLaSirPentruFisier());
            }
        }

        private void ResetFormular()
        {
            _modEditare = false;
            _indexEditare = -1;

            InspectieViewModel vm = new InspectieViewModel();
            this.DataContext = vm;

            cmbRezultat.SelectedIndex = 0;
            lblTitluFormular.Content = "ADAUGA INSPECTIE NOUA";
            btnSalveaza.Content = "Salveaza";

            if (dgInspectii != null)
                dgInspectii.SelectedItem = null;
        }
    }
}
