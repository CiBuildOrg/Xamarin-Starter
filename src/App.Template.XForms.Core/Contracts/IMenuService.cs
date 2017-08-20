using System;
using System.Collections.Generic;
using System.Text;
using App.Template.XForms.Core.Models;

namespace App.Template.XForms.Core.Contracts
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems();
    }
}
