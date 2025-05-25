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

            using var consulta = BuscarLogs(conexaoBanco);
            var logs = LerLogs(consulta);


            Console.WriteLine("Transações que devem realizar REDO:");
            foreach (var operacao in logs)
                Console.WriteLine($"id: {operacao.Id}, transação: {operacao.Operacao}, id-cliente: {operacao.IdCliente}, nome: {operacao.Nome}, saldo: {operacao.Saldo}");

            RecuperarTabelaClientesEmMemoria(conexaoBanco, logs);

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

        private void RecuperarTabelaClientesEmMemoria(NpgsqlConnection conexaoBanco, List<Log> logs)
        {
            CriarTabelaSeNaoExistir(conexaoBanco);

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
                cmdRedo.Parameters.AddWithValue("id", op.IdCliente);

                if (op.Operacao != "DELETE")
                {
                    cmdRedo.Parameters.AddWithValue("nome", op.Nome ?? (object)DBNull.Value);
                    cmdRedo.Parameters.AddWithValue("saldo", op.Saldo);
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

        private void CriarTabelaSeNaoExistir(NpgsqlConnection conexaoBanco)
        {
            string sql = @"
            CREATE UNLOGGED TABLE IF NOT EXISTS clientes_em_memoria (
            id SERIAL PRIMARY KEY,
            nome TEXT,
            saldo NUMERIC
        );";

            using var cmd = new NpgsqlCommand(sql, conexaoBanco);
            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
