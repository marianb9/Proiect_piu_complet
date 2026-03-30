using System;

namespace TemaAcasa
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Introduceți numărul de ore lucrate: ");
            int oreLucrate = int.Parse(Console.ReadLine());

            Console.Write("Introduceți tariful pe oră: ");
            double tarifPeOra = double.Parse(Console.ReadLine());

            double salariu = (double)oreLucrate * tarifPeOra;

            Console.WriteLine($"Salariul calculat este: {salariu} lei");

            if (salariu > 3000)
            {
                Console.WriteLine("Salariu mare");
            }
            else
            {
                Console.WriteLine("Ați lucrat prea puține ore pentru a avea un salariu mare!");
            }
        }
    }
}
