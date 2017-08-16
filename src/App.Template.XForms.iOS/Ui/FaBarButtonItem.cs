using System;
using App.Template.XForms.Core.Exceptions;
using CoreGraphics;
using UIKit;

namespace App.Template.XForms.iOS.Ui
{
    public sealed class FaBarButtonItem : UIBarButtonItem
    {
        private readonly UILabel _titleLabel;
        private readonly UIButton _iconButton;

        /// <summary>
        /// Gets or sets the Title of the button.
        /// Throws <see cref="FontAwesomeException"/> if this button does not have a title
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get => _titleLabel?.Text;
            set
            {
                if (_titleLabel != null)
                {
                    _titleLabel.Text = value;
                }
                else
                {
                    throw new FontAwesomeException("This button does not have a title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the icon for the button. Should be from <see cref="FontAwesomeException"/>.
        /// Throws <see cref="FontAwesome"/> if the button has not been initialized yet
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get
            {
                if (_iconButton != null)
                {
                    return _iconButton.Title(UIControlState.Normal);
                }
                else
                {
                    throw new FontAwesomeException("This button has not been initialized yet");
                }
            }
            set
            {
                if (_iconButton != null)
                {
                    _iconButton.SetTitle(value, UIControlState.Normal);
                }
                else
                {
                    throw new FontAwesomeException("This button has not been initialized yet");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaBarButtonItem"/> class.
        /// Use the CustomView property to access the button that we create with the new icon
        /// </summary>
        /// <param name="icon">An icon from <see cref="FontAwesome"/></param>
        /// <param name="handler">The UIColor for the icon and title</param>
        /// <param name="handler">The event handler for when the button is pressed</param>
        public FaBarButtonItem(string icon, UIColor fontColor, EventHandler handler)
        {
            _iconButton = new UIButton(new CGRect(0, 0, 32, 32))
            {
                Font = FontAwesomeHelper.Font(25)
            };
            _iconButton.SetTitleColor(fontColor, UIControlState.Normal);
            _iconButton.TouchUpInside += handler;

            Icon = icon;
            CustomView = _iconButton;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaBarButtonItem"/> class.
        /// </summary>
        /// <param name="icon">An icon from <see cref="FontAwesome"/></param>
        /// <param name="fontColor">A title to display under the icon</param>
        /// <param name="handler">The UIColor for the icon and title</param>
        /// <param name="handler">The event handler for when the button is pressed</param>
        public FaBarButtonItem(string icon, string title, UIColor fontColor, EventHandler handler)
        {
            var view = new UIView(new CGRect(0, 0, 32, 32));

            _iconButton = new UIButton(new CGRect(0, 0, 32, 21))
            {
                Font = FontAwesomeHelper.Font(20),
            };
            _iconButton.SetTitleColor(fontColor, UIControlState.Normal);
            _iconButton.TouchUpInside += handler;

            _titleLabel = new UILabel(new CGRect(0, 18, 32, 10))
            {
                TextColor = fontColor,
                Font = UIFont.SystemFontOfSize(10f),
                TextAlignment = UITextAlignment.Center,
                Text = title
            };

            Icon = icon;

            view.Add(_iconButton);
            view.Add(_titleLabel);

            CustomView = view;
        }
    }
}