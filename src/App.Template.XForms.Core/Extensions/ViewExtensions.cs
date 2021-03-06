﻿using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace App.Template.XForms.Core.Extensions
{
    public static class ViewExtensions
    {
        public static T GetBehaviour<T>(this View view) where T : Behavior<View>
        {
            return view.Behaviors?.SingleOrDefault(b => b is T) as T;
        }

        public static IEnumerable<T> GetBehaviours<T>(this View view) where T : Behavior<View>
        {
            return view.Behaviors?.Where(b => b is T) as IEnumerable<T>;
        }
    }
}
