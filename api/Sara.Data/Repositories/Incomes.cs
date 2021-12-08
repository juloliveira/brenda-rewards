using Sara.Core;
using Sara.Core.Interfaces;

namespace Sara.Data.Repositories
{
    public class Incomes : EFCoreRepository<Income>, IIncomes
    {
        public Incomes(SaraContext context) : base(context) { }
    }
}
