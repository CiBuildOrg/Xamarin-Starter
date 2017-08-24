using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Interaction;
using App.Template.XForms.Core.Utils.Validation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Target;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace App.Template.XForms.iOS.Infrastructure.Validation
{
    public class MvxTouchValidationService : MvxValidationService
    {
        private Dictionary<string, List<UIView>> _sourceBindingRelationships;
        private UIView _firstText;
        private nfloat? _defaultWidth;
        private CGColor _defaultColor;

        readonly IInteractiveAlerts _toastService;

        private static readonly PropertyInfo UiTextViewGetter;
        private static readonly PropertyInfo UiTextFieldGetter;

        static MvxTouchValidationService()
        {
            var bindingType = typeof(MvxUITextViewTextTargetBinding);
            UiTextViewGetter = bindingType.GetProperty("View",
                                                           BindingFlags.NonPublic | BindingFlags.GetProperty |
                                                           BindingFlags.Instance);
            bindingType = typeof(MvxUITextFieldTextTargetBinding);
            UiTextFieldGetter = bindingType.GetProperty("View",
                                                           BindingFlags.NonPublic | BindingFlags.GetProperty |
                                                           BindingFlags.Instance);
        }

        public MvxTouchValidationService(IInteractiveAlerts toastService, IValidator validator, IMvxMessenger messenger)
            : base(validator, messenger)
        {
            _toastService = toastService;
        }

        public override void SetupForValidation(IMvxBindingContext context, IMvxViewModel viewModel)
        {
            _sourceBindingRelationships = new Dictionary<string, List<UIView>>();
            base.SetupForValidation(context, viewModel);
        }

        protected override void Validated(IErrorCollection errors)
        {
            if (_firstText != null)
                _firstText.BecomeFirstResponder();
            _toastService.ShowAlert(new InteractiveAlertConfig
            {
                CancelButton = new InteractiveActionButton(),
               IsCancellable = true,
               Message = errors.Collect(),
               OkButton = new InteractiveActionButton(),
               Style = InteractiveAlertStyle.Error,
               Title = "Error"
            });
        }

        protected override void Validate(IErrorInfo errorInfo)
        {
            List<UIView> texts;
            if (_sourceBindingRelationships.TryGetValue(errorInfo.MemberName, out texts))
            {
                foreach (var editText in texts)
                {
                    if (_firstText == null)
                        _firstText = editText;
                    editText.Layer.BorderColor = UIColor.Red.CGColor;
                    editText.Layer.BorderWidth = 3.0f;
                }
            }
        }

        protected override bool Validating()
        {
            if (_defaultColor == null)
            {
                var firstRelationship = _sourceBindingRelationships.FirstOrDefault();
                var firstView = firstRelationship.Value?.FirstOrDefault();
                if (firstView != null)
                {
                    _defaultColor = firstView.Layer.BorderColor;
                    _defaultWidth = firstView.Layer.BorderWidth;
                }
            }

            if (_sourceBindingRelationships == null) return false;
            foreach (var textView in _sourceBindingRelationships.SelectMany(sourceBindingRelationship => sourceBindingRelationship.Value))
            {
                textView.Layer.BorderColor = _defaultColor;
                textView.Layer.BorderWidth = _defaultWidth.GetValueOrDefault();
            }
            return true;
        }

        protected override void ProcessSourceAndTarget(string sourcePath, object target)
        {
            var boundView = target as UIView;
            if (boundView == null) return;

            List<UIView> collection;
            if (!_sourceBindingRelationships.TryGetValue(sourcePath, out collection))
            {
                collection = new List<UIView>();
                _sourceBindingRelationships[sourcePath] = collection;
            }
            collection.Add(boundView);
        }

        protected override object GetTargetFromBinding(object targetBinding)
        {
            object view = null;
            var textViewBinding = targetBinding as MvxUITextViewTextTargetBinding;
            if (textViewBinding != null)
            {
                var viewView = UiTextViewGetter.GetValue(textViewBinding) as UITextView;
                if (viewView != null)
                {

                }
                view = viewView;
            }

            var textFieldBinding = targetBinding as MvxUITextFieldTextTargetBinding;
            if (textFieldBinding != null)
            {
                var textField = UiTextFieldGetter.GetValue(textFieldBinding) as UITextField;
                if (textField != null)
                {
                    if (textField.Placeholder.IsNotNullOrEmpty() && !textField.Placeholder.EndsWith("*"))
                    {
                        textField.Placeholder += "*";
                    }
                }
                view = textField;
            }
            return view;
        }
    }
}