namespace Vita.Contracts
{
    public abstract class Document : ValueObject
    {
        public string TemplateId { get; set; }
    }
}