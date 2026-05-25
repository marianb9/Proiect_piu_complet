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
        public Masina(string linie)
        {
            string[] parts = linie.Split(';');

            Marca = parts[0];
            Model = parts[1];
            AnFabricatie = int.Parse(parts[2]);
            NrInmatriculare = parts[3];

            
            Proprietar = new Proprietar(parts[4], parts[5], parts[6]);

            // Deserializam inspectiile (parts[7] poate sa nu existe pe linii vechi)
            // Deserializam inspectiile
            IstoricInspectii = new List<InspectieITP>();
            if (parts.Length > 7 && !string.IsNullOrWhiteSpace(parts[7]))
            {
                foreach (string inspStr in parts[7].Split('|'))
                {
                    string[] ip = inspStr.Split(',');
                    if (ip.Length >= 2)
                    {
                        DateTime data = DateTime.ParseExact(ip[0], "dd.MM.yyyy", null);
                        Rezultat rez = (Rezultat)Enum.Parse(typeof(Rezultat), ip[1]);
                        string numeInspector = ip.Length > 2 ? ip[2] : "";
                        string defectiuni = ip.Length > 3 ? ip[3] : "";

                        // Adapteaza parametrii la constructorii tai reali:
                        Inspector inspector = new Inspector(numeInspector, "");
                        IstoricInspectii.Add(new InspectieITP(data, rez, inspector, defectiuni));
                    }
                }
            }
        }
        public string ConversieLaSirPentruFisier()
        {
            string numeProprietar = Proprietar?.Nume ?? "";
            string cnpProprietar = Proprietar?.CNP ?? "";
            string telefonProprietar = Proprietar?.Telefon ?? "";

            // Serializam fiecare inspectie cu '|' ca separator
            string inspectiiSerializate = string.Join("|", IstoricInspectii.Select(i =>
                $"{i.DataEfectuare:dd.MM.yyyy},{i.Rezultat},{i.InspectorCareAAprobat?.Nume ?? ""},{i.Defectiuni ?? ""}"));

            return $"{Marca};{Model};{AnFabricatie};{NrInmatriculare};" +
                   $"{numeProprietar};{cnpProprietar};{telefonProprietar};" +
                   $"{inspectiiSerializate}";
        }
    }
}