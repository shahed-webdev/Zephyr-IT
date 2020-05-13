using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class PurchasePaymentRepository : Repository<PurchasePayment>, IPurchasePaymentRepository
    {
        public PurchasePaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetNewSnAsync()
        {
            var sn = 0;
            if (await Context.PurchasePayment.AnyAsync().ConfigureAwait(false))
            {
                sn = await Context.PurchasePayment.MaxAsync(p => p == null ? 0 : p.ReceiptSn).ConfigureAwait(false);
            }

            return sn + 1;
        }
    }
}