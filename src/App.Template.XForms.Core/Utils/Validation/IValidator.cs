﻿namespace App.Template.XForms.Core.Utils.Validation
{
    public interface IValidator
    {
        IErrorCollection Validate(object subject, string group = null);
        bool IsRequired(object subject, string memberName);
    }
}