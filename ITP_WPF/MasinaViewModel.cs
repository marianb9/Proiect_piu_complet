using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Proiect_PIU
{
    /// <summary>
    /// MVVM ViewModel pentru adaugarea unei masini.
    /// Implementeaza:
    ///   - INotifyPropertyChanged  -> notifica UI-ul la modificarea proprietatilor
    ///   - IDataErrorInfo          -> validare integrata prin binding
    /// </summary>
    public class MasinaViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        //CONSTANTE PENTRU LIMITE
        public const int MAX_MARCA = 30;
        public const int MAX_MODEL = 30;
        public const int MAX_NUME = 50;
        public const int AN_MINIM = 1900;
        public const int AN_MAXIM = 2025;
        public const int LUNGIME_CNP = 13;
        public const int LUNGIME_TELEFON = 10;

        //PROPRIETATI CU NOTIFICARE
        private string _marca = string.Empty;
        private string _model = string.Empty;
        private string _an = string.Empty;
        private string _nrInmatriculare = string.Empty;
        private string _numeProprietar = string.Empty;
        private string _cnp = string.Empty;
        private string _telefon = string.Empty;

        public string Marca
        {
            get => _marca;
            set { _marca = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string An
        {
            get => _an;
            set { _an = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string NrInmatriculare
        {
            get => _nrInmatriculare;
            set { _nrInmatriculare = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string NumeProprietar
        {
            get => _numeProprietar;
            set { _numeProprietar = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string CNP
        {
            get => _cnp;
            set { _cnp = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        public string Telefon
        {
            get => _telefon;
            set { _telefon = value; OnPropertyChanged(); OnPropertyChanged(nameof(EsteValid)); }
        }

        //IDATAERRORINFO —validare per proprietate
        //WPF apeleaza this[numeProprietate] automat la fiecare modificare prin binding
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Marca):
                        if (string.IsNullOrWhiteSpace(Marca))
                            return "Marca este obligatorie!";
                        if (Marca.Trim().Length > MAX_MARCA)
                            return $"Marca nu poate depasi {MAX_MARCA} caractere!";
                        break;

                    case nameof(Model):
                        if (string.IsNullOrWhiteSpace(Model))
                            return "Modelul este obligatoriu!";
                        if (Model.Trim().Length > MAX_MODEL)
                            return $"Modelul nu poate depasi {MAX_MODEL} caractere!";
                        break;

                    case nameof(An):
                        if (!int.TryParse(An, out int an) || an < AN_MINIM || an > AN_MAXIM)
                            return $"Anul trebuie sa fie intre {AN_MINIM} si {AN_MAXIM}!";
                        break;

                    case nameof(NrInmatriculare):
                        if (string.IsNullOrWhiteSpace(NrInmatriculare))
                            return "Nr. inmatriculare este obligatoriu!";
                        if (!Regex.IsMatch(NrInmatriculare.Trim().ToUpper(), @"^[A-Z]{1,2}-\d{2,3}-[A-Z]{3}$"))
                            return "Format invalid! Exemplu: SV-01-XYZ";
                        break;

                    case nameof(NumeProprietar):
                        if (string.IsNullOrWhiteSpace(NumeProprietar))
                            return "Numele proprietarului este obligatoriu!";
                        if (NumeProprietar.Trim().Length > MAX_NUME)
                            return $"Numele nu poate depasi {MAX_NUME} caractere!";
                        break;

                    case nameof(CNP):
                        if (!Regex.IsMatch(CNP?.Trim() ?? "", @"^\d{13}$"))
                            return "CNP-ul trebuie sa contina exact 13 cifre!";
                        break;

                    case nameof(Telefon):
                        if (!Regex.IsMatch(Telefon?.Trim() ?? "", @"^\d{10}$"))
                            return "Telefonul trebuie sa contina exact 10 cifre!";
                        break;
                }
                return null;
            }
        }

        //Eroare la nivel de obiect , nu e folosita, dar e ceruta de interfata
        public string Error => null;

        //ESTEVALID — activeaza butonul Adauga automat prin binding
        public bool EsteValid =>
            string.IsNullOrEmpty(this[nameof(Marca)]) &&
            string.IsNullOrEmpty(this[nameof(Model)]) &&
            string.IsNullOrEmpty(this[nameof(An)]) &&
            string.IsNullOrEmpty(this[nameof(NrInmatriculare)]) &&
            string.IsNullOrEmpty(this[nameof(NumeProprietar)]) &&
            string.IsNullOrEmpty(this[nameof(CNP)]) &&
            string.IsNullOrEmpty(this[nameof(Telefon)]);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
