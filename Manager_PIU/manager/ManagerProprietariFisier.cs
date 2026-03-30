using System.IO;
using Modele_PIU.models;

namespace Manager_PIU.manager
{
    public class ManagerProprietariFisier
    {
        private string numeFisier = "Proprietari.txt";

        public ManagerProprietariFisier()
        {
            if (!File.Exists(numeFisier)) File.Create(numeFisier).Close();
        }

        public void AdaugaProprietar(Proprietar p)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                // Salvam simplu: Nume;CNP;Telefon
                sw.WriteLine($"{p.Nume};{p.CNP};{p.Telefon}");
            }
        }
    }
}