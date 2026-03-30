using System;
using Modele_PIU.enums;

namespace Modele_PIU.models
{
    public class InspectieITP
    {
        public DateTime DataEfectuare { get; set; }
        public Rezultat Rezultat { get; set; }
        public DateTime DataExpirare { get; set; }
        public string Defectiuni { get; set; }
        public Inspector InspectorCareAAprobat { get; set; }

        public InspectieITP(DateTime data, Rezultat rezultat, Inspector inspector, string defectiuni = "")
        {
            DataEfectuare = data;
            Rezultat = rezultat;
            InspectorCareAAprobat = inspector;
            Defectiuni = defectiuni;

            if (rezultat == Rezultat.Admis) DataExpirare = data.AddYears(2);
            else DataExpirare = data;
        }
    }
}