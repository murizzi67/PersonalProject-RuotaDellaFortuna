using System;
using System.Threading;

namespace RuotaDellaFortuna
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //interfaccia principale e menu
            bool continua = true;
            int scelta;
            int Counter1stcheck = 0;
            int CONTO = 0;
            while (continua)
            {
                Menu();
                Console.Write("Insersci numero --> ");
                scelta = int.Parse(Console.ReadLine());

                while (scelta < 0 || scelta >= 4)
                {
                    Console.Clear();
                    Menu();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("!Input non valido!\n");
                    if (Counter1stcheck > 1)
                    {
                        Console.WriteLine("!Deve essere compreso tra 0 e 3!");
                    }
                    Console.ResetColor();
                    Console.Write("Insersci numero --> ");
                    scelta = int.Parse(Console.ReadLine());
                    Counter1stcheck++;
                }
                switch (scelta)
                {
                    case 1:
                        FraseEasteregg();
                        //DEBUG: gioco 
                        break;
                    case 2:
                        Regole();
                        break;
                    case 3:
                        Console.WriteLine("DEBUG: visualizzazione attivita nel turno"); 
                        break;
                    case 0:
                        continua = false;
                        Console.WriteLine("Grazie per aver giocato!");
                        break;
                }
            }
        }
        static void Stampa(char[][] frase, char[][] JaggedSupporto, string argomento)
        {
            bool ContinuaRound = true;
            while (ContinuaRound)
            {
                Intestazione(argomento);

                for (int i = 0; i < frase.Length; i++)
                {
                    for (int j = 0; j < frase[i].Length; j++)
                    {
                        Console.Write(JaggedSupporto[i][j] + " ");
                    }
                    Console.WriteLine();
                }
                char GuessLettera = GuessChar();
                ContinuaRound = CheckLetteraIndovinata(frase, JaggedSupporto, GuessLettera);
            }
            Console.WriteLine("mi spiace, la lettera che hai provato a indovinare non e' presente nella frase!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("DEBUG: POSSIBILITA' DI RESPIN");
            Console.ResetColor();
        }
        static void FraseEasteregg()
        {
            string argomento = "Argomento --> Frase easter egg!";
            char[][] fraseegg = new char[4][];
            fraseegg[0] = "vinsero-la".ToCharArray();
            fraseegg[1] = "battaglia".ToCharArray();
            fraseegg[2] = "grazie-alla".ToCharArray();
            fraseegg[3] = "loro-fuga".ToCharArray();

            //bool[][] scoperte = CheckLettere(fraseegg);
            char[][] JaggedSupporto = CreaJaggedSupporto(fraseegg);
            Stampa(fraseegg, JaggedSupporto, argomento);

        }
        static char[][] CreaJaggedSupporto(char[][] frase)
        {
            char[][] JaggedSupporto = new char[4][];

            for (int i = 0; i < frase.Length; i++)
            {
                JaggedSupporto[i] = new char[frase[i].Length];

                for (int j = 0; j < JaggedSupporto[i].Length; j++)
                {
                    if (frase[i][j] == '-')
                    {
                        JaggedSupporto[i][j] = ' ';
                    }
                    else
                    {
                        JaggedSupporto[i][j] = '_';
                    }
                }
            }

            //Stampadebug(frase, JaggedSupporto);

            return JaggedSupporto;
        }
        static bool CheckLetteraIndovinata(char[][] frase, char[][] JaggedSupporto, char lettera)
        {
            int count =0;
            for (int i = 0; i < frase.Length; i++)
            { 
                for (int j = 0; j < frase[i].Length; j++)
                {
                    if (lettera == frase[i][j])
                    {
                        JaggedSupporto[i][j] = frase[i][j];
                        count++; 
                    }
                }
            }
            if (count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //{
        //    bool[][] jaggedFrase = new bool[4][];
        //    for (int i = 0; i < checkfrase.Length; i++)
        //    {
        //        jaggedFrase[i] = new bool[checkfrase[i].Length];
        //    }
        //    return jaggedFrase;
        //}
        static void tastiera(char input) //WIP      
        {
            Console.Write("Q");
        }
        static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1 - Gioca!");
            Console.ResetColor();
            Console.WriteLine("2 - Regole");
            Console.WriteLine("3 - Visualizzazione attivita'");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.ResetColor();
        }
        static void Stampadebug(char[][] frase, char[][] JaggedSupporto)
        {
            for (int i = 0; frase.Length > i; i++)
            {
                for (int j = 0; j < frase[i].Length; j++)
                {
                    if (frase[i][j] == '-')
                        Console.Write("  "); // due spazi per allineamento 
                    else
                        Console.Write(JaggedSupporto[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static char GuessChar()
        {
            Console.Write("Prova a indovinare una consonante! --> ");
            string input = Console.ReadLine();
            char c;
            while (string.IsNullOrEmpty(input))
            {
                Console.Write("nessun carattere inserito, riprovare: ");
                input = Console.ReadLine();

            }
            c = input[0];
            return c;
        }
        static void Intestazione(string argomento)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RUOTA DELLA FORTUNA\r\n=====>-------<=====");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(argomento + "\n");
        }
        static void Regole()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== RUOTA DELLA FORTUNA - Versione Console ===\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("OBIETTIVO");
            Console.ResetColor();
            Console.WriteLine("Indovinare la frase nascosta accumulando piu soldi possibile.\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("LA FRASE");
            Console.ResetColor();
            Console.WriteLine("La frase e mostrata con _ al posto delle lettere non ancora scoperte.");
            Console.WriteLine("Gli spazi sono visibili. Esempio:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("_ _ _ _   _ _ _ _ _ _\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("TURNO DI GIOCO");
            Console.ResetColor();
            Console.WriteLine("Ogni turno il giocatore sceglie tra tre azioni:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  [1] Gira la ruota  -> valore casuale: 100, 200, 300, 500, 750, 1000, FALLIMENTO, BANCAROTTA");
            Console.WriteLine("  [2] Compra una vocale -> costa 250, disponibile solo se hai almeno 250");
            Console.WriteLine("  [3] Risolvi la frase  -> tenti di indovinare l'intera frase\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DOPO AVER GIRATO LA RUOTA");
            Console.ResetColor();
            Console.WriteLine("  - Valore in denaro -> inserisci una consonante");
            Console.WriteLine("      Se presente:  guadagni valore x numero di volte che appare");
            Console.WriteLine("      Se assente:   perdi il turno");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  - FALLIMENTO      -> passi il turno ma mantieni i soldi");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  - BANCAROTTA      -> perdi tutti i soldi e il turno\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("VOCALI E CONSONANTI");
            Console.ResetColor();
            Console.WriteLine("  - Vocali (A E I O U)  -> si comprano a 250");
            Console.WriteLine("  - Consonanti          -> si ottengono girando la ruota\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("VITTORIA");
            Console.ResetColor();
            Console.WriteLine("Vinci quando risolvi correttamente la frase.");
            Console.WriteLine("Il punteggio finale e il denaro accumulato.\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("GAME OVER");
            Console.ResetColor();
            Console.WriteLine("Se tutti i giocatori vanno in bancarotta e nessuno riesce a risolvere,");
            Console.WriteLine("la frase viene rivelata e nessuno vince.\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Premi un tasto per continuare...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
    }
}
