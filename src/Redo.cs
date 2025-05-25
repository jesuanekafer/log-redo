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


    }
}
