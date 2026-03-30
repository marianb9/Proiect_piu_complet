namespace Modele_PIU.models
{
    public class Proprietar
    {
        public string Nume { get; set; }
        public string CNP { get; set; }
        public string Telefon { get; set; }

        public Proprietar(string nume, string cnp, string telefon)
        {
            Nume = nume; CNP = cnp; Telefon = telefon;
        }
    }
}