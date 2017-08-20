using Xamarin.Forms;

namespace App.Template.XForms.Core.Forms.Triggers
{
    public class ClearEntryTrigger : TriggerAction<Button>
    {
        public Entry TargetEntry { get; set; }

        protected override void Invoke(Button sender)
        {
            if (TargetEntry != null)
                TargetEntry.Text = string.Empty;
        }
    }
}
