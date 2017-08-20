using System.Collections.Generic;
using App.Template.XForms.Core.Models;

namespace App.Template.XForms.Core.Contracts
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems();
    }
}
