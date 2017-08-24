using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation
{
    public class ErrorCollection : Collection<IErrorInfo>, IErrorCollection
    {
        public ErrorCollection(IList<IErrorInfo> result) : base(result)
        {
            
        }

        public bool IsValid { get { return !this.Any(); } }

        public string Format()
        {
            return string.Join("\r\n", this.Select(t => t.Message));
        }

        public string Collect()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var error in this.Items)
            {
                builder.AppendLine($"{error.Message} - {error.Message}");
            }

            return builder.ToString();
        }
    }
}