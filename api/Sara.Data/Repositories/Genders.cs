using Sara.Core;
using Sara.Core.Interfaces;

namespace Sara.Data.Repositories
{
    public class Genders : EFCoreRepository<GenderIdentity>, IGenders
    {
        public Genders(SaraContext context) : base(context) { }
    }
}
