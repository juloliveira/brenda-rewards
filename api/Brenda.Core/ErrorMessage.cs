namespace Brenda.Core
{
    public class ErrorMessage : Entity
    {
        public string Field { get; set; }
        public string Lang { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
