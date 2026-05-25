using System.Collections.Generic;
using System.IO; // Obligatoriu pentru fisiere
using System.Linq;
using Modele_PIU.models;

namespace Manager_PIU.manager
{
    public class ManagerMasiniFisier
    {
        private string numeFisier = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Masini.txt"
    );

        public ManagerMasiniFisier()
        {
            if (!File.Exists(numeFisier)) File.Create(numeFisier).Close();
        }

        public void AdaugaMasina(Masina masina)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(masina.ConversieLaSirPentruFisier());
            }
        }

        public List<Masina> GetToateMasinile()
        {
            List<Masina> masini = new List<Masina>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    masini.Add(new Masina(linie));
                }
            }
            return masini;
        }

        public Masina CautaDupaNrInmatriculare(string nrInmatriculare)
        {
            return GetToateMasinile().FirstOrDefault(m => m.NrInmatriculare.ToUpper() == nrInmatriculare.ToUpper());
        }
    }
}