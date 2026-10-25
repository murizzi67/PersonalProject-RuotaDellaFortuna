namespace RuotaDellaFortuna
{
    internal class Giocatore
    {
        private string nome;

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private int conto;

        public int Conto
        {
            get { return conto; }
            set { conto = value; }
        }

        private int banca;

        public int Banca
        {
            get { return banca; }
            set { banca = value; }
        }
        private string logturno;

        public string Logturno
        {
            get { return logturno; }
            set { logturno = value; }
        }

        private int moltiplicatore;

        public int Moltiplicatore
        {
            get { return moltiplicatore; }
            set { moltiplicatore = value; }
        }
        //public void Aggiorna
        public void AggiornaConto(int Nlettere)
        {
            conto += (Nlettere * moltiplicatore);
        }
        public void AggiornaLog(string aggiornamento)
        {
            logturno += aggiornamento;
        }
        public void ResetConto()
        {
            conto = 0;
        }
        public void ResetBanca()
        {
            banca = 0;
        }

    }
}
