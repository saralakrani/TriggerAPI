using static Domain.Modal;

namespace AppLayer
{
    public interface IRepository
    {
        Task MultiGetBoomBatchTransactionIds(string BoomBulkBatchId, string ipAddress, string userAgent);
    }
}
