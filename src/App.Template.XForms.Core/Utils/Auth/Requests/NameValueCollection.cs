using System.Collections.Generic;

namespace App.Template.XForms.Core.Utils.Auth.Requests
{
    public class NameValueCollection : Dictionary<string, string>
    {
        public NameValueCollection()
        {
        }

        public NameValueCollection(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public NameValueCollection(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public NameValueCollection(int capacity) : base(capacity)
        {
        }

        public NameValueCollection(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public NameValueCollection(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }
    }
}