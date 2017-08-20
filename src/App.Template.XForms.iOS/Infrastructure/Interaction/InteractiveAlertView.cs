using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.Template.XForms.Core.Utils.Interaction;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.Template.XForms.iOS.Infrastructure.Interaction
{
    public class InteractiveAlertView : UIViewController
    {
        private static readonly nfloat KCircleHeightBackground = 62.0f;

        private readonly SclAppearance _appearance;

        [SuppressMessage("ReSharper", "NotAccessedField.Local")] private InteractiveAlertView _selfReference;

        private NSObject _keyboardWillShowToken;

        private NSObject _keyboardWillHideToken;

        public class SclAppearance
        {
            public nfloat DefaultShadowOpacity { get; set; } = 0.7f;
            public nfloat CircleTopPosition { get; set; } = 0.0f;
            public nfloat CircleBackgroundTopPosition { get; set; } = 6.0f;
            public nfloat CircleHeight { get; set; } = 56.0f;
            public nfloat CircleIconHeight { get; set; } = 20.0f;
            public nfloat TitleTop { get; set; } = 30.0f;
            public nfloat TitleHeight { get; set; } = 25.0f;
            public nfloat TitleMinimumScaleFactor { get; set; } = 1.0f;
            public nfloat WindowWidth { get; set; } = 240.0f;
            public nfloat WindowHeight { get; set; } = 178.0f;
            public nfloat TextHeight { get; set; } = 90.0f;
            public nfloat TextFieldHeight { get; set; } = 45.0f;
            public nfloat TextViewdHeight { get; set; } = 80.0f;
            public nfloat ButtonHeight { get; set; } = 45.0f;
            public UIColor CircleBackgroundColor { get; set; } = UIColor.White;
            public UIColor ContentViewColor { get; set; } = UIColor.White;
            // 0xCCCCCC
            public UIColor ContentViewBorderColor { get; set; } = UIColor.FromRGB(204, 204, 204);
            // 0x4D4D4D
            public UIColor TitleColor { get; set; } = UIColor.FromRGB(77, 77, 77);

            // Fonts
            public UIFont TitleFont { get; set; } = UIFont.SystemFontOfSize(20);
            public UIFont TextFont { get; set; } = UIFont.SystemFontOfSize(14);
            public UIFont ButtonFont { get; set; } = UIFont.SystemFontOfSize(14);

            // UI Options
            public bool DisableTapGesture { get; set; } = false;
            public bool ShowCloseButton { get; set; } = true;
            public bool ShowCircularIcon { get; set; } = true;
            // Set this false to 'Disable' Auto hideView when SCLButton is tapped
            public bool ShouldAutoDismiss { get; set; } = true;
            public nfloat ContentViewCornerRadius { get; set; } = 5.0f;
            public nfloat FieldCornerRadius { get; set; } = 3.0f;
            public nfloat ButtonCornerRadius { get; set; } = 3.0f;
            public bool DynamicAnimatorActive { get; set; } = false;

            // Actions
            public bool HideWhenBackgroundViewIsTapped { get; set; } = false;

            public void SetWindowHeight(nfloat kWindowHeight)
            {
                WindowHeight = kWindowHeight;
            }

            public void SetTextHeight(nfloat kTextHeight)
            {
                TextHeight = kTextHeight;
            }
        }

        // UI Colour
        public UIColor ViewColor { get; set; }

        // UI Options
        public UIColor IconTintColor { get; set; }
        public UIView CustomSubview { get; set; }

        // Members declaration
        private readonly UIView _baseView = new UIView();
        private readonly UILabel _labelTitle = new UILabel();
        private readonly UITextView _viewText = new UITextView();
        private readonly UIView _contentView = new UIView();
        private readonly UIView _circleBg = new UIView(new CGRect(0, 0, KCircleHeightBackground, KCircleHeightBackground));
        private readonly UIView _circleView = new UIView();
        private UIView _circleIconView;
        private double _duration;
        private NSTimer _durationStatusTimer;
        private NSTimer _durationTimer;
        private Action _dismissBlock;
        private List<UITextField> Inputs { get; } = new List<UITextField>();
        private List<UITextView> Input { get; } = new List<UITextView>();
        private List<SclButton> Buttons { get; } = new List<SclButton>();
        private CGPoint? _tmpContentViewFrameOrigin;
        private CGPoint? _tmpCircleViewFrameOrigin;
        private bool _keyboardHasBeenShown;
        // DynamicAnimator function
        private UIDynamicAnimator _animator;
        private UISnapBehavior _snapBehavior;

        public InteractiveAlertView(SclAppearance appearance)
            : base(null, null)
        {
            _appearance = appearance;
            Setup();
        }

        public InteractiveAlertView(string nibNameOrNil, NSBundle bundle, SclAppearance appearance) : base(nibNameOrNil, bundle)
        {
            _appearance = appearance;
            Setup();
        }

        public InteractiveAlertView(string nibNameOrNil, NSBundle bundle) : this(nibNameOrNil, bundle, new SclAppearance())
        {

        }

        public InteractiveAlertView() : this(new SclAppearance())
        {

        }

        private void Setup()
        {
            // Set up main view
            View.Frame = UIScreen.MainScreen.Bounds;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            View.BackgroundColor = new UIColor(red: 0, green: 0, blue: 0, alpha: _appearance.DefaultShadowOpacity);
            View.AddSubview(_baseView);
            // Base View
            _baseView.Frame = View.Frame;
            _baseView.AddSubview(_contentView);
            // Content View
            _contentView.Layer.CornerRadius = _appearance.ContentViewCornerRadius;
            _contentView.Layer.MasksToBounds = true;
            _contentView.Layer.BorderWidth = 0.5f;
            _contentView.AddSubview(_labelTitle);
            _contentView.AddSubview(_viewText);
            // Circle View
            _circleBg.BackgroundColor = _appearance.CircleBackgroundColor;
            _circleBg.Layer.CornerRadius = _circleBg.Frame.Size.Height / 2f;
            _baseView.AddSubview(_circleBg);
            _circleBg.AddSubview(_circleView);
            var x = (KCircleHeightBackground - _appearance.CircleHeight) / 2f;
            _circleView.Frame = new CGRect(x, x + _appearance.CircleTopPosition, _appearance.CircleHeight, _appearance.CircleHeight);
            _circleView.Layer.CornerRadius = _circleView.Frame.Size.Height / 2f;
            // Title
            _labelTitle.Lines = 0;
            _labelTitle.TextAlignment = UITextAlignment.Center;
            _labelTitle.Font = _appearance.TitleFont;
            if (_appearance.TitleMinimumScaleFactor < 1)
            {
                _labelTitle.MinimumScaleFactor = _appearance.TitleMinimumScaleFactor;
                _labelTitle.AdjustsFontSizeToFitWidth = true;
            }
            _labelTitle.Frame = new CGRect(12, _appearance.TitleTop, _appearance.WindowWidth - 24, _appearance.TitleHeight);
            // View text
            _viewText.Editable = false;
            _viewText.TextAlignment = UITextAlignment.Center;
            _viewText.TextContainerInset = UIEdgeInsets.Zero;
            _viewText.TextContainer.LineFragmentPadding = 0;
            _viewText.Font = _appearance.TextFont;
            // Colours
            _contentView.BackgroundColor = _appearance.ContentViewColor;
            _viewText.BackgroundColor = _appearance.ContentViewColor;
            _labelTitle.TextColor = _appearance.TitleColor;
            _viewText.TextColor = _appearance.TitleColor;
            _contentView.Layer.BorderColor = _appearance.ContentViewBorderColor.CGColor;
            //Gesture Recognizer for tapping outside the textinput
            if (_appearance.DisableTapGesture == false)
            {
                var tapGesture = new UITapGestureRecognizer(Tapped) {NumberOfTapsRequired = 1};
                View.AddGestureRecognizer(tapGesture);
            }
        }

        public void SetTitle(string title)
        {
            _labelTitle.Text = title;
        }

        public void SetSubTitle(string subTitle)
        {
            _viewText.Text = subTitle;
        }

        public void SetDismissBlock(Action dismissBlock)
        {
            _dismissBlock = dismissBlock;
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            var rv = UIApplication.SharedApplication.KeyWindow;
            var sz = rv.Frame.Size;
            var frame = rv.Frame;
            frame.Width = sz.Width;
            frame.Height = sz.Height;

            // Set background frame
            View.Frame = frame;
            nfloat hMargin = 12f;

            // get actual height of title text
            nfloat titleActualHeight = 0f;
            if (!string.IsNullOrEmpty(_labelTitle.Text))
            {
                titleActualHeight = SclAlertViewExtension.HeightWithConstrainedWidth(_labelTitle.Text, _appearance.WindowWidth - hMargin * 2f, _labelTitle.Font) + 10f;
                // get the larger height for the title text
                titleActualHeight = (titleActualHeight > _appearance.TitleHeight ? titleActualHeight : _appearance.TitleHeight);
            }

            // computing the right size to use for the textView
            var maxHeight = sz.Height - 100f; // max overall height
            nfloat consumedHeight = 0f;
            consumedHeight += (titleActualHeight > 0 ? _appearance.TitleTop + titleActualHeight : hMargin);
            consumedHeight += 14;
            consumedHeight += _appearance.ButtonHeight * Buttons.Count;
            consumedHeight += _appearance.TextFieldHeight * Inputs.Count;
            consumedHeight += _appearance.TextViewdHeight * Input.Count;
            var maxViewTextHeight = maxHeight - consumedHeight;
            var viewTextWidth = _appearance.WindowWidth - hMargin * 2f;
            nfloat viewTextHeight;

            // Check if there is a custom subview and add it over the textview
            if (CustomSubview != null)
            {
                viewTextHeight = (nfloat)Math.Min(CustomSubview.Frame.Height, maxViewTextHeight);
                _viewText.Text = string.Empty;
                _viewText.AddSubview(CustomSubview);
            }
            else
            {
                // computing the right size to use for the textView
                var suggestedViewTextSize = _viewText.SizeThatFits(new CGSize(viewTextWidth, nfloat.MaxValue));
                viewTextHeight = (nfloat)Math.Min(suggestedViewTextSize.Height, maxViewTextHeight);
                // scroll management
                _viewText.ScrollEnabled = suggestedViewTextSize.Height > maxViewTextHeight;
            }

            var windowHeight = consumedHeight + viewTextHeight;
            // Set frames
            var x = (sz.Width - _appearance.WindowWidth) / 2f;
            var y = (sz.Height - windowHeight - (_appearance.CircleHeight / 8)) / 2f;
            _contentView.Frame = new CGRect(x, y, _appearance.WindowWidth, windowHeight);
            _contentView.Layer.CornerRadius = _appearance.ContentViewCornerRadius;
            y -= KCircleHeightBackground * 0.6f;
            x = (sz.Width - KCircleHeightBackground) / 2f;
            _circleBg.Frame = new CGRect(x, y + _appearance.CircleBackgroundTopPosition, KCircleHeightBackground, KCircleHeightBackground);

            //adjust Title frame based on circularIcon show/hide flag
            var titleOffset = _appearance.ShowCircularIcon ? 0.0f : -12.0f;
            _labelTitle.Frame.Offset(0, titleOffset);

            // Subtitle
            y = titleActualHeight > 0f ? _appearance.TitleTop + titleActualHeight + titleOffset : hMargin;
            _viewText.Frame = new CGRect(hMargin, y, _appearance.WindowWidth - hMargin * 2f, _appearance.TextHeight);
            _viewText.Frame = new CGRect(hMargin, y, viewTextWidth, viewTextHeight);
            // Text fields
            y += viewTextHeight + 14.0f;
            foreach (var txt in Inputs)
            {
                txt.Frame = new CGRect(hMargin, y, _appearance.WindowWidth - hMargin * 2, 30);
                txt.Layer.CornerRadius = _appearance.FieldCornerRadius;
                y += _appearance.TextFieldHeight;
            }
            foreach (var txt in Input)
            {
                txt.Frame = new CGRect(hMargin, y, _appearance.WindowWidth - hMargin * 2f, 70);
                //txt.layer.cornerRadius = fieldCornerRadius
                y += _appearance.TextViewdHeight;
            }

            // Buttons
            foreach (var btn in Buttons)
            {
                btn.Frame = new CGRect(hMargin, y, _appearance.WindowWidth - hMargin * 2, 35);
                btn.Layer.CornerRadius = _appearance.ButtonCornerRadius;
                y += _appearance.ButtonHeight;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _keyboardWillShowToken = UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShow);
            _keyboardWillHideToken = UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHide);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _keyboardWillShowToken?.Dispose();
            _keyboardWillHideToken?.Dispose();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (evt.TouchesForView(View)?.Count > 0)
            {
                View.EndEditing(true);
            }
        }

        public UITextField AddTextField(string title)
        {
            // Update view height
            _appearance.SetWindowHeight(_appearance.WindowHeight + _appearance.TextFieldHeight);
            // Add text field
            var txt = new UITextField
            {
                BorderStyle = UITextBorderStyle.RoundedRect,
                Font = _appearance.TextFont,
                AutocapitalizationType = UITextAutocapitalizationType.Words,
                ClearButtonMode = UITextFieldViewMode.WhileEditing
            };
            txt.Layer.MasksToBounds = true;
            txt.Layer.BorderWidth = 1.0f;
            if (!string.IsNullOrEmpty(title))
            {
                txt.Placeholder = title;
            }
            _contentView.AddSubview(txt);
            Inputs.Add(txt);
            return txt;
        }

        public UITextView AddTextView()
        {
            // Update view height
            _appearance.SetWindowHeight(_appearance.WindowHeight + _appearance.TextViewdHeight);
            // Add text view
            var txt = new UITextView {Font = _appearance.TextFont};
            // No placeholder with UITextView but you can use KMPlaceholderTextView library 
            txt.Layer.MasksToBounds = true;
            txt.Layer.BorderWidth = 1.0f;
            txt.Layer.CornerRadius = 4f;
            _contentView.AddSubview(txt);
            Input.Add(txt);
            return txt;
        }

        public SclButton AddButton(string title, Action action, UIColor backgroundColor = null, UIColor textColor = null, bool showDurationStatus = false)
        {
            var btn = AddButton(title, backgroundColor, textColor, showDurationStatus);
            btn.ActionType = SclActionType.Closure;
            btn.Action = action;
            btn.AddTarget(ButtonTapped, UIControlEvent.TouchUpInside);
            btn.AddTarget(ButtonTapDown, UIControlEvent.TouchDown | UIControlEvent.TouchDragEnter);
            btn.AddTarget(ButtonRelease,
                UIControlEvent.TouchUpInside |
                UIControlEvent.TouchUpOutside |
                UIControlEvent.TouchCancel |
                UIControlEvent.TouchDragOutside);

            return btn;
        }

        public SclButton AddButton(string title, UIColor backgroundColor = null, UIColor textColor = null, bool showDurationStatus = false)
        {
            // Update view height
            _appearance.SetWindowHeight(_appearance.WindowHeight + _appearance.ButtonHeight);
            // Add button
            var btn = new SclButton();
            btn.Layer.MasksToBounds = true;
            btn.SetTitle(title, UIControlState.Normal);
            btn.TitleLabel.Font = _appearance.ButtonFont;
            btn.CustomBackgroundColor = backgroundColor;
            btn.CustomTextColor = textColor;
            btn.InitialTitle = title;
            btn.ShowDurationStatus = showDurationStatus;
            _contentView.AddSubview(btn);
            Buttons.Add(btn);

            return btn;
        }

        private void ButtonTapped(object sender, EventArgs args)
        {
            var btn = (SclButton)sender;
            if (btn.ActionType == SclActionType.Closure)
            {
                btn.Action?.Invoke();
            }
            else
            {
                Console.WriteLine("Unknow action type for button");
            }

            if (View.Alpha != 0 && _appearance.ShouldAutoDismiss)
            {
                HideView();
            }
        }

        private void ButtonTapDown(object sender, EventArgs args)
        {
            var btn = (SclButton)sender;
            nfloat hue = 0f;
            nfloat saturation = 0;
            nfloat brightness = 0;
            nfloat alpha = 0;
            nfloat pressBrightnessFactor = 0.85f;
            btn.BackgroundColor?.GetHSBA(out hue, out saturation, out brightness, out alpha);
            brightness = brightness * pressBrightnessFactor;
            btn.BackgroundColor = UIColor.FromHSBA(hue, saturation, brightness, alpha);
        }

        private void ButtonRelease(object sender, EventArgs args)
        {
            var btn = (SclButton)sender;
            btn.BackgroundColor = btn.CustomBackgroundColor ?? ViewColor ?? btn.BackgroundColor;
        }

        private void KeyboardWillShow(object sender, UIKeyboardEventArgs args)
        {
            if (_keyboardHasBeenShown)
            {
                return;
            }

            _keyboardHasBeenShown = true;
            var endKeyBoardFrame = args.FrameEnd.GetMinY();

            if (_tmpContentViewFrameOrigin == null)
            {
                _tmpContentViewFrameOrigin = _contentView.Frame.Location;
            }

            if (_tmpCircleViewFrameOrigin == null)
            {
                // todo location replace origin 
                _tmpCircleViewFrameOrigin = _circleBg.Frame.Location;
            }

            var newContentViewFrameY = _contentView.Frame.GetMaxY() - endKeyBoardFrame;
            if (newContentViewFrameY < 0)
            {
                newContentViewFrameY = 0;
            }

            var newBallViewFrameY = _circleBg.Frame.Y - newContentViewFrameY;
            UIView.AnimateNotify(args.AnimationDuration, 0, ConvertToAnimationOptions(args.AnimationCurve), () =>
            {
                var contentViewFrame = _contentView.Frame;
                contentViewFrame.Y -= newContentViewFrameY;
                _contentView.Frame = contentViewFrame;

                var circleBgFrame = _circleBg.Frame;
                circleBgFrame.Y = newBallViewFrameY;
                _circleBg.Frame = circleBgFrame;
            }, null);
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            if (_keyboardHasBeenShown)
            {
                UIView.AnimateNotify(args.AnimationDuration, 0, ConvertToAnimationOptions(args.AnimationCurve), () =>
                {
                    //This could happen on the simulator (keyboard will be hidden)
                    if (_tmpContentViewFrameOrigin.HasValue)
                    {
                        var contentViewFrame = _contentView.Frame;
                        contentViewFrame.Y = _tmpContentViewFrameOrigin.Value.Y;
                        _contentView.Frame = contentViewFrame;
                        _tmpContentViewFrameOrigin = null;
                    }
                    if (_tmpCircleViewFrameOrigin.HasValue)
                    {
                        var circleBgFrame = _circleBg.Frame;
                        circleBgFrame.Y = _tmpCircleViewFrameOrigin.Value.Y;
                        _circleBg.Frame = circleBgFrame;
                        _tmpCircleViewFrameOrigin = null;
                    }
                }, null);
            }

            _keyboardHasBeenShown = false;
        }

        //Dismiss keyboard when tapped outside textfield & close SCLAlertView when hideWhenBackgroundViewIsTapped
        private void Tapped(UITapGestureRecognizer gestureRecognizer)
        {
            View.EndEditing(true);
            if (ReferenceEquals(gestureRecognizer.View.HitTest(gestureRecognizer.LocationInView(gestureRecognizer.View), null),
                    _baseView) && _appearance.HideWhenBackgroundViewIsTapped)
            {
                HideView();
            }
        }

        public SclAlertViewResponder ShowCustom(string title,
            string subTitle,
            UIColor color,
            UIImage icon,
            string closeButtonTitle = null,
            double duration = 0.0,
            UIColor colorStyle = null,
            UIColor colorTextButton = null,
            InteractiveAlertStyle style = InteractiveAlertStyle.Success,
            SclAnimationStyle animationStyle = SclAnimationStyle.TopToBottom)
        {

            colorStyle = colorStyle ?? GetDefaultColorStyle(style);
            colorTextButton = colorTextButton ?? GetDefaultColorTextButton(style) ?? UIColor.White;
            return ShowTitle(title, subTitle, duration, closeButtonTitle, style, color, colorTextButton, icon, animationStyle);
        }

        public SclAlertViewResponder ShowAlert(InteractiveAlertStyle style,
            string title,
            string subTitle,
            string closeButtonTitle = null,
            double duration = 0.0,
            UIColor colorStyle = null,
            UIColor colorTextButton = null,
            UIImage circleIconImage = null,
            SclAnimationStyle animationStyle = SclAnimationStyle.TopToBottom)
        {
            colorStyle = colorStyle ?? GetDefaultColorStyle(style);
            colorTextButton = colorTextButton ?? GetDefaultColorTextButton(style) ?? UIColor.White;

            return ShowTitle(title, subTitle, duration, closeButtonTitle, style, colorStyle, colorTextButton, circleIconImage, animationStyle);
        }

        public SclAlertViewResponder ShowTitle(string title,
            string subTitle,
            double duration,
            string completeText,
            InteractiveAlertStyle style,
            UIColor colorStyle = null,
            UIColor colorTextButton = null,
            UIImage circleIconImage = null,
            SclAnimationStyle animationStyle = SclAnimationStyle.TopToBottom)
        {
            colorStyle = colorStyle ?? UIColor.Black;
            colorTextButton = colorTextButton ?? UIColor.White;
            _selfReference = this;
            View.Alpha = 0;
            var rv = UIApplication.SharedApplication.KeyWindow;
            rv.AddSubview(View);
            View.Frame = rv.Bounds;
            _baseView.Frame = rv.Bounds;

            // Alert colour/icon
            UIImage iconImage = null;
            // Icon style
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                    iconImage = CheckCircleIconImage(circleIconImage, SclAlertViewStyleKit.ImageOfCheckmark());
                    break;
                case InteractiveAlertStyle.Error:
                    iconImage = CheckCircleIconImage(circleIconImage, SclAlertViewStyleKit.ImageOfCross());
                    break;
                //case InteractiveAlertStyle.Notice:
                //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfNotice());
                //break;
                case InteractiveAlertStyle.Warning:
                    iconImage = CheckCircleIconImage(circleIconImage, SclAlertViewStyleKit.ImageOfWarning());
                    break;
                //case InteractiveAlertStyle.Info:
                //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfInfo());
                //break;
                case InteractiveAlertStyle.Edit:
                    iconImage = CheckCircleIconImage(circleIconImage, SclAlertViewStyleKit.ImageOfEdit());
                    break;
                case InteractiveAlertStyle.Wait:
                    iconImage = null;
                    break;
                //case InteractiveAlertStyle.Question:
                //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfQuestion());
                //break;
            }

            // Title
            if (!string.IsNullOrEmpty(title))
            {
                _labelTitle.Text = title;
                var actualHeight = SclAlertViewExtension.HeightWithConstrainedWidth(title, _appearance.WindowWidth - 24, _labelTitle.Font);
                _labelTitle.Frame = new CGRect(12, _appearance.TitleTop, _appearance.WindowWidth - 24, actualHeight);
            }

            // Subtitle
            if (!string.IsNullOrEmpty(subTitle))
            {
                _viewText.Text = subTitle;
                // Adjust text view size, if necessary
                var str = new NSString(subTitle);
                var attr = new UIStringAttributes { Font = _viewText.Font };
                var sz = new CGSize(_appearance.WindowWidth - 24, 90);
                var r = str.GetBoundingRect(sz, NSStringDrawingOptions.UsesLineFragmentOrigin, attr, null);
                var ht = (nfloat)Math.Ceiling(r.Size.Height);
                if (ht < _appearance.TextHeight)
                {
                    _appearance.WindowHeight -= _appearance.TextHeight - ht;
                    _appearance.SetTextHeight(ht);
                }
            }

            if

                // Done button
                (_appearance.ShowCloseButton)
            {
                title = completeText ?? "Done";
                AddButton(title, HideView);
            }

            //hidden/show circular view based on the ui option
            _circleView.Hidden = !_appearance.ShowCircularIcon;
            _circleBg.Hidden = !_appearance.ShowCircularIcon;

            // Alert view colour and images
            _circleView.BackgroundColor = colorStyle;
            // Spinner / icon
            if (style == InteractiveAlertStyle.Wait)
            {
                var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
                indicator.StartAnimating();
                _circleIconView = indicator;
            }
            else
            {
                if (IconTintColor != null)
                {
                    _circleIconView =
                        new UIImageView(iconImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate))
                        {
                            TintColor = IconTintColor
                        };
                }
                else
                {
                    _circleIconView = new UIImageView(iconImage);
                }
            }

            _circleView.AddSubview(_circleIconView);
            var x = (_appearance.CircleHeight - _appearance.CircleIconHeight) / 2f;
            _circleIconView.Frame = new CGRect(x, x, _appearance.CircleIconHeight, _appearance.CircleIconHeight);
            _circleIconView.Layer.CornerRadius = _circleIconView.Bounds.Height / 2f;
            _circleIconView.Layer.MasksToBounds = true;

            foreach (var txt in Inputs)
            {
                txt.Layer.BorderColor = colorStyle.CGColor;
            }

            foreach (var txt in Input)
            {
                txt.Layer.BorderColor = colorStyle.CGColor;
            }

            foreach (var btn in Buttons)
            {
                btn.BackgroundColor = btn.CustomBackgroundColor ?? colorStyle;
                btn.SetTitleColor(btn.CustomTextColor ?? colorTextButton ?? UIColor.White, UIControlState.Normal);
            }

            // Adding duration
            if (duration > 0)
            {
                _duration = duration;
                _durationTimer?.Invalidate();
                _durationTimer = NSTimer.CreateScheduledTimer(_duration, false, obj => { HideView(); });
                _durationStatusTimer?.Invalidate();
                _durationStatusTimer = NSTimer.CreateScheduledTimer(1.0d, true, obj => { UpdateDurationStatus(); });
            }

            // Animate in the alert view
            ShowAnimation(animationStyle);

            // Chainable objects
            return new SclAlertViewResponder(this);
        }

        // Show animation in the alert view
        private void ShowAnimation(
            SclAnimationStyle animationStyle = SclAnimationStyle.TopToBottom,
            float animationStartOffset = -400.0f,
            float boundingAnimationOffset = 15.0f,
            double animationDuration = 0.2f)
        {

            var rv = UIApplication.SharedApplication.KeyWindow;
            var animationStartOrigin = _baseView.Frame;
            CGPoint animationCenter = rv.Center;
            switch (animationStyle)
            {
                case SclAnimationStyle.NoAnimation:
                    View.Alpha = 1.0f;
                    break;
                case SclAnimationStyle.TopToBottom:
                    animationStartOrigin.Location = new CGPoint(animationStartOrigin.X, _baseView.Frame.Y + animationStartOffset);
                    animationCenter = new CGPoint(animationCenter.X, animationCenter.Y + boundingAnimationOffset);
                    break;
                case SclAnimationStyle.BottomToTop:
                    animationStartOrigin.Location = new CGPoint(animationStartOrigin.X, _baseView.Frame.Y - animationStartOffset);
                    animationCenter = new CGPoint(animationCenter.X, animationCenter.Y - boundingAnimationOffset);
                    break;
                case SclAnimationStyle.LeftToRight:
                    animationStartOrigin.Location = new CGPoint(_baseView.Frame.X + animationStartOffset, animationStartOrigin.Y);
                    animationCenter = new CGPoint(animationCenter.X + boundingAnimationOffset, animationCenter.Y);
                    break;
                case SclAnimationStyle.RightToLeft:
                    animationStartOrigin.Location = new CGPoint(_baseView.Frame.X - animationStartOffset, animationStartOrigin.Y);
                    animationCenter = new CGPoint(animationCenter.X - boundingAnimationOffset, animationCenter.Y);
                    break;
            }

            var baseViewFrame = _baseView.Frame;
            baseViewFrame = animationStartOrigin;
            _baseView.Frame = baseViewFrame;

            if (_appearance.DynamicAnimatorActive)
            {
                UIView.AnimateNotify(animationDuration, () =>
                {
                    View.Alpha = 1;
                }, null);

                Animate(_baseView, rv.Center);
            }
            else
            {
                UIView.AnimateNotify(animationDuration, () =>
                {
                    View.Alpha = 1;
                    _baseView.Center = animationCenter;
                }, completion =>
                {
                    UIView.AnimateNotify(animationDuration, () =>
                    {
                        View.Alpha = 1;
                        _baseView.Center = rv.Center;
                    }, null);
                });
            }
        }

        private void Animate(UIView item, CGPoint center)
        {
            if (_snapBehavior != null)
            {
                _animator?.RemoveBehavior(_snapBehavior);
            }

            _animator = new UIDynamicAnimator(View);
            var tempSnapBehavior = new UISnapBehavior(item, center);
            _animator?.AddBehavior(tempSnapBehavior);
            _snapBehavior = tempSnapBehavior;
        }

        private void UpdateDurationStatus()
        {
            _duration = _duration - 1;
            foreach (var btn in Buttons.Where(x => x.ShowDurationStatus))
            {
                var txt = $"{btn.InitialTitle} {_duration}";
                btn.SetTitle(txt, UIControlState.Normal);
            }
        }

        // Close SCLAlertView
        public void HideView()
        {
            UIView.AnimateNotify(0.2, () =>
            {
                View.Alpha = 0;
            }, completion =>
            {
                //Stop durationTimer so alertView does not attempt to hide itself and fire it's dimiss block a second time when close button is tapped
                _durationTimer?.Invalidate();
                // Stop StatusTimer
                _durationStatusTimer?.Invalidate();
                // Call completion handler when the alert is dismissed
                _dismissBlock?.Invoke();

                // This is necessary for SCLAlertView to be de-initialized, preventing a strong reference cycle with the viewcontroller calling SCLAlertView.
                foreach (var button in Buttons)
                {
                    button.Action = null;
                }

                View.RemoveFromSuperview();
                _selfReference = null;
            });
        }

        protected static UIImage CheckCircleIconImage(UIImage circleIconImage, UIImage defaultImage) => circleIconImage ?? defaultImage;

        private static UIViewAnimationOptions ConvertToAnimationOptions(UIViewAnimationCurve curve)
        {
            // Looks like a hack. But it is correct.
            // UIViewAnimationCurve and UIViewAnimationOptions are shifted by 16 bits
            // http://stackoverflow.com/questions/18870447/how-to-use-the-default-ios7-uianimation-curve/18873820#18873820
            return (UIViewAnimationOptions)((int)curve << 16);
        }

        private static UIColor GetDefaultColorTextButton(InteractiveAlertStyle style)
        {
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                case InteractiveAlertStyle.Error:
                //case InteractiveAlertStyle.Notice:
                //case InteractiveAlertStyle.Info:
                case InteractiveAlertStyle.Wait:
                case InteractiveAlertStyle.Edit:
                    //case InteractiveAlertStyle.Question:
                    return UIColor.White;
                case InteractiveAlertStyle.Warning:
                    return UIColor.Black;
                default:
                    return UIColor.White;
            }
        }

        private static UIColor GetDefaultColorStyle(InteractiveAlertStyle style)
        {
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                    // 0x22B573
                    return UIColor.FromRGB(34, 181, 115);
                case InteractiveAlertStyle.Error:
                    // 0xC1272D
                    return UIColor.FromRGB(193, 39, 45);
                //case InteractiveAlertStyle.Notice:
                //// 0x727375
                //return UIColor.FromRGB(114, 115, 117);
                case InteractiveAlertStyle.Warning:
                    // 0xFFD110
                    return UIColor.FromRGB(255, 209, 16);
                //case InteractiveAlertStyle.Info:
                //// 0x2866BF
                //return UIColor.FromRGB(40, 102, 191);
                case InteractiveAlertStyle.Edit:
                    // 0xA429FF
                    return UIColor.FromRGB(164, 41, 255);
                case InteractiveAlertStyle.Wait:
                    // 0xD62DA5
                    return UIColor.FromRGB(204, 45, 165);
                //case InteractiveAlertStyle.Question:
                //// 0x727375
                //return UIColor.FromRGB(114, 115, 117);
                default:
                    return UIColor.White;
            }
        }

        // Button sub-class
        public class SclButton : UIButton
        {
            public SclActionType ActionType { get; set; } = SclActionType.None;
            public UIColor CustomBackgroundColor { get; set; }
            public UIColor CustomTextColor { get; set; }
            public string InitialTitle { get; set; }
            public bool ShowDurationStatus { get; set; }
            public Action Action { get; set; }

            public SclButton() : base(CGRect.Empty)
            {

            }

            public SclButton(CGRect rect) : base(rect)
            {

            }
        }

        protected static class SclAlertViewExtension
        {
            public static nfloat HeightWithConstrainedWidth(string text, nfloat width, UIFont font)
            {
                var constraintRect = new CGSize(width, nfloat.MaxValue);
                var boundingBox = new NSString(text).GetBoundingRect(constraintRect, NSStringDrawingOptions.UsesLineFragmentOrigin, new UIStringAttributes { Font = font }, null);

                return boundingBox.Height;
            }
        }

        // ------------------------------------
        // Icon drawing
        // Code generated by PaintCode
        // ------------------------------------
        protected class SclAlertViewStyleKit : NSObject
        {
            // Cache
            protected static class Cache
            {
                public static UIImage ImageOfCheckmarkImage;
                public static UIImage ImageOfCrossImage;
                public static UIImage ImageOfNoticeImage;
                public static UIImage ImageOfWarningImage;
                public static UIImage ImageOfInfoImage;
                public static UIImage ImageOfEditImage;
                public static UIImage ImageOfQuestionImage;
            }

            public static UIImage ImageOfCheckmark() => RendererandCacheImage(DrawCheckmark, ref Cache.ImageOfCheckmarkImage);

            public static UIImage ImageOfCross() => RendererandCacheImage(DrawCross, ref Cache.ImageOfCrossImage);

            public static UIImage ImageOfNotice() => RendererandCacheImage(DrawNotice, ref Cache.ImageOfNoticeImage);

            public static UIImage ImageOfWarning() => RendererandCacheImage(DrawWarning, ref Cache.ImageOfWarningImage);

            public static UIImage ImageOfInfo() => RendererandCacheImage(DrawInfo, ref Cache.ImageOfInfoImage);

            public static UIImage ImageOfEdit() => RendererandCacheImage(DrawEdit, ref Cache.ImageOfEditImage);

            public static UIImage ImageOfQuestion() => RendererandCacheImage(DrawQuestion, ref Cache.ImageOfQuestionImage);

            private static UIImage RendererandCacheImage(Action rendererAction, ref UIImage image)
            {
                if (image != null)
                {
                    return image;
                }

                UIGraphics.BeginImageContextWithOptions(new CGSize(80, 80), false, 0);
                rendererAction.Invoke();
                image = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return image;
            }

            // Drawing Methods
            private static void DrawCheckmark()
            {
                // Checkmark Shape Drawing
                var checkmarkShapePath = new UIBezierPath();
                checkmarkShapePath.MoveTo(new CGPoint(x: 73.25, y: 14.05));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 64.51, y: 13.86), new CGPoint(x: 70.98, y: 11.44), new CGPoint(x: 66.78, y: 11.26));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 27.46, y: 52));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 15.75, y: 39.54));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 6.84, y: 39.54), new CGPoint(x: 13.48, y: 36.93), new CGPoint(x: 9.28, y: 36.93));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 6.84, y: 49.02), new CGPoint(x: 4.39, y: 42.14), new CGPoint(x: 4.39, y: 46.42));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 22.91, y: 66.14));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 27.28, y: 68), new CGPoint(x: 24.14, y: 67.44), new CGPoint(x: 25.71, y: 68));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 31.65, y: 66.14), new CGPoint(x: 28.86, y: 68), new CGPoint(x: 30.43, y: 67.26));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 73.08, y: 23.35));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 73.25, y: 14.05), new CGPoint(x: 75.52, y: 20.75), new CGPoint(x: 75.7, y: 16.65));
                checkmarkShapePath.ClosePath();
                checkmarkShapePath.MiterLimit = 4;

                UIColor.White.SetFill();
                checkmarkShapePath.Fill();
            }

            private static void DrawCross()
            {
                // Cross Shape Drawing
                var crossShapePath = new UIBezierPath();
                crossShapePath.MoveTo(new CGPoint(x: 10, y: 70));
                crossShapePath.AddLineTo(new CGPoint(x: 70, y: 10));
                crossShapePath.MoveTo(new CGPoint(x: 10, y: 10));
                crossShapePath.AddLineTo(new CGPoint(x: 70, y: 70));
                crossShapePath.LineCapStyle = CGLineCap.Round;
                crossShapePath.LineJoinStyle = CGLineJoin.Round;
                UIColor.White.SetStroke();
                crossShapePath.LineWidth = 14;
                crossShapePath.Stroke();
            }

            private static void DrawNotice()
            {
                // Notice Shape Drawing
                var noticeShapePath = new UIBezierPath();
                noticeShapePath.MoveTo(new CGPoint(x: 72, y: 48.54));
                noticeShapePath.AddLineTo(new CGPoint(x: 72, y: 39.9));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 66.38, y: 34.01), new CGPoint(x: 72, y: 36.76), new CGPoint(x: 69.48, y: 34.01));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 61.53, y: 35.97), new CGPoint(x: 64.82, y: 34.01), new CGPoint(x: 62.69, y: 34.8));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 60.36, y: 35.78), new CGPoint(x: 61.33, y: 35.97), new CGPoint(x: 62.3, y: 35.78));
                noticeShapePath.AddLineTo(new CGPoint(x: 60.36, y: 33.22));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 54.16, y: 26.16), new CGPoint(x: 60.36, y: 29.3), new CGPoint(x: 57.65, y: 26.16));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 48.73, y: 29.89), new CGPoint(x: 51.64, y: 26.16), new CGPoint(x: 50.67, y: 27.73));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.73, y: 28.71));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 43.49, y: 21.64), new CGPoint(x: 48.73, y: 24.78), new CGPoint(x: 46.98, y: 21.64));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 39.03, y: 25.37), new CGPoint(x: 40.97, y: 21.64), new CGPoint(x: 39.03, y: 23.01));
                noticeShapePath.AddLineTo(new CGPoint(x: 39.03, y: 9.07));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 32.24, y: 2), new CGPoint(x: 39.03, y: 5.14), new CGPoint(x: 35.73, y: 2));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 25.45, y: 9.07), new CGPoint(x: 28.56, y: 2), new CGPoint(x: 25.45, y: 5.14));
                noticeShapePath.AddLineTo(new CGPoint(x: 25.45, y: 41.47));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 24.29, y: 43.44), new CGPoint(x: 25.45, y: 42.45), new CGPoint(x: 24.68, y: 43.04));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 9.55, y: 43.04), new CGPoint(x: 16.73, y: 40.88), new CGPoint(x: 11.88, y: 40.69));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 8, y: 46.58), new CGPoint(x: 8.58, y: 43.83), new CGPoint(x: 8, y: 45.2));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 14.4, y: 55.81), new CGPoint(x: 8.19, y: 50.31), new CGPoint(x: 12.07, y: 53.84));
                noticeShapePath.AddLineTo(new CGPoint(x: 27.2, y: 69.56));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 42.91, y: 77.8), new CGPoint(x: 30.5, y: 74.47), new CGPoint(x: 35.73, y: 77.21));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 43.88, y: 77.8), new CGPoint(x: 43.3, y: 77.8), new CGPoint(x: 43.68, y: 77.8));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 47.18, y: 78), new CGPoint(x: 45.04, y: 77.8), new CGPoint(x: 46.01, y: 78));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.34, y: 78));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.34, y: 78));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 71.61, y: 52.08), new CGPoint(x: 56.48, y: 78), new CGPoint(x: 69.87, y: 75.05));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 72, y: 48.54), new CGPoint(x: 71.81, y: 51.29), new CGPoint(x: 72, y: 49.72));
                noticeShapePath.ClosePath();
                noticeShapePath.MiterLimit = 4;

                UIColor.White.SetFill();
                noticeShapePath.Fill();
            }

            private static void DrawWarning()
            {
                // Color Declarations
                var greyColor = new UIColor(red: 0.236f, green: 0.236f, blue: 0.236f, alpha: 1.000f);

                // Warning Group
                // Warning Circle Drawing
                var warningCirclePath = new UIBezierPath();
                warningCirclePath.MoveTo(new CGPoint(x: 40.94, y: 63.39));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 36.03, y: 65.55), new CGPoint(x: 39.06, y: 63.39), new CGPoint(x: 37.36, y: 64.18));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 34.14, y: 70.45), new CGPoint(x: 34.9, y: 66.92), new CGPoint(x: 34.14, y: 68.49));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 36.22, y: 75.54), new CGPoint(x: 34.14, y: 72.41), new CGPoint(x: 34.9, y: 74.17));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 77.5), new CGPoint(x: 37.54, y: 76.91), new CGPoint(x: 39.06, y: 77.5));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 45.86, y: 75.35), new CGPoint(x: 42.83, y: 77.5), new CGPoint(x: 44.53, y: 76.72));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 47.93, y: 70.45), new CGPoint(x: 47.18, y: 74.17), new CGPoint(x: 47.93, y: 72.41));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 45.86, y: 65.35), new CGPoint(x: 47.93, y: 68.49), new CGPoint(x: 47.18, y: 66.72));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 63.39), new CGPoint(x: 44.53, y: 64.18), new CGPoint(x: 42.83, y: 63.39));
                warningCirclePath.ClosePath();
                warningCirclePath.MiterLimit = 4;

                greyColor.SetFill();
                warningCirclePath.Fill();


                // Warning Shape Drawing
                var warningShapePath = new UIBezierPath();
                warningShapePath.MoveTo(new CGPoint(x: 46.23, y: 4.26));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 2.5), new CGPoint(x: 44.91, y: 3.09), new CGPoint(x: 43.02, y: 2.5));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 34.71, y: 4.26), new CGPoint(x: 38.68, y: 2.5), new CGPoint(x: 36.03, y: 3.09));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 31.5, y: 8.77), new CGPoint(x: 33.01, y: 5.44), new CGPoint(x: 31.5, y: 7.01));
                warningShapePath.AddLineTo(new CGPoint(x: 31.5, y: 19.36));
                warningShapePath.AddLineTo(new CGPoint(x: 34.71, y: 54.44));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 40.38, y: 58.16), new CGPoint(x: 34.9, y: 56.2), new CGPoint(x: 36.41, y: 58.16));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 45.67, y: 54.44), new CGPoint(x: 44.34, y: 58.16), new CGPoint(x: 45.67, y: 56.01));
                warningShapePath.AddLineTo(new CGPoint(x: 48.5, y: 19.36));
                warningShapePath.AddLineTo(new CGPoint(x: 48.5, y: 8.77));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 46.23, y: 4.26), new CGPoint(x: 48.5, y: 7.01), new CGPoint(x: 47.74, y: 5.44));
                warningShapePath.ClosePath();
                warningShapePath.MiterLimit = 4;

                greyColor.SetFill();
                warningShapePath.Fill();
            }

            private static void DrawInfo()
            {
                // Color Declarations
                var color0 = new UIColor(red: 1.000f, green: 1.000f, blue: 1.000f, alpha: 1.000f);

                // Info Shape Drawing
                var infoShapePath = new UIBezierPath();
                infoShapePath.MoveTo(new CGPoint(x: 45.66, y: 15.96));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 45.66, y: 5.22), new CGPoint(x: 48.78, y: 12.99), new CGPoint(x: 48.78, y: 8.19));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 34.34, y: 5.22), new CGPoint(x: 42.53, y: 2.26), new CGPoint(x: 37.47, y: 2.26));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 34.34, y: 15.96), new CGPoint(x: 31.22, y: 8.19), new CGPoint(x: 31.22, y: 12.99));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 45.66, y: 15.96), new CGPoint(x: 37.47, y: 18.92), new CGPoint(x: 42.53, y: 18.92));
                infoShapePath.ClosePath();
                infoShapePath.MoveTo(new CGPoint(x: 48, y: 69.41));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 40, y: 77), new CGPoint(x: 48, y: 73.58), new CGPoint(x: 44.4, y: 77));
                infoShapePath.AddLineTo(new CGPoint(x: 40, y: 77));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 32, y: 69.41), new CGPoint(x: 35.6, y: 77), new CGPoint(x: 32, y: 73.58));
                infoShapePath.AddLineTo(new CGPoint(x: 32, y: 35.26));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 40, y: 27.67), new CGPoint(x: 32, y: 31.08), new CGPoint(x: 35.6, y: 27.67));
                infoShapePath.AddLineTo(new CGPoint(x: 40, y: 27.67));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 48, y: 35.26), new CGPoint(x: 44.4, y: 27.67), new CGPoint(x: 48, y: 31.08));
                infoShapePath.AddLineTo(new CGPoint(x: 48, y: 69.41));
                infoShapePath.ClosePath();
                color0.SetFill();
                infoShapePath.Fill();
            }

            private static void DrawEdit()
            {
                // Color Declarations
                var color = new UIColor(red: 1.0f, green: 1.0f, blue: 1.0f, alpha: 1.0f);

                // Edit shape Drawing
                var editPathPath = new UIBezierPath();
                editPathPath.MoveTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.AddCurveToPoint(new CGPoint(x: 71.9, y: 15.2), new CGPoint(x: 74.7, y: 5.9), new CGPoint(x: 75.1, y: 11.6));
                editPathPath.AddLineTo(new CGPoint(x: 64.5, y: 23.7));
                editPathPath.AddLineTo(new CGPoint(x: 49.9, y: 11.1));
                editPathPath.AddLineTo(new CGPoint(x: 57.3, y: 2.6));
                editPathPath.AddCurveToPoint(new CGPoint(x: 69.7, y: 1.7), new CGPoint(x: 60.4, y: -1.1), new CGPoint(x: 66.1, y: -1.5));
                editPathPath.AddLineTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.AddLineTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.AddLineTo(new CGPoint(x: 13.4, y: 53.1));
                editPathPath.AddLineTo(new CGPoint(x: 15.7, y: 55.1));
                editPathPath.AddLineTo(new CGPoint(x: 50.1, y: 15.5));
                editPathPath.AddLineTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.AddLineTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.AddLineTo(new CGPoint(x: 23.8, y: 62.2));
                editPathPath.AddLineTo(new CGPoint(x: 58.2, y: 22.6));
                editPathPath.AddLineTo(new CGPoint(x: 52, y: 17.1));
                editPathPath.AddLineTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.AddLineTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.AddLineTo(new CGPoint(x: 60.1, y: 24.2));
                editPathPath.AddLineTo(new CGPoint(x: 62.3, y: 26.1));
                editPathPath.AddLineTo(new CGPoint(x: 28.1, y: 65.7));
                editPathPath.AddLineTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.AddLineTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 25.9, y: 68.1));
                editPathPath.AddLineTo(new CGPoint(x: 4.2, y: 79.5));
                editPathPath.AddLineTo(new CGPoint(x: 11.3, y: 55.5));
                editPathPath.AddLineTo(new CGPoint(x: 25.9, y: 68.1));
                editPathPath.ClosePath();
                editPathPath.MiterLimit = 4;
                editPathPath.UsesEvenOddFillRule = true;
                color.SetFill();
                editPathPath.Fill();
            }

            private static void DrawQuestion()
            {
                // Color Declarations
                var color = new UIColor(red: 1.0f, green: 1.0f, blue: 1.0f, alpha: 1.0f);
                // Questionmark Shape Drawing
                var questionShapePath = new UIBezierPath();
                questionShapePath.MoveTo(new CGPoint(x: 33.75, y: 54.1));
                questionShapePath.AddLineTo(new CGPoint(x: 44.15, y: 54.1));
                questionShapePath.AddLineTo(new CGPoint(x: 44.15, y: 47.5));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 51.85, y: 37.2), new CGPoint(x: 44.15, y: 42.9), new CGPoint(x: 46.75, y: 41.2));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 61.95, y: 19.9), new CGPoint(x: 59.05, y: 31.6), new CGPoint(x: 61.95, y: 28.5));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 41.45, y: 2.8), new CGPoint(x: 61.95, y: 7.6), new CGPoint(x: 52.85, y: 2.8));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 25.05, y: 5.8), new CGPoint(x: 34.75, y: 2.8), new CGPoint(x: 29.65, y: 3.8));
                questionShapePath.AddLineTo(new CGPoint(x: 25.05, y: 14.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 38.15, y: 12.3), new CGPoint(x: 29.15, y: 13.2), new CGPoint(x: 32.35, y: 12.3));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 49.65, y: 20.8), new CGPoint(x: 45.65, y: 12.3), new CGPoint(x: 49.65, y: 14.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 43.65, y: 31.7), new CGPoint(x: 49.65, y: 26), new CGPoint(x: 47.95, y: 28.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 33.75, y: 46.6), new CGPoint(x: 37.15, y: 36.9), new CGPoint(x: 33.75, y: 39.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.75, y: 54.1));
                questionShapePath.ClosePath();
                questionShapePath.MoveTo(new CGPoint(x: 33.15, y: 75.4));
                questionShapePath.AddLineTo(new CGPoint(x: 45.35, y: 75.4));
                questionShapePath.AddLineTo(new CGPoint(x: 45.35, y: 63.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.15, y: 63.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.15, y: 75.4));
                questionShapePath.ClosePath();
                color.SetFill();
                questionShapePath.Fill();
            }
        }
    }
}