using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Modele_PIU.enums;
using Modele_PIU.models;

namespace Proiect_PIU
{
    public class InspectieViewModel : INotifyPropertyChanged
    {
        private DateTime _dataEfectuare;
        private Rezultat _rezultat;
        private string _defectiuni = string.Empty;
        private string _numeInspector = string.Empty;

        public DateTime DataEfectuare
        {
            get => _dataEfectuare;
            set { _dataEfectuare = value; OnPropertyChanged(); OnPropertyChanged(nameof(DataExpirare)); }
        }

        public DateTime DataExpirare => _dataEfectuare.AddYears(2);

        public Rezultat Rezultat
        {
            get => _rezultat;
            set { _rezultat = value; OnPropertyChanged(); OnPropertyChanged(nameof(RezultatText)); }
        }

        public string RezultatText => _rezultat == Rezultat.Admis ? "ADMIS" : "RESPINS";

        public string Defectiuni
        {
            get => _defectiuni;
            set { _defectiuni = value ?? string.Empty; OnPropertyChanged(); }
        }

        public string NumeInspector
        {
            get => _numeInspector;
            set { _numeInspector = value ?? string.Empty; OnPropertyChanged(); }
        }

        public InspectieViewModel()
        {
            _dataEfectuare = DateTime.Today;
            _rezultat = Rezultat.Admis;
        }

        public InspectieViewModel(InspectieITP inspectie)
        {
            _dataEfectuare = inspectie.DataEfectuare;
            _rezultat = inspectie.Rezultat;
            _defectiuni = inspectie.Defectiuni ?? string.Empty;
            _numeInspector = inspectie.InspectorCareAAprobat?.Nume ?? string.Empty;
        }

        public InspectieITP ToInspectieITP()
        {
            Inspector inspector = new Inspector(_numeInspector, "N/A");
            return new InspectieITP(_dataEfectuare, _rezultat, inspector, _defectiuni);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
