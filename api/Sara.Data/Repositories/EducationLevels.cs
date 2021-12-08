using Sara.Core;
using Sara.Core.Interfaces;

namespace Sara.Data.Repositories
{
    public class EducationLevels : EFCoreRepository<EducationLevel>, IEducationLevels
    {
        public EducationLevels(SaraContext context) : base(context) { }
    }
}
