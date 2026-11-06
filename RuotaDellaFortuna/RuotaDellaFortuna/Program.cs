using System;
using System.Collections.Generic;
using System.Threading;

namespace RuotaDellaFortuna
{
    internal class Giocatore
    {
        public string Nome { get; set; }
        public int ContoRound { get; set; } = 0;
        public int BancaTotale { get; set; } = 0;
    }

    internal class Program
    {
        struct GameVariables
        {
            public bool CheckRuota;
            public char[][] JaggedSupporto;
            public char[][] Frase;
            public char[][] Tastiera;
            public string Argomento;
            public string LogTurno;
        }

        enum ValoreRuota
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
            Victory,
            Running,
        }

        static HashSet<char> consonanti = new HashSet<char>()
        {
            'b','c','d','f','g','h','l','m','n','p','q','r','s','t','v','z'
        };

        static HashSet<char> lettereGiuste = new HashSet<char>();
        static HashSet<char> lettereSbagliate = new HashSet<char>();

        static GameVariables game = new GameVariables();
        static GameStatus status = GameStatus.Menu;

        static Giocatore g1 = new Giocatore();
        static Giocatore g2 = new Giocatore();
        static bool turnoGiocatore1 = true;

        static Giocatore GiocatoreCorrente => turnoGiocatore1 ? g1 : g2;
        static Giocatore GiocatoreAvversario => turnoGiocatore1 ? g2 : g1;
        static int moltiplicatoreCorrente = 100;

