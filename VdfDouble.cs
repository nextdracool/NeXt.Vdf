using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Vdf
{

    /// <summary>
    /// A VdfValue that represents a double
    /// </summary>
    public sealed class VdfDouble : VdfValue
    {
        public VdfDouble(string name) : base(name)
        {
            Type = VdfValueType.Double;
        }

        public VdfDouble(string name, double value) : this(name)
        {
            Content = value;
        }

        public double Content { get; set; }
    }
}
