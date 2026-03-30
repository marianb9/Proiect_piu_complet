using System;
using System.Collections.Generic;
using System.Linq;
using Modele_PIU.models;

namespace Proiect_PIU.manager
{
    public class ManagerMasini
    {
        private List<Masina> listaMasini = new List<Masina>();

        public void AdaugaMasina(Masina masina)
        {
            listaMasini.Add(masina);
        }

        public List<Masina> GetToateMasinile()
        {
            return listaMasini;
        }

        public Masina CautaDupaNrInmatriculare(string nrInmatriculare)
        {
            return listaMasini.FirstOrDefault(m => m.NrInmatriculare.ToUpper() == nrInmatriculare.ToUpper());
        }

        public List<Masina> CautaDupaMarca(string marca)
        {
            return listaMasini.Where(m => m.Marca.ToUpper() == marca.ToUpper()).ToList();
        }
    }
}