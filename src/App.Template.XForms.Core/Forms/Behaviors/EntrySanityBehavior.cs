using Xamarin.Forms;

namespace App.Template.XForms.Core.Forms.Behaviors
{
    public class EntrySanityBehaviour : Behavior<Entry>
    {
        public bool DoRemoveWhiteSpace { get; set; }
        private Entry _context;

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(_context);
            _context = bindable;
            _context.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            _context.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(_context);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DoRemoveWhiteSpace)
                RemoveWhiteSpace(e.NewTextValue);
        }

        private void RemoveWhiteSpace(string input)
        {
            _context.Text = input.Replace(" ", "");
        }
    }
}
