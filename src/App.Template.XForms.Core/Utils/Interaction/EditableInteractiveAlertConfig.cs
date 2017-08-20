namespace App.Template.XForms.Core.Utils.Interaction
{
    public class EditableInteractiveAlertConfig : InteractiveAlertConfig
    {
        public bool SingleLine { get; set; } = true;

        public string Text { get; set; }

        public string Placeholder { get; set; }
    }
}