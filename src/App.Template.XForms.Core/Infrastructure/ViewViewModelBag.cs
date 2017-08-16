using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Template.XForms.Core.Bootstrapper;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.MvvmCross;
using MvvmCross.Platform.IoC;

namespace App.Template.XForms.Core.Infrastructure
{
    public class ViewViewModelBagService : IViewViewModelBagService
    {
        private static Dictionary<string, Type> GetTypesInThisAssembly(string typeSuffix)
        {
            return CoreAssemblyHelper.CoreAssembly.CreatableTypes()
                .Where(t => t.Name.EndsWith(typeSuffix))
                .ToDictionary(t => t.Name.Remove(t.Name.LastIndexOf(typeSuffix, StringComparison.Ordinal)));
        }

        public List<ViewModelViewBagItem> GetViewViewModelCorrespondenceMap()
        {
            var result = new List<ViewModelViewBagItem>();

            var viewModelTypes = GetTypesInThisAssembly(MvvmConfig.ViewModelSuffix);
            var viewTypes = GetTypesInThisAssembly(MvvmConfig.ViewSuffix);

            foreach (var viewModelTypeAndName in viewModelTypes)
            {
                if (viewTypes.TryGetValue(viewModelTypeAndName.Key, out Type viewType))
                {
                    result.Add(new ViewModelViewBagItem
                    {
                        ViewModel = viewModelTypeAndName.Value,
                        View = viewType
                    });
                }
            }

            return result;
        }
    }
}
