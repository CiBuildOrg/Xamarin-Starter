﻿using System.Windows.Input;
using Xamarin.Forms;

namespace App.Template.XForms.Core.Forms.Attached
{
    public class EntryCompleteAttached
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(Entry),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnEntryComplete);

        private static void OnEntryComplete(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Entry control)
                control.Completed += (sender, args) =>
                {
                    var entry = sender as Entry;
                    var command = GetItemTapped(entry);
                    command?.Execute(args);
                };
        }

        public static void SetItemTapped(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }
public static ICommand GetItemTapped(BindableObject bindable)
        {
            return (ICommand)bindable.GetValue(CommandProperty);
        }
    }
}
