using Npgsql;

namespace trabalho_bd_log.src
{
    public class Redo
    {
        private static string connectionString = "Host=localhost;" + "Username=postgres;" + "Password=postgres;" + "Database=banco_log";

        #region Public Methods

        public void RecuperarLog()
        {

            using var conexaoBanco = new NpgsqlConnection(connectionString);

            conexaoBanco.Open();

            using var consulta = BuscarLogs(conexaoBanco);
            var logs = LerLogs(consulta);


            Console.WriteLine("Transações que devem realizar REDO:");
            foreach (var operacao in logs)
                Console.WriteLine($"id: {operacao.Id}, transação: {operacao.Operacao}, id-cliente: {operacao.IdCliente}, nome: {operacao.Nome}, saldo: {operacao.Saldo}");

        }
       
        #endregion

        #region Private Methods

        private NpgsqlDataReader BuscarLogs(NpgsqlConnection conexaoBanco)
        {
            var comando = new NpgsqlCommand(@"
                SELECT id,  operacao, id_cliente, nome, saldo FROM log ORDER BY id;", conexaoBanco);

            var consulta = comando.ExecuteReader();
            return consulta;
        }

        private List<Log> LerLogs(NpgsqlDataReader consulta)
        {
            var logs = new List<Log>();
            while (consulta.Read())
            {
                var log = new Log
                {
                    Id = consulta.GetInt32(0),
                    Operacao = consulta.GetString(1),
                    IdCliente = consulta.GetInt32(2),
                    Nome = consulta.GetString(3),
                    Saldo = consulta.GetDecimal(4)
                };
                logs.Add(log);
            }

            consulta.Close();
            return logs;
        }

        #endregion
    }
}
