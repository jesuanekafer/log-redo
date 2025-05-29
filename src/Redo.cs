using Npgsql;

namespace trabalho_bd_log.src
{
    public class Redo
    {
        private static string connectionString = "Host=localhost;" + "Username=postgres;" + "Password=kafer;" + "Database=banco_log";

        #region Public Methods

        public void RecuperarLog()
        {
            using var conexaoBanco = new NpgsqlConnection(connectionString);
            conexaoBanco.Open();

            var todosLogs = LerTodosLogs(conexaoBanco);

            var transacoesComitadasIds = IdentificarTransacoesComitadas(todosLogs);

            var logsParaRedo = FiltrarLogsParaRedo(todosLogs, transacoesComitadasIds);

            Console.WriteLine("Transações que devem realizar REDO:");
            foreach (var operacao in logsParaRedo)
                Console.WriteLine($"id: {operacao.Txid}, transação: {operacao.Operacao}, id-cliente: {operacao.IdCliente}, nome: {operacao.Nome}, saldo: {operacao.Saldo}");

            RecuperarTabelaClientesEmMemoria(conexaoBanco, logsParaRedo);
        }

        #endregion

        #region Private Methods

        private List<Log> LerTodosLogs(NpgsqlConnection conexaoBanco)
        {
            var comando = new NpgsqlCommand("SELECT * FROM log ORDER BY txid;", conexaoBanco);
            using var consulta = comando.ExecuteReader();

            var logs = new List<Log>();
            while (consulta.Read())
            {
                var log = new Log
                {
                    Txid = consulta.GetInt32(0),
                    Operacao = consulta.GetString(1),
                    IdCliente = consulta.IsDBNull(2) ? (int?)null : consulta.GetInt32(2),
                    Nome = consulta.IsDBNull(3) ? null : consulta.GetString(3),
                    Saldo = consulta.IsDBNull(4) ? (decimal?)null : consulta.GetDecimal(4)
                };
                logs.Add(log);
            }

            return logs;
        }

        private List<long> IdentificarTransacoesComitadas(List<Log> todosLogs)
        {
            var transacoesComitadasIds = new List<long>();

            foreach (var log in todosLogs)
            {
                if (log.Operacao == "END")
                {
                    transacoesComitadasIds.Add(log.Txid);
                }
            }

            return transacoesComitadasIds;
        }

        private List<Log> FiltrarLogsParaRedo(List<Log> todosLogs, List<long> transacoesComitadas)
        {
            return todosLogs
                .Where(log => transacoesComitadas.Contains(log.Txid)
                            && log.Operacao != "BEGIN"
                            && log.Operacao != "END")
                .ToList();
        }

        private void RecuperarTabelaClientesEmMemoria(NpgsqlConnection conexaoBanco, List<Log> logs)
        {
            using var transaction = conexaoBanco.BeginTransaction();

            foreach (var op in logs)
            {
                string sql = "";

                if (op.Operacao == "INSERT")
                    sql = "INSERT INTO clientes_em_memoria (id, nome, saldo) VALUES (@id, @nome, @saldo) ON CONFLICT (id) DO NOTHING;";
                else if (op.Operacao == "UPDATE")
                    sql = "UPDATE clientes_em_memoria SET nome = @nome, saldo = @saldo WHERE id = @id;";
                else if (op.Operacao == "DELETE")
                    sql = "DELETE FROM clientes_em_memoria WHERE id = @id;";

                using var cmdRedo = new NpgsqlCommand(sql, conexaoBanco);
                cmdRedo.Parameters.AddWithValue("id", op.IdCliente ?? (object)DBNull.Value);

                if (op.Operacao != "DELETE")
                {
                    cmdRedo.Parameters.AddWithValue("nome", op.Nome ?? (object)DBNull.Value);
                    cmdRedo.Parameters.AddWithValue("saldo", op.Saldo ?? (object)DBNull.Value);
                }

                int rowsAffected = cmdRedo.ExecuteNonQuery();
                Console.WriteLine($"Operação {op.Operacao} no registro {op.IdCliente} executada");

                if (op.Operacao != "DELETE")
                {
                    Console.WriteLine($"Dados aplicados -> Id: {op.IdCliente}, Nome: {op.Nome}, Saldo: {op.Saldo}");
                }
                else
                {
                    Console.WriteLine($"Dados removidos -> Id: {op.IdCliente}");
                }
            }

            transaction.Commit();
        }

        #endregion
    }

}