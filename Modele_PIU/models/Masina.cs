using System.Collections.Generic;
using Modele_PIU.enums;

namespace Modele_PIU.models
{
    public class Masina
    {
        public string Marca { get; set; }
        public string Model { get; set; }
        public int AnFabricatie { get; set; }
        public string NrInmatriculare { get; set; }
        public Proprietar Proprietar { get; set; }
        public Culoare CuloareMasina { get; set; }
        public Optiuni Dotari { get; set; }

        public List<InspectieITP> IstoricInspectii { get; set; } = new List<InspectieITP>();

        public Masina(string marca, string model, int an, string nrInmatriculare, Proprietar proprietar, Culoare culoare = Culoare.Alb, Optiuni dotari = Optiuni.Niciuna)
        {
            Marca = marca; Model = model; AnFabricatie = an; NrInmatriculare = nrInmatriculare; Proprietar = proprietar; CuloareMasina = culoare; Dotari = dotari;
        }
        public Masina(string linieFisier)
        {
            var date = linieFisier.Split(';');
            Marca = date[0];
            Model = date[1];
            AnFabricatie = int.Parse(date[2]);
            NrInmatriculare = date[3];
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{Marca};{Model};{AnFabricatie};{NrInmatriculare}";
        }
    }
}