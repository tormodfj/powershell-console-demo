using Caliburn.Micro;
using ScriptablePhonebook.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Configuration;

namespace ScriptablePhonebook
{
    public class AppBootstrapper : Bootstrapper<PhonebookViewModel>
    {
        private CompositionContainer container;

        protected override void Configure()
        {
            var assemblyCatalogs = AssemblySource.Instance
                .Select(a => new AssemblyCatalog(a))
                .OfType<ComposablePartCatalog>();
            var catalog = new AggregateCatalog(assemblyCatalogs);
            container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);
            batch.AddExportedValue("Profile", ConfigurationManager.AppSettings["Profile"]);

            container.Compose(batch);

            base.Configure();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (!exports.Any())
            {
                throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
            }

            return exports.First();
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }
    }
}