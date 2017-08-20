namespace App.Template.XForms.Core.Utils.Interaction
{
    public class InteractiveAlertConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";

        public static string DefaultCancelText { get; set; } = "Cancel";

        private InteractiveActionButton _okButton;
        private InteractiveActionButton _cancelButton;

        public InteractiveAlertStyle Style { get; set; } = InteractiveAlertStyle.Success;

        public string Title { get; set; }

        public string Message { get; set; }


        public InteractiveActionButton OkButton
        {
            get => _okButton;
            set
            {
                _okButton = value;
                if (string.IsNullOrEmpty(_okButton.Title))
                {
                    _okButton.Title = DefaultOkText;
                }
            }
        }

        public InteractiveActionButton CancelButton
        {
            get => _cancelButton;

            set
            {
                _cancelButton = value;
                if (string.IsNullOrEmpty(_cancelButton.Title))
                {
                    _cancelButton.Title = DefaultCancelText;
                }
            }
        }

        public bool IsCancellable { get; set; } = true;
    }
}