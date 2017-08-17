using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.MvvmCross;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace App.Template.XForms.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            LoadServiceLocator();
            RegisterAppStart<MenuViewModel>();
        }

        public static IMvxViewsContainer LoadViewsContainer(IMvxViewsContainer viewsContainer)
        {
            var viewModelTypes = GetTypesInAssembly("App.Template.XForms.Core", MvvmConfig.ViewModelSuffix);

            var viewTypes = GetTypesInAssembly("App.Template.XForms.Core", MvvmConfig.ViewSuffix);
            foreach (var viewModelTypeAndName in viewModelTypes)
            {
                if (viewTypes.TryGetValue(viewModelTypeAndName.Key, out Type viewType))
                    viewsContainer.Add(viewModelTypeAndName.Value, viewType);
            }
            return viewsContainer;
        }

        public static IMvxViewsContainer LoadViewsContainer(IMvxViewsContainer viewsContainer, IMvxViewsContainerHelper viewViewModelBagService)
        {
            foreach (var bag in viewViewModelBagService.GetViewViewModelCorrespondenceMap())
            {
                viewsContainer.Add(bag.ViewModel, bag.View);
            }

            return viewsContainer;
        }

        private static Dictionary<string, Type> GetTypesInAssembly(string assembyName, string typeSuffix)
        {
            return Assembly.Load(new AssemblyName(assembyName)).CreatableTypes()
                .Where(t => t.Name.EndsWith(typeSuffix))
                .ToDictionary(t => t.Name.Remove(t.Name.LastIndexOf(typeSuffix, StringComparison.Ordinal)));
        }

        private void LoadServiceLocator()
        {
            RegisterAssemblyTypes("App.Template.XForms.Core");
            Mvx.RegisterSingleton<IMvxFormsPageLoader>(new MvxFormsViewLoader());
        }

        private void RegisterAssemblyTypes(string assemblyName)
        {
            // By default register All types as interface and dynamic.
            Assembly.Load(new AssemblyName(assemblyName)).CreatableTypes()
                .AsInterfaces()
                .RegisterAsDynamic();

            // Override Models registration, are register as Types.
            // An POCO should not has behavior therefore not are required interfaces for testeability.
            Assembly.Load(new AssemblyName(assemblyName)).CreatableTypes()
                .Where(t => t.Namespace.EndsWith("Models"))
                .AsTypes()
                .RegisterAsDynamic();
        }
    }
}