namespace Discounts.Server.Model
{
    public class Code
    {
        public Code(string value)
        {
            Value = value;
            IsUsed = false;
        }

        public Code (string value, bool isUsed)
        {
            Value = value;
            IsUsed = isUsed;
        }

        public string Value { get; private set; }
        public bool IsUsed { get; private set; }

        public void Void()
        {
            IsUsed = true;
        }
    }
}
