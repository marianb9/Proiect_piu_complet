namespace Modele_PIU.models
{
    public class Inspector
    {
        public string Nume { get; set; }
        public string SerieInsigna { get; set; }

        public Inspector(string nume, string serie)
        {
            Nume = nume; SerieInsigna = serie;
        }
    }
}