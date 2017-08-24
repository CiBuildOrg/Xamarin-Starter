using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Text;
using Android.Widget;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Validation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Android.Infrastructure.Validation
{
    public class MvxAndroidValidationService : MvxValidationService
    {
        private static readonly PropertyInfo TextViewGetter;

        private Dictionary<string, List<TextView>> _sourceBindingRelationships;

        static MvxAndroidValidationService()
        {
            Initialize();

            var textBinding = typeof(MvxTextViewTextTargetBinding);
            TextViewGetter = textBinding.GetProperty("TextView",
                                                     BindingFlags.NonPublic | BindingFlags.GetProperty |
                                                     BindingFlags.Instance);
        }

        public MvxAndroidValidationService(IValidator validator, IMvxMessenger messenger)
            : base(validator, messenger)
        {
        }

        public override void SetupForValidation(IMvxBindingContext context, IMvxViewModel viewModel)
        {
            _sourceBindingRelationships = new Dictionary<string, List<TextView>>();
            base.SetupForValidation(context, viewModel);
        }

        private TextView _firstText;

        protected override void Validate(IErrorInfo errorInfo)
        {
            List<TextView> texts;
            if (_sourceBindingRelationships.TryGetValue(errorInfo.MemberName, out texts))
            {
                foreach (var editText in texts)
                {
                    
                    if (_firstText == null)
                        _firstText = editText;
                    editText.ErrorFormatted =
#pragma warning disable 618
                        Html.FromHtml($"<font color='black'>{errorInfo.Message}</font>");
#pragma warning restore 618
                }
            }
        }

        protected override void Validated(IErrorCollection errors)
        {
            if (_firstText != null)
            {
                _firstText.RequestFocus();
                _firstText = null;
            }
        }

        protected override bool Validating()
        {
            if (_sourceBindingRelationships == null) return false;
            foreach (
                var textView in
                    _sourceBindingRelationships.SelectMany(sourceBindingRelationship => sourceBindingRelationship.Value))
            {
                textView.Error = null;
            }
            return true;
        }

        protected override void ProcessSourceAndTarget(string sourcePath, object target)
        {
            List<TextView> collection;
            var boundView = target as TextView;
            if (boundView == null) return;

            if (!_sourceBindingRelationships.TryGetValue(sourcePath, out collection))
            {
                collection = new List<TextView>();
                _sourceBindingRelationships[sourcePath] = collection;
            }
            collection.Add(boundView);

            if (Validator.IsRequired(ViewModel, sourcePath))
            {
                if (boundView.Hint.IsNotNullOrEmpty() && !boundView.Hint.EndsWith("*"))
                    boundView.Hint += "*";
            }
        }

        protected override object GetTargetFromBinding(object targetBinding)
        {
            object boundView = null;
            var textBinding = targetBinding as MvxTextViewTextTargetBinding;
            if (textBinding != null)
            {
                if (TextViewGetter != null)
                {
                    boundView = TextViewGetter.GetValue(textBinding, null);
                }
                else
                {
                    MvxTrace.Error("TextView on {0} cannot be found.", typeof(MvxTextViewTextTargetBinding));
                }
            }
            return boundView;
        }
    }
}