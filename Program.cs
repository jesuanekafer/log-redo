using trabalho_bd_log.src;

namespace trabalho_bd_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var redo = new Redo();
            redo.RecuperarLog();
        }
    }
}
