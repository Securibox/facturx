namespace Securibox.FacturX.Models.EN16931
{
    public class LineItemAttribute
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        internal LineItemAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
