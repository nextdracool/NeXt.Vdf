using System.Collections.Generic;

namespace NeXt.Vdf
{
    /// <summary>
    /// A VdfValue that represents a table containing other VdfValues
    /// </summary>
    public sealed class VdfTable : VdfValue, IList<VdfValue>
    {
        public VdfTable(string name) : base(name)
        {

        }

        private List<VdfValue> values = new List<VdfValue>();
        private Dictionary<string, VdfValue> valuelookup = new Dictionary<string,VdfValue>();

        internal VdfValue owner;

        public int IndexOf(VdfValue item)
        {
            return values.IndexOf(item);
        }

        public void Insert(int index, VdfValue item)
        {
            item.Parent = this;

            values.Insert(index, item);
            valuelookup.Add(item.Name, item);
        }

        public void RemoveAt(int index)
        {
            var val = values[index];
            values.RemoveAt(index);
            valuelookup.Remove(val.Name);
            
        }

        public VdfValue this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                if(values[index].Name != value.Name)
                {
                    valuelookup.Remove(values[index].Name);
                    valuelookup.Add(value.Name, value);
                }
                else
                {
                valuelookup[value.Name] = value;
                }
                values[index] = value;
            }
        }

        public VdfValue this[string name]
        {
            get
            {
                return valuelookup[name];
            }
        }

        public void Add(VdfValue item)
        {
            item.Parent = this;

            values.Add(item);
            valuelookup.Add(item.Name, item);
        }

        public void Clear()
        {
            values.Clear();
            valuelookup.Clear();
        }

        public bool Contains(VdfValue item)
        {
            return valuelookup.ContainsKey(item.Name);
        }

        public void CopyTo(VdfValue[] array, int arrayIndex)
        {
            values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return values.Count; }
        }

        public bool Remove(VdfValue item)
        {
            valuelookup.Remove(item.Name);
            return values.Remove(item);
        }

        public IEnumerator<VdfValue> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        bool ICollection<VdfValue>.IsReadOnly
        {
            get { return false; }
        }

    }
}
