using System.Text;
using System.IO;
using System.Globalization;

namespace NeXt.Vdf
{
    /// <summary>
    /// Class that handles serialization of VdfValue objects into Vdf formatted strings
    /// </summary>
    public class VdfSerializer
    {
        private delegate void OnStep(string text);

        /// <summary>
        /// Creates a VdfSerializer object
        /// </summary>
        /// <param name="value">the VdfValue to serialize</param>
        public VdfSerializer(VdfValue value)
        {
            root = value;
        }

        private VdfValue root;
        private int indentLevel = 0;

        private string EscapeString(string v)
        {
            return v.Replace("\\", @"\\").Replace("\t", @"\t").Replace("\n", @"\n").Replace("\"", @"\""");
        }

        private void WriteString(OnStep step, string text, bool escape)
        {
            step("\"");
            if(escape)
            {
                step(EscapeString(text));
            }
            else
            {
                step(text);
            }
            step("\"");
        }

        private void WriteToken(OnStep step,  string value)
        {
            step("[$");
            step(EscapeString(value));
            step("]");
        }

        private void RunSerialization(OnStep onStep, VdfValue current)
        {
            if(current.Comments.Count > 0)
            {
                foreach(var s in current.Comments)
                {
                    onStep("  ".Repeat(indentLevel));
                    onStep("//");
                    onStep(EscapeString(s));
                    onStep("\n");
                }
            }

            onStep("  ".Repeat(indentLevel));
            WriteString(onStep, current.Name, true);
            switch(current.Type)
            {
                case VdfValueType.String:
                {
                    onStep(" ");
                    WriteString(onStep, (current as VdfString).Content, true);
  
                    onStep("\n");
                    break;
                }
                case VdfValueType.Integer:
                {
                    onStep(" ");
                    WriteString(onStep, (current as VdfInteger).Content.ToString(CultureInfo.InvariantCulture), false);

                    onStep("\n");
                    break;
                }
                case VdfValueType.Double:
                {
                    onStep(" ");
                    WriteString(onStep, (current as VdfDouble).Content.ToString(CultureInfo.InvariantCulture), false);
                    onStep("\n");
                    break;
                }
                case VdfValueType.Table:
                {

                    onStep("\n");
                    onStep("  ".Repeat(indentLevel));
                    onStep("{\n");
                    indentLevel++;
                    foreach(var v in current as VdfTable)
                    {
                        RunSerialization(onStep, v);
                    }
                    indentLevel--;
                    onStep("  ".Repeat(indentLevel));
                    onStep("}\n");
                    break;
                }
            }
        }

        /// <summary>
        /// Serialize the object to string
        /// </summary>
        /// <returns>a string representing the VdfValue</returns>
        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            RunSerialization((s) => sb.Append(s), root);

            return sb.ToString();
        }

        /// <summary>
        /// Serialize the object to a file
        /// </summary>
        /// <param name="filePath">full path to the file to serialize into</param>
        public void Serialize(string filePath)
        {
            Serialize(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// Serialize the object to a file using the given encoding
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        public void Serialize(string filePath, Encoding encoding)
        {
            StreamWriter writer = new StreamWriter(filePath, false, encoding);
            RunSerialization((s) => writer.Write(s), root);
        }
    }
}
