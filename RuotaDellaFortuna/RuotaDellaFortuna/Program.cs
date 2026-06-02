using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace RuotaDellaFortuna
{
    internal class Program
    {
        struct GameVariables
        {
            public int CONTO;
            public int BANCA;
            public char[][] JaggedSupporto;
            public char[][] FRASE;
            public int MOLTIPLICATORE;
            public bool CHECKRUOTA;
            public string ARGOMENTO;
            public bool CHECKCONTO;
        }
        enum ValoreRouta
        {
            Cento = 100,
            Duecento = 200,
            Trecento = 300,
            Cinquecento = 500,
            Settecentocinquanta = 750,
            Mille = 1000,
            SkipTurno = -1,
            Bancarotta = -2
        }
        enum GameStatus
        {
            Menu,
            Skip,
            Running,
        }

        static HashSet<char> consonanti = new HashSet<char>()
        {
            'b','c','d','f','g','h','l','m','n','p','q','r','s','t','v','z'
        };
        static List<char> vocali = new List<char>()
        {
            'a','e','i','o','u'
        };

        static GameVariables game = new GameVariables();

        static GameStatus status = new GameStatus();


        static void Main(string[] args)
        {

            bool continua = true;
            int scelta;
            int Counter1stcheck = 0;
            game.CONTO = 100000;

            while (continua)
            {
                GoBack:
                status = GameStatus.Running;
                if (status == GameStatus.Menu)
                {
                    goto GoBack;
                }
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
                        goto GoBack;
                    case 2:
                        Regole();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine($"Statistiche attuali: \n Conto: {game.CONTO}\n Banca: {game.BANCA}");
                        break;
                    case 0:
                        continua = false;
                        Console.WriteLine("Grazie per aver giocato!");
                        break;
                }
            }
        }

        static void Ruota()
        {
            Console.Clear();
            Respin:
            Console.Write("Premi un tasto per girare la ruota!");
            Console.ReadKey();        
        ValoreRouta[] ruota =
            {
                ValoreRouta.Cento,
                ValoreRouta.Cento,
                ValoreRouta.Duecento,
                ValoreRouta.Duecento,
                ValoreRouta.Trecento,
                ValoreRouta.Trecento,
                ValoreRouta.Cinquecento,
                ValoreRouta.Cinquecento,
                ValoreRouta.Settecentocinquanta,
                ValoreRouta.Mille,
                ValoreRouta.SkipTurno,
                ValoreRouta.Bancarotta
            };

            Random rnd = new Random();
            int temp = rnd.Next(0, ruota.Length);
            ValoreRouta risultato = ruota[temp];
            switch (risultato)
            {
                case ValoreRouta.Bancarotta:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("BANCAROTTA! Hai perso tutti i soldi! Ricominci a 0!");
                    Console.ResetColor();
                    status = GameStatus.Menu;
                    game.BANCA = 0;
                    Thread.Sleep(2000);
                    goto Respin;
                case ValoreRouta.SkipTurno:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Skip turno! Respin!");
                    Console.ResetColor();               
                    Thread.Sleep(2000);
                    Console.Clear();
                    goto Respin;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Giochi per {(int)risultato} punti!");
                    Thread.Sleep(2000);
                    Console.ResetColor();
                    game.MOLTIPLICATORE = (int)risultato;
                    break;

            }
            return;
            
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
            return;
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

        static void Gioco()
        {
            if (!game.CHECKRUOTA)
            {
                Ruota();
            }
            game.CHECKRUOTA = false;
            bool ContinuaRound = true;
            while (ContinuaRound)
            {

                Stampa();

                char GuessLettera = GuessChar();
                ContinuaRound = CheckLetteraIndovinata(GuessLettera);
            }
            Console.Clear();
            Console.WriteLine("mi spiace, la lettera che hai provato a indovinare non e' presente nella frase!");
            status = GameStatus.Menu;
            Console.ResetColor();      
            return;
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
            if (!game.CHECKCONTO)
            { 
                game.CONTO += (game.MOLTIPLICATORE * count);
            }
            game.CHECKCONTO = false;
            return count > 0;
        }

        static char GuessChar()
        {
            //if (string.IsNullOrEmpty()
            {
                Console.Write("Prova a indovinare una consonante! --> ");
            }

            Stamp1();
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                Console.Write("nessun carattere inserito, riprovare: ");
                input = Console.ReadLine();
                Stamp1();
            }
            while (!consonanti.Contains(input[0]) && input[0] != '0' && input[0] != '1')
            {
                Console.Write("non è possibile inserire una vocale o un numero, riprovare: ");
                input = Console.ReadLine();
                Stamp1();
            }
            switch (input[0])
            {
                case '0':
                status = GameStatus.Menu;
                    break;
                case '1':
                    NegozioVocali();
                    break;
            }
            return input[0];
        }
        static void NegozioVocali()
        {
            game.CHECKRUOTA = true;
            game.CHECKCONTO = true;
            int temp = 0;
            if (game.CONTO < 500) { Console.WriteLine("Saldo insufficiente"); Thread.Sleep(1500); return; }
            while (true)
            {
                try
                {
                    Console.Clear();
                    Stampa();
                    Console.Write("Scegli quale vocale comprare, e scoprire se la frase la contiene! \n a = 1\n e = 2 \n 3 = i \n o = 4 \n 5 = u \n 0 per uscire \n La tua scelta --> ");
                    temp = int.Parse(Console.ReadLine());
                    break; // input valido, esci dal while
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Inserire numeri, non lettere!");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    Console.Write("La tua scelta --> ");
                }
            }

            switch (temp)
            {
                case 1:
                    CheckLetteraIndovinata('a');
                    break;
                case 2:
                    CheckLetteraIndovinata('e');
                    break;
                case 3:
                    CheckLetteraIndovinata('i');
                    break;
                case 4:
                    CheckLetteraIndovinata('o');
                    break;
                case 5:
                    CheckLetteraIndovinata('u');
                    break;
                case 0: status = GameStatus.Menu; return;
            }
            game.CONTO -= 500;
            Gioco();
        }

        static void Stampa()
        {
            Intestazione();

            for (int i = 0; i < game.FRASE.Length; i++)
            {
                for (int j = 0; j < game.FRASE[i].Length; j++)
                {
                    Console.Write(game.JaggedSupporto[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void GestioneConto()
        {
            game.BANCA += game.CONTO;
            game.CONTO = 0;
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

        static void FraseEasteregg()
        {
            game.ARGOMENTO = "Argomento --> Frase easter egg!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "vinsero-la".ToCharArray();
            game.FRASE[1] = "battaglia".ToCharArray();
            game.FRASE[2] = "grazie-alla".ToCharArray();
            game.FRASE[3] = "loro-fuga".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void FraseProverbio()
        {
            game.ARGOMENTO = "Argomento --> Proverbio italiano!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "chi-dorme-non".ToCharArray();
            game.FRASE[1] = "piglia-pesci".ToCharArray();
            game.FRASE[2] = "ma-chi-lavora".ToCharArray();
            game.FRASE[3] = "ottiene-tutto".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void FraseMododidire()
        {
            game.ARGOMENTO = "Argomento --> Modo di dire!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "non-tutte-le".ToCharArray();
            game.FRASE[1] = "ciambelle-riescono".ToCharArray();
            game.FRASE[2] = "sempre-con".ToCharArray();
            game.FRASE[3] = "il-buco-giusto".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void FraseNatura()
        {
            game.ARGOMENTO = "Argomento --> La natura!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "il-sole-sorge".ToCharArray();
            game.FRASE[1] = "ogni-mattina".ToCharArray();
            game.FRASE[2] = "e-illumina-il".ToCharArray();
            game.FRASE[3] = "mondo-intero".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void FraseViaggio()
        {
            game.ARGOMENTO = "Argomento --> Il viaggio!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "viaggiare-apre".ToCharArray();
            game.FRASE[1] = "la-mente-e".ToCharArray();
            game.FRASE[2] = "arricchisce-lo".ToCharArray();
            game.FRASE[3] = "spirito-umano".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void FraseAmicizia()
        {
            game.ARGOMENTO = "Argomento --> L'amicizia!";
            game.FRASE = new char[4][];
            game.FRASE[0] = "un-vero-amico".ToCharArray();
            game.FRASE[1] = "vale-piu-di".ToCharArray();
            game.FRASE[2] = "mille-persone".ToCharArray();
            game.FRASE[3] = "conosciute".ToCharArray();

            game.JaggedSupporto = CreaJaggedSupporto();
            Gioco();
            return;
        }

        static void Intestazione()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RUOTA DELLA FORTUNA\r\n=====>-------<=====");
            Console.ResetColor();
            Console.WriteLine($"Conto: {game.CONTO} ");
            Console.WriteLine(game.ARGOMENTO + "\n");
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

        static void Stamp1()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1 per negozio vocali, 0 per uscire");
            Console.ResetColor();
        }
    }
}