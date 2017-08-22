using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Infrastructure
{
    public class Now : INow
    {
        public DateTime DateNow => DateTime.Now;
    }
}