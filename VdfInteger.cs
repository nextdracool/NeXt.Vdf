namespace NeXt.Vdf
{
    /// <summary>
    /// A VdfValue that represents an integer
    /// </summary>
    public sealed class VdfInteger : VdfValue
    {
        public VdfInteger(string name) : base(name)
        {
            Type = VdfValueType.Integer;
        }

        public VdfInteger(string name, int value) : this(name)
        {
            Content = value;
        }

        public int Content { get; set; }
    }
}
