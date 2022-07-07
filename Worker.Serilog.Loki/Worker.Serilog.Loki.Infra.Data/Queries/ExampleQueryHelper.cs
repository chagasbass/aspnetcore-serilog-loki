using Z.Dapper.Plus;

namespace Worker.Serilog.Loki.Infra.Data.Queries
{
    /*Esta classe pode ser apagada após a geração do template
     * 
     * 
     */

    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public static class ExampleQueryHelper
    {
        public const string MappingName = "Pessoa_Exemplo_Mapping";

        /// <summary>
        /// Método que será usado para efetuar o mapeamento da entidade criada com a tabela quando
        /// as propriedades da entidade são diferentes dos nomes dos campos da tabela
        /// </summary>
        public static void EfetuarMapeamentoDeDados()
        {
            DapperPlusManager.Entity<Pessoa>(MappingName)
                             .Table("Nome_da_tabela")
                             .Map(x => x.Id, "ID")
                             .Map(x => x.Nome, "NOME_PESSOA")
                             .Map(x => x.Email, "EMAIL_PESSOA");
        }
    }
}
