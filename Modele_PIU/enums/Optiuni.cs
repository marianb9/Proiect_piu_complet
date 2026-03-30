using System;

namespace Modele_PIU.enums
{
    [Flags]
    public enum Optiuni
    {
        Niciuna = 0,
        AerConditionat = 1,
        Navigatie = 2,
        CutieAutomata = 4,
        SenzoriParcare = 8
    }
}