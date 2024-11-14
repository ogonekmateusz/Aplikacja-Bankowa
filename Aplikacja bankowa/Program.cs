using System;

namespace Aplikacja_bankowa
{
    internal class Program
    {
        public interface IKontoBankowe
        {
            decimal Saldo { get; }
            void Wplac(decimal ilosc);
            bool Wyplac(decimal ilosc);
            bool PrzetransferujDo(IKontoBankowe cel, decimal ilosc);
        }

        public class KontoOszczednosciowe : IKontoBankowe
        {
            private decimal saldo;
            private const decimal Oplata = 5;

            public decimal Saldo => saldo; //alternatywa  public decimal Saldo { get { return saldo; } }

            public KontoOszczednosciowe(decimal saldo)
            {
                this.saldo = saldo;
            }

            public KontoOszczednosciowe() : this(0) { }

            public void Wplac(decimal ilosc)
            {
                saldo += ilosc;
            }

            public bool Wyplac(decimal ilosc)
            {
                if (saldo >= ilosc + Oplata)
                {
                    saldo -= ilosc + Oplata;
                    return true;
                }
                Console.WriteLine("Nie można wypłacić środków - niewystarczające saldo.");
                return false;
            }

            public bool PrzetransferujDo(IKontoBankowe cel, decimal ilosc)
            {
                if (Wyplac(ilosc))
                {
                    cel.Wplac(ilosc);
                    return true;
                }
                Console.WriteLine("Nie można przelać środków.");
                return false;
            }
        }

        public class KontoFirmowe : IKontoBankowe
        {
            private decimal saldo;
            private int liczbaTransakcji;
            private const int LimitTransakcji = 10;

            public decimal Saldo => saldo; //alternatywa  public decimal Saldo { get { return saldo; } }

            public KontoFirmowe(decimal saldo)
            {
                this.saldo = saldo;
                this.liczbaTransakcji = 0;
            }

            public KontoFirmowe() : this(0) { }

            public void Wplac(decimal ilosc)
            {
                if (liczbaTransakcji < LimitTransakcji)
                {
                    saldo += ilosc;
                    liczbaTransakcji++;
                }
                else
                {
                    Console.WriteLine("Limit transakcji osiągnięty. Nie można wpłacić środków.");
                }
            }

            public bool Wyplac(decimal ilosc)
            {
                if (liczbaTransakcji < LimitTransakcji && saldo >= ilosc)
                {
                    saldo -= ilosc;
                    liczbaTransakcji++;
                    return true;
                }
                Console.WriteLine("Nie można wypłacić środków - brak salda lub limit transakcji osiągnięty.");
                return false;
            }

            public bool PrzetransferujDo(IKontoBankowe cel, decimal ilosc)
            {
                if (Wyplac(ilosc))
                {
                    cel.Wplac(ilosc);
                    return true;
                }
                Console.WriteLine("Nie można przelać środków.");
                return false;
            }
        }

        static void Main(string[] args)
        {
            KontoFirmowe kontoFirmowe = new KontoFirmowe(200);
            KontoOszczednosciowe kontoOszczednosciowe = new KontoOszczednosciowe(200);

            Console.WriteLine($"Konto firmowe saldo: {kontoFirmowe.Saldo}");
            Console.WriteLine($"Konto oszczednosciowe saldo: {kontoOszczednosciowe.Saldo}");

            kontoFirmowe.PrzetransferujDo(kontoOszczednosciowe, 50);

            Console.WriteLine($"Konto firmowe saldo po przelewie: {kontoFirmowe.Saldo}");
            Console.WriteLine($"Konto oszczednosciowe saldo po przelewie: {kontoOszczednosciowe.Saldo}");
        }
    }
}
