
using Dapper;
using Worker.Serilog.Loki.Infra.Data.DataContexts;
using Worker.Serilog.Loki.Infra.Data.Queries;
using Z.Dapper.Plus;

namespace Worker.Serilog.Loki.Infra.Data.Repositories
{
    public interface IExampleRepository
    {
        Task AddAssync(IEnumerable<object> data);
        Task<IEnumerable<object>> GetAllAssync();
    }

    /// <summary>
    /// Classe de exemplo com a implementação do Repository pattern usando operaçoes de bulk para Inserção
    /// A classe pode ser excluída quando o template for usado
    /// </summary>
    public class ExampleRepository : IExampleRepository
    {
        private readonly DataContext _dataContext;

        public ExampleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;

            //quando há mapeamento de entidades
            ExampleQueryHelper.EfetuarMapeamentoDeDados();
        }
        public async Task AddAssync(IEnumerable<object> data)
        {
            using var conexao = _dataContext.AbrirConexao();


            await conexao.BulkActionAsync(x => x.BulkInsert(data));

        }

        public async Task<IEnumerable<object>> GetAllAssync()
        {
            using var conexao = _dataContext.AbrirConexao();
            var dados = await conexao.QueryAsync<object>("minhaquery");

            return dados;
        }
    }
}
