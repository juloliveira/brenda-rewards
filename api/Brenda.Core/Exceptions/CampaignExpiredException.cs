using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Core.Exceptions
{
    public abstract class BrendaException : Exception { }

    public class CampaignInvalid : BrendaException { }

    public class CampaignExpiredException : BrendaException { }

    public class CampaignGeoValidationException : BrendaException { }

    public class TicketAlreadyRewardedException : BrendaException { }


}
