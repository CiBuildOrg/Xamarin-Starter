﻿using System.Collections.Generic;

namespace App.Template.XForms.Core.Contracts
{
    public interface IViewViewModelBagService
    {
        List<ViewModelViewBagItem> GetViewViewModelCorrespondenceMap();
    }
}
