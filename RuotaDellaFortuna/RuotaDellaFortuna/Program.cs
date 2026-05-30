using System;
using System.Collections.Generic;
using System.Threading;

namespace RuotaDellaFortuna
{
    internal class Program
    {
        struct GameVariables
        {
            public int CONTO;
            public char[][] JaggedSupporto;
            public char[][] FRASE;
        }

        static HashSet<char> consonanti = new HashSet<char>()
        {
            'b','c','d','f','g','h','l','m','n','p','q','r','s','t','v','z'
        };
        static HashSet<char> vocali = new HashSet<char>()
        {
            'a','e','i','o','u'
        };

        static GameVariables game = new GameVariables();
        

        static void Main(string[] args)
        {
            bool continua = true;
            int scelta;
            int Counter1stcheck = 0;

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
                        SceltaFraseRandom();
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

        static void SceltaFraseRandom()
        {
            Random rnd = new Random();
            int numero = rnd.Next(1, 7);
            switch (numero)
            {
                case 1: FraseEasteregg(); break;
                case 2: FraseAmicizia(); break;
                case 3: FraseMododidire(); break;
                case 4: FraseNatura(); break;
                case 5: FraseProverbio(); break;
                case 6: FraseViaggio(); break;
            }
        }

        static void FraseEasteregg()
        {
            string argomento = "Argomento --> Frase easter egg!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "vinsero-la".ToCharArray();
            game.FRASE[1] = "battaglia".ToCharArray();
            game.FRASE[2] = "grazie-alla".ToCharArray();
            game.FRASE[3] = "loro-fuga".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static void FraseProverbio()
        {
            string argomento = "Argomento --> Proverbio italiano!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "chi-dorme-non".ToCharArray();
            game.FRASE[1] = "piglia-pesci".ToCharArray();
            game.FRASE[2] = "ma-chi-lavora".ToCharArray();
            game.FRASE[3] = "ottiene-tutto".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static void FraseMododidire()
        {
            string argomento = "Argomento --> Modo di dire!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "non-tutte-le".ToCharArray();
            game.FRASE[1] = "ciambelle-riescono".ToCharArray();
            game.FRASE[2] = "sempre-con".ToCharArray();
            game.FRASE[3] = "il-buco-giusto".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static void FraseNatura()
        {
            string argomento = "Argomento --> La natura!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "il-sole-sorge".ToCharArray();
            game.FRASE[1] = "ogni-mattina".ToCharArray();
            game.FRASE[2] = "e-illumina-il".ToCharArray();
            game.FRASE[3] = "mondo-intero".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static void FraseViaggio()
        {
            string argomento = "Argomento --> Il viaggio!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "viaggiare-apre".ToCharArray();
            game.FRASE[1] = "la-mente-e".ToCharArray();
            game.FRASE[2] = "arricchisce-lo".ToCharArray();
            game.FRASE[3] = "spirito-umano".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static void FraseAmicizia()
        {
            string argomento = "Argomento --> L'amicizia!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "un-vero-amico".ToCharArray();
            game.FRASE[1] = "vale-piu-di".ToCharArray();
            game.FRASE[2] = "mille-persone".ToCharArray();
            game.FRASE[3] = "conosciute".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Stampa(argomento);
        }

        static char[][] CreaJaggedSupporto()
        {
            game.JaggedSupporto = new char[4][];

            for (int i = 0; i < game.FRASE.Length; i++)
            {
                game.JaggedSupporto[i] = new char[game.FRASE[i].Length];

                for (int j = 0; j < game.JaggedSupporto[i].Length; j++)
                {
                    if (game.FRASE[i][j] == '-')
                        game.JaggedSupporto[i][j] = ' ';
                    else
                        game.JaggedSupporto[i][j] = '_';
                }
            }

            return game.JaggedSupporto;
        }

        static void Stampa(string argomento)
        {
            bool ContinuaRound = true;
            while (ContinuaRound)
            {
                Intestazione(argomento);

                for (int i = 0; i < game.FRASE.Length; i++)
                {
                    for (int j = 0; j < game.FRASE[i].Length; j++)
                    {
                        Console.Write(game.JaggedSupporto[i][j] + " ");
                    }
                    Console.WriteLine();
                }
                char GuessLettera = GuessChar();
                ContinuaRound = CheckLetteraIndovinata(GuessLettera);
            }
            Console.WriteLine("mi spiace, la lettera che hai provato a indovinare non e' presente nella frase!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("DEBUG: POSSIBILITA' DI RESPIN");
            Console.ResetColor();
        }

        static bool CheckLetteraIndovinata(char lettera)
        {
            int count = 0;
            for (int i = 0; i < game.FRASE.Length; i++)
            {
                for (int j = 0; j < game.FRASE[i].Length; j++)
                {
                    if (lettera == game.FRASE[i][j])
                    {
                        game.JaggedSupporto[i][j] = game.FRASE[i][j];
                        count++;
                    }
                }
            }
            return count > 0;
        }

        static char GuessChar()
        {
            Console.Write("Prova a indovinare una consonante! --> ");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                Console.Write("nessun carattere inserito, riprovare: ");
                input = Console.ReadLine();
            }
            while (!consonanti.Contains(input[0]))
            {
                Console.Write("non è possibile inserire una vocale o un numero, riprovare: ");
                input = Console.ReadLine();
            }


            return input[0];
        }

        static void Stampadebug()
        {
            for (int i = 0; i < game.FRASE.Length; i++)
            {
                for (int j = 0; j < game.FRASE[i].Length; j++)
                {
                    if (game.FRASE[i][j] == '-')
                        Console.Write("  ");
                    else
                        Console.Write(game.JaggedSupporto[i][j] + " ");
                }
                Console.WriteLine();
            }
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0 - Esci");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.ResetColor();
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