        static void Main(string[] args)
        {
            InizializzaTastiera();
            bool continua = true;

            while (continua)
            {
                status = GameStatus.Menu;
                Console.Clear();
                Menu();
                Console.Write("Inserisci numero --> ");

                if (!int.TryParse(Console.ReadLine(), out int scelta) || scelta < 0 || scelta > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n!Input non valido! Deve essere compreso tra 0 e 3.");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    continue;
                }

                switch (scelta)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Giocatore 1, inserisci il tuo nickname!: ");
                        g1.Nome = (Console.ReadLine()?.ToUpper()) ?? "GIOCATORE 1";
                        Console.Write("Giocatore 2, inserisci il tuo nickname!: ");
                        g2.Nome = (Console.ReadLine()?.ToUpper()) ?? "GIOCATORE 2";

                        g1.ContoRound = 0; g1.BancaTotale = 0;
                        g2.ContoRound = 0; g2.BancaTotale = 0;
                        turnoGiocatore1 = true;

                        game.LogTurno = "";
                        SceltaFraseRandom();
                        break;

                    case 2:
                        Regole();
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("=== STATISTICHE ATTUALI ===");
                        Console.WriteLine($"{g1.Nome} -> Conto Round: {g1.ContoRound} | Banca: {g1.BancaTotale}");
                        Console.WriteLine($"{g2.Nome} -> Conto Round: {g2.ContoRound} | Banca: {g2.BancaTotale}\n");
                        Console.WriteLine("Attivita' turno: \n\n" + (string.IsNullOrEmpty(game.LogTurno) ? "Nessuna attività registrata." : game.LogTurno));
                        Console.WriteLine("\nPremi un tasto per tornare al menu...");
                        Console.ReadKey();
                        break;

                    case 0:
                        continua = false;
                        Console.WriteLine("Grazie per aver giocato!");
                        break;
                }
            }
        }

        static void CambiaTurno()
        {
            turnoGiocatore1 = !turnoGiocatore1;
        }

        static void Gioco()
        {
            status = GameStatus.Running;
            bool continuaRound = true;

            while (continuaRound && status == GameStatus.Running)
            {
                if (!game.CheckRuota)
                {
                    bool giraAncora = Ruota();
                    if (!giraAncora) continue;
                }
                game.CheckRuota = false;

                Stampa();

                if (CheckVittoria())
                {
                    Vittoria();
                    status = GameStatus.Victory;
                    return;
                }

                char guessLettera = GuessChar();

                if (status == GameStatus.Menu) return;
                if (guessLettera == 'V' || guessLettera == 'R') continue; // Vocale o tentativo di risoluzione gestiti

                bool indovinata = CheckLetteraIndovinata(guessLettera);
                if (indovinata)
                {
                    lettereGiuste.Add(guessLettera);
                    game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha indovinato la lettera '{guessLettera}'.\n\n";
                }
                else
                {
                    lettereSbagliate.Add(guessLettera);
                    game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha provato la lettera '{guessLettera}', ma era errata. Cambio turno.\n\n";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nLa lettera '{guessLettera}' non è presente nella frase!");
                    Console.ResetColor();
                    Thread.Sleep(2000);

                    GiocatoreCorrente.ContoRound = 0;
                    CambiaTurno();
                }
            }
        }

        static bool CheckLetteraIndovinata(char lettera)
        {
            int count = 0;
            for (int i = 0; i < game.Frase.Length; i++)
            {
                for (int j = 0; j < game.Frase[i].Length; j++)
                {
                    if (lettera == game.Frase[i][j])
                    {
                        game.JaggedSupporto[i][j] = game.Frase[i][j];
                        count++;
                    }
                }
            }

            if (count > 0 && !lettereGiuste.Contains(lettera))
            {
                GiocatoreCorrente.ContoRound += (moltiplicatoreCorrente * count);
            }
            return count > 0;
        }

        static bool Ruota()
        {
            Console.Clear();
            Console.WriteLine($"Turno di {GiocatoreCorrente.Nome}");
            Console.Write("Premi un tasto per girare la ruota!");
            Console.ReadKey();

            ValoreRuota[] ruota =
            {
                ValoreRuota.Cento, ValoreRuota.Cento,
                ValoreRuota.Duecento, ValoreRuota.Duecento,
                ValoreRuota.Trecento, ValoreRuota.Trecento,
                ValoreRuota.Cinquecento, ValoreRuota.Cinquecento,
                ValoreRuota.Settecentocinquanta,
                ValoreRuota.Mille,
                ValoreRuota.SkipTurno,
                ValoreRuota.Bancarotta
            };

            Random rnd = new Random();
            ValoreRuota risultato = ruota[rnd.Next(0, ruota.Length)];

            switch (risultato)
            {
                case ValoreRuota.Bancarotta:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("BANCAROTTA! Hai perso tutti i soldi del round e il turno!");
                    Console.ResetColor();
                    GiocatoreCorrente.ContoRound = 0;
                    game.LogTurno += $"\t{GiocatoreCorrente.Nome} è finito in bancarotta.\n\n";
                    Thread.Sleep(2000);
                    CambiaTurno();
                    return false;

                case ValoreRuota.SkipTurno:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("SKIP TURNO! Salti il turno!");
                    Console.ResetColor();
                    game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha fatto skip turno.\n\n";
                    Thread.Sleep(2000);
                    CambiaTurno();
                    return false;

                default:
                    moltiplicatoreCorrente = (int)risultato;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Giochi per {moltiplicatoreCorrente} punti a lettera!");
                    Thread.Sleep(1500);
                    Console.ResetColor();
                    game.LogTurno += $"\t{GiocatoreCorrente.Nome} gira la ruota per {moltiplicatoreCorrente} punti.\n\n";
                    return true;
            }
        }

        static char GuessChar()
        {
            while (true)
            {
                Console.Clear();
                Stampa();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[1] Negozio Vocali (500 pt) | [2] Risolvi la frase | [0] Torna al Menu");
                Console.ResetColor();
                Console.Write("Inserisci una consonante o un comando --> ");

                string input = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrEmpty(input)) continue;

                if (input == "0")
                {
                    status = GameStatus.Menu;
                    return '0';
                }

                if (input == "1")
                {
                    NegozioVocali();
                    return 'V';
                }

                if (input == "2")
                {
                    TentativoRisoluzioneFrase();
                    return 'R';
                }

                char c = input[0];

                if (!consonanti.Contains(c))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Puoi inserire solo consonanti, oppure usa i comandi [1] o [2]!");
                    Console.ResetColor();
                    Thread.Sleep(1800);
                    continue;
                }

                if (lettereGiuste.Contains(c) || lettereSbagliate.Contains(c))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lettera già tentata in precedenza!");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    continue;
                }

                return c;
            }
        }

        static void TentativoRisoluzioneFrase()
        {
            game.CheckRuota = true; // Evita di rigirare la ruota al ritorno

            Console.Clear();
            Stampa();
            Console.Write("Inserisci la frase completa (usa i trattini '-' al posto degli spazi se vuoi, es: parola-parola) --> ");
            string tentativo = Console.ReadLine()?.ToLower().Trim();

            // Ricostruisci la frase vera unendo le righe
            List<string> pezzi = new List<string>();
            for (int i = 0; i < game.Frase.Length; i++)
            {
                pezzi.Add(new string(game.Frase[i]));
            }
            string fraseVera = string.Join("-", pezzi).ToLower();
            // Accetta anche spazi normali al posto dei trattini per comodità dell'utente
            string tentativoNormalizzato = tentativo.Replace(" ", "-");

            if (tentativoNormalizzato == fraseVera)
            {
                // Rivela tutta la griglia
                for (int i = 0; i < game.Frase.Length; i++)
                {
                    for (int j = 0; j < game.Frase[i].Length; j++)
                    {
                        game.JaggedSupporto[i][j] = game.Frase[i][j];
                    }
                }
                Stampa();
                game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha risolto l'intera frase ed ha vinto il round!\n\n";
                Vittoria();
                status = GameStatus.Victory;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTentativo errato! Perdi i punti del round e il turno.");
                Console.ResetColor();
                game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha tentato di risolvere la frase ma ha sbagliato. Cambio turno.\n\n";
                GiocatoreCorrente.ContoRound = 0;
                Thread.Sleep(2500);
                CambiaTurno();
            }
        }

        static void NegozioVocali()
        {
            game.CheckRuota = true;

            if (GiocatoreCorrente.ContoRound < 500)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSaldo del round insufficiente! (Servono almeno 500 punti)");
                Console.ResetColor();
                Thread.Sleep(2000);
                return;
            }

            Console.Clear();
            Stampa();
            Console.Write("Scegli la vocale da comprare:\n [1] A\n [2] E\n [3] I\n [4] O\n [5] U\n [0] Annulla\nLa tua scelta --> ");

            if (!int.TryParse(Console.ReadLine(), out int sceltaVocale) || sceltaVocale < 0 || sceltaVocale > 5)
            {
                return;
            }

            if (sceltaVocale == 0) return;

            char vocaleScelta = ' ';
            switch (sceltaVocale)
            {
                case 1:
                    vocaleScelta = 'a';
                    break;
                case 2:
                    vocaleScelta = 'e';
                    break;
                case 3:
                    vocaleScelta = 'i';
                    break;
                case 4:
                    vocaleScelta = 'o';
                    break;
                case 5:
                    vocaleScelta = 'u';
                    break;
                default:
                    vocaleScelta = ' ';
                    break;
            }

            if (lettereGiuste.Contains(vocaleScelta))
            {
                Console.WriteLine("Vocale già scoperta!");
                Thread.Sleep(1500);
                return;
            }

            GiocatoreCorrente.ContoRound -= 500;
            lettereGiuste.Add(vocaleScelta);

            bool trovata = CheckLetteraIndovinata(vocaleScelta);
            if (trovata)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nLa vocale '{vocaleScelta}' era presente nella frase!");
                game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha comprato la vocale '{vocaleScelta}' (trovata).\n\n";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nLa vocale '{vocaleScelta}' non è presente nella frase.");
                game.LogTurno += $"\t{GiocatoreCorrente.Nome} ha comprato la vocale '{vocaleScelta}' (assente).\n\n";
            }
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        static void Stampa()
        {
            Intestazione();

            for (int i = 0; i < game.Frase.Length; i++)
            {
                for (int j = 0; j < game.Frase[i].Length; j++)
                {
                    Console.Write(game.JaggedSupporto[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            StampaTastiera();
            Console.WriteLine();
        }

        static void StampaTastiera()
        {
            for (int i = 0; i < game.Tastiera.Length; i++)
            {
                for (int j = 0; j < game.Tastiera[i].Length; j++)
                {
                    if (lettereGiuste.Contains(game.Tastiera[i][j]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(game.Tastiera[i][j] + " ");
                        Console.ResetColor();
                    }
                    else if (lettereSbagliate.Contains(game.Tastiera[i][j]))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(game.Tastiera[i][j] + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(game.Tastiera[i][j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        static bool CheckVittoria()
        {
            for (int i = 0; i < game.JaggedSupporto.Length; i++)
            {
                for (int j = 0; j < game.JaggedSupporto[i].Length; j++)
                {
                    if (game.JaggedSupporto[i][j] == '_') return false;
                }
            }
            return true;
        }

        static void Vittoria()
        {
            Console.Clear();
            for (int i = 0; i < game.Frase.Length; i++)
            {
                for (int j = 0; j < game.Frase[i].Length; j++)
                {
                    Console.Write(game.JaggedSupporto[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            GiocatoreCorrente.BancaTotale += GiocatoreCorrente.ContoRound;
            Console.WriteLine($"Complimenti {GiocatoreCorrente.Nome}, hai indovinato la frase!\n" +
                              $"Hai vinto {GiocatoreCorrente.ContoRound} punti in questo round. Totale in banca: {GiocatoreCorrente.BancaTotale}\n" +
                              $"Premi un tasto per continuare...");

            GiocatoreCorrente.ContoRound = 0;
            Console.ReadKey();
        }

        static char[][] CreaJaggedSupporto()
        {
            game.JaggedSupporto = new char[4][];

            for (int i = 0; i < game.Frase.Length; i++)
            {
                game.JaggedSupporto[i] = new char[game.Frase[i].Length];

                for (int j = 0; j < game.JaggedSupporto[i].Length; j++)
                {
                    if (game.Frase[i][j] == '-')
                        game.JaggedSupporto[i][j] = ' ';
                    else
                        game.JaggedSupporto[i][j] = '_';
                }
            }
            return game.JaggedSupporto;
        }

        static void SceltaFraseRandom()
        {
            Random rnd = new Random();
            int numero = rnd.Next(1, 12);
            switch (numero)
            {
                case 1: FraseEasteregg(); break;
                case 2: FraseAmicizia(); break;
                case 3: FraseMododidire(); break;
                case 4: FraseNatura(); break;
                case 5: FraseProverbio(); break;
                case 6: FraseViaggio(); break;
                case 7: FraseSport(); break;
                case 8: FraseMusica(); break;
                case 9: FraseCinema(); break;
                case 10: FraseTecnologia(); break;
                case 11: FraseCucina(); break;
            }
        }

        static void InizializzaTastiera()
        {
            game.Tastiera = new char[][]
            {
                new char[] { 'q','w','e','r','t','y','u','i','o'},
                new char[] { 'a','s','d','f','g','h','l','p'},
                new char[] { 'z','x','c','v','b','n','m'}
            };
        }

        static void Intestazione()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RUOTA DELLA FORTUNA\r\n=====>-------<=====");
            Console.ResetColor();
            Console.WriteLine("Turno di: " + GiocatoreCorrente.Nome);
            Console.WriteLine($"Conto Round: {GiocatoreCorrente.ContoRound} ");
            Console.WriteLine($"Banca Totale: {GiocatoreCorrente.BancaTotale} ");
            Console.WriteLine(game.Argomento + "\n");
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Premi un tasto per continuare...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        // FRASI
        static void FraseEasteregg() { game.Argomento = "Argomento --> Frase easter egg!"; game.Frase = new char[4][]; game.Frase[0] = "vinsero-la".ToCharArray(); game.Frase[1] = "battaglia".ToCharArray(); game.Frase[2] = "grazie-alla".ToCharArray(); game.Frase[3] = "loro-fuga".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseProverbio() { game.Argomento = "Argomento --> Proverbio italiano!"; game.Frase = new char[4][]; game.Frase[0] = "chi-dorme-non".ToCharArray(); game.Frase[1] = "piglia-pesci".ToCharArray(); game.Frase[2] = "ma-chi-lavora".ToCharArray(); game.Frase[3] = "ottiene-tutto".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseMododidire() { game.Argomento = "Argomento --> Modo di dire!"; game.Frase = new char[4][]; game.Frase[0] = "non-tutte-le".ToCharArray(); game.Frase[1] = "ciambelle-riescono".ToCharArray(); game.Frase[2] = "sempre-con".ToCharArray(); game.Frase[3] = "il-buco-giusto".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseNatura() { game.Argomento = "Argomento --> La natura!"; game.Frase = new char[4][]; game.Frase[0] = "il-sole-sorge".ToCharArray(); game.Frase[1] = "ogni-mattina".ToCharArray(); game.Frase[2] = "e-illumina-il".ToCharArray(); game.Frase[3] = "mondo-intero".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseViaggio() { game.Argomento = "Argomento --> Il viaggio!"; game.Frase = new char[4][]; game.Frase[0] = "viaggiare-apre".ToCharArray(); game.Frase[1] = "la-mente-e".ToCharArray(); game.Frase[2] = "arricchisce-lo".ToCharArray(); game.Frase[3] = "spirito-umano".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseAmicizia() { game.Argomento = "Argomento --> L'amicizia!"; game.Frase = new char[4][]; game.Frase[0] = "un-vero-amico".ToCharArray(); game.Frase[1] = "vale-piu-di".ToCharArray(); game.Frase[2] = "mille-persone".ToCharArray(); game.Frase[3] = "conosciute".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseSport() { game.Argomento = "Argomento --> Lo sport!"; game.Frase = new char[4][]; game.Frase[0] = "lo-sport-insegna".ToCharArray(); game.Frase[1] = "la-disciplina".ToCharArray(); game.Frase[2] = "e-il-rispetto".ToCharArray(); game.Frase[3] = "delle-regole".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseMusica() { game.Argomento = "Argomento --> La musica!"; game.Frase = new char[4][]; game.Frase[0] = "la-musica-unisce".ToCharArray(); game.Frase[1] = "i-popoli-del".ToCharArray(); game.Frase[2] = "mondo-in-una".ToCharArray(); game.Frase[3] = "sola-armonia".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseCinema() { game.Argomento = "Argomento --> Il cinema!"; game.Frase = new char[4][]; game.Frase[0] = "il-grande-schermo".ToCharArray(); game.Frase[1] = "racconta-storie".ToCharArray(); game.Frase[2] = "che-restano-nel".ToCharArray(); game.Frase[3] = "cuore-per-sempre".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseTecnologia() { game.Argomento = "Argomento --> La tecnologia!"; game.Frase = new char[4][]; game.Frase[0] = "la-tecnologia-cambia".ToCharArray(); game.Frase[1] = "il-modo-in-cui".ToCharArray(); game.Frase[2] = "viviamo-e-comunichiamo".ToCharArray(); game.Frase[3] = "ogni-giorno".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
        static void FraseCucina() { game.Argomento = "Argomento --> La cucina!"; game.Frase = new char[4][]; game.Frase[0] = "una-buona-cucina".ToCharArray(); game.Frase[1] = "nasce-da-ingredienti".ToCharArray(); game.Frase[2] = "semplici-e-genuini".ToCharArray(); game.Frase[3] = "con-amore".ToCharArray(); game.JaggedSupporto = CreaJaggedSupporto(); Gioco(); }
    }
}