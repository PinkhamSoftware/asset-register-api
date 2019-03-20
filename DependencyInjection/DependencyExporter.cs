using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace DependencyInjection
{
    using IDependencyReceiver = Action<Type, Func<object>>;
    using ITypeDependencyReceiver = Action<Type, Type>;

    public abstract class DependencyExporter
    {
        private readonly Dictionary<Type, Func<object>> _dependencies;
        private readonly Dictionary<Type, Type> _typeDependencies;

        private readonly Dictionary<Type, Func<object>> _singletonDependencies;
        private readonly Dictionary<Type, Type> _singletonTypeDependencies;

        protected IConfiguration Configuration { get; private set; }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        protected DependencyExporter(IConfiguration configuration)
        {
            Configuration = configuration;
            _dependencies = new Dictionary<Type,Func<object>>();
            _typeDependencies = new Dictionary<Type, Type>();
            _singletonTypeDependencies = new Dictionary<Type, Type>();
            _singletonDependencies = new Dictionary<Type, Func<object>>();

            RegisterAllExportedDependencies();
            ConstructHiddenDependencies();
        }
        
        public virtual T Get<T>() where T : class
        {
            return _dependencies[typeof(T)]() as T;
        }
        
        public void ExportDependencies(IDependencyReceiver dependencyReceiver)
        {
            foreach(var (type, provider) in _dependencies)
            {
                dependencyReceiver(type, provider);
            }
        }

        public void ExportTypeDependencies(ITypeDependencyReceiver dependencyReceiver)
        {
            foreach (var (type, typeDependency) in _typeDependencies)
            {
                dependencyReceiver(type, typeDependency);
            }
        }

        public void ExportSingletonDependencies(IDependencyReceiver dependencyReceiver)
        {
            foreach (var (type, provider) in _singletonDependencies)
            {
                dependencyReceiver(type, provider);
            }
        }

        public void ExportSingletonTypeDependencies(ITypeDependencyReceiver dependencyReceiver)
        {
            foreach (var (type, typeDependency) in _singletonTypeDependencies)
            {
                dependencyReceiver(type, typeDependency);
            }
        }

        protected void RegisterExportedDependency<T>(Func<object> provider)
        {
            _dependencies.Add(typeof(T), provider);
        }

        protected void RegisterExportedDependency<TInterface, TDependency>()
        {
            _typeDependencies.Add(typeof(TInterface), typeof(TDependency));
        }

        protected void RegisterExportedSingletonDependency<T>(Func<object> provider)
        {
            _singletonDependencies.Add(typeof(T), provider);
        }

        protected void RegisterExportedSingletonDependency<TInterface, TDependency>()
        {
            _singletonTypeDependencies.Add(typeof(TInterface), typeof(TDependency));
        }

        protected abstract void ConstructHiddenDependencies();
        protected abstract void RegisterAllExportedDependencies();
    }
}
