using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            while (continua)
            {
                Menu();
                Console.Write("Insersci numero --> ");
                scelta = int.Parse(Console.ReadLine());

                while (scelta < 0 || scelta > 4)
                {

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
                        //DEBUG: gico 
                        break;
                    case 2:
                        Regole();
                        break;
                    case 3:
                        FraseEasteregg();
                        //DEBUG: visualizzazione attivita nel turno
                        break;
                    case 0:
                        continua = false;
                        Console.WriteLine("Grazie per aver giocato!");
                        break;
                }
            }
        }
        static void Stampa(char[][] frase, bool[][] scoperte)
        {
            for (int i = 0; i < frase[0][i]; i++)
            {
                Console.Write(frase);
            }
        }
        static void FraseEasteregg()
        {
            char[][] fraseegg = new char[4][];
            fraseegg[0] = "vinsero-la".ToCharArray();
            fraseegg[1] = "battaglia".ToCharArray();
            fraseegg[2] = "grazie-alla".ToCharArray();
            fraseegg[3] = "loro-fuga".ToCharArray();

            bool[][] scoperte = CheckLettere(fraseegg);
            char[][] vuoto = JaggedVuoto(fraseegg);
            //Stampa(fraseegg, scoperte);

            //todo
        }
        static char[][] JaggedVuoto(char[][] frase)
        {
            char[][] vuoto = new char[4][];

            for (int i = 0; i < frase.Length; i++)
            {
                vuoto[i] = new char[frase[i].Length];
            }
            for (int i = 0; i < vuoto.Length; i++)
            {
                for (int j = 0; j < vuoto[i].Length; j++)
                {
                    vuoto[i][j] = '_';
                }
            }

            Stampadebug(frase,vuoto);
                
            return vuoto;
        }
        static bool[][] CheckLettere(char[][] checkfrase)
        {
            bool[][] jaggedFrase = new bool[4][];
            for (int i = 0; i < checkfrase.Length; i++)
            {
                jaggedFrase[i] = new bool[checkfrase[i].Length];
            }
            return jaggedFrase;
        }
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
        static void Stampadebug(char[][] frase, char[][] vuoto)
        {
            for (int i = 0; frase.Length > i; i++)
            {
                for (int j = 0; j < frase[i].Length; j++)
                {
                    if (frase[i][j] == '-')
                        Console.Write("  "); // due spazi per allineamento
                    else
                        Console.Write(vuoto[i][j] + " ");
                }
                Console.WriteLine();
            }
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
