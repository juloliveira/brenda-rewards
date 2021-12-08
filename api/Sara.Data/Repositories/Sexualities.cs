using Sara.Core;
using Sara.Core.Interfaces;

namespace Sara.Data.Repositories
{
    public class Sexualities : EFCoreRepository<Sexuality>, ISexualities
    {
        public Sexualities(SaraContext context) : base(context) { }
    }
}
