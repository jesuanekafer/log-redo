namespace trabalho_bd_log.src
{
    public class Log
    {
        public int Id { get; set; }
        public string Operacao { get; set; }
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public decimal? Saldo { get; set; }
    }

}
