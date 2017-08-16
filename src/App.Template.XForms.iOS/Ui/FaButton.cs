using System;
using App.Template.XForms.Core.Fonts;
using UIKit;

namespace App.Template.XForms.iOS.Ui
{
    public sealed class FaButton : UIButton
    {
        /// <summary>
        /// Gets or sets the icon for the button. Should be from <see cref="FontAwesome"/>.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get => Title(UIControlState.Normal);
            set => SetTitle(value, UIControlState.Normal);
        }

        /// <summary>
        /// Gets or sets the size of the icon
        /// </summary>
        /// <value>The size of the icon.</value>
        public nfloat IconSize
        {
            get => Font.PointSize;
            set => Font = FontAwesomeHelper.Font(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaButton"/> class.
        /// This class extends UIButton. It does not set a default Frame. You must do this yourself
        /// </summary>
        /// <param name="icon">Icon.</param>
        /// <param name="fontColor">Font color.</param>
        /// <param name="iconSize">Icon size.</param>
        public FaButton(string icon, UIColor fontColor, float iconSize = 20)
        {
            Icon = icon;
            IconSize = iconSize;
            SetTitleColor(fontColor, UIControlState.Normal);
            SetTitleColor(fontColor.ColorWithAlpha(100), UIControlState.Highlighted);
        }
    }
}