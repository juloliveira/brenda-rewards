namespace Brenda.Core.Validations
{
    public class CampaignErrorMessage
    {
        public CampaignErrorMessage(string tag, string fieldName, string message)
        {
            Tag = tag;
            FieldName = fieldName;
            Message = message;
        }

        public string Tag { get; set; }

        public string FieldName { get; set; }

        public string Message { get; set; }
    }
}
