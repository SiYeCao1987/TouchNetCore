using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace TouchNetCore.Component.Autofac
{
    /// <summary>
    /// Ioc容器管理
    /// </summary>
    public class IocManager : IIocManager
    {
        private IContainer _container;

        public static IocManager Instance { get; } = new IocManager();

        /// <summary>
        /// 容器
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// Ioc容器初始化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IServiceProvider Initialize(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(Instance).As<IIocManager>().SingleInstance();
            //所有程序集 和程序集下类型
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            var listAllType = new List<Type>();
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    listAllType.AddRange(assembly.GetTypes().Where(type => type != null));
                }
                catch { }
            }

            //找到所有外部IDependencyRegistrar实现，调用注册
            var registrarType = typeof(IDependencyRegistrar);
            var arrRegistrarType = listAllType.Where(t => registrarType.IsAssignableFrom(t) && t != registrarType).ToArray();
            var listRegistrarInstances = new List<IDependencyRegistrar>();
            foreach (var drType in arrRegistrarType)
            {
                listRegistrarInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            //排序
            listRegistrarInstances = listRegistrarInstances.OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in listRegistrarInstances)
            {
                dependencyRegistrar.Register(builder, listAllType);
            }

            //注册ITransientDependency实现类
            var dependencyType = typeof(ITransientDependency);
            var arrDependencyType = listAllType.Where(t => dependencyType.IsAssignableFrom(t) && t != dependencyType).ToArray();
            builder.RegisterTypes(arrDependencyType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired().EnableInterfaceInterceptors();

            foreach (Type type in arrDependencyType)
            {
                if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                {
                    builder.RegisterType(type).As(type.BaseType)
                        .InstancePerLifetimeScope()
                        .PropertiesAutowired();
                }
            }


            //注册ISingletonDependency实现类
            var singletonDependencyType = typeof(ISingletonDependency);
            var arrSingletonDependencyType = listAllType.Where(t => singletonDependencyType.IsAssignableFrom(t) && t != singletonDependencyType).ToArray();
            builder.RegisterTypes(arrSingletonDependencyType)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();

            foreach (Type type in arrSingletonDependencyType)
            {
                if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                {
                    builder.RegisterType(type).As(type.BaseType)
                        .SingleInstance()
                        .PropertiesAutowired();
                }
            }

            builder.Populate(services);
            _container = builder.Build();
            return new AutofacServiceProvider(_container);
        }

        /// <summary>
        /// 服务实例是否有注册
        /// </summary>
        public bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// 从容器中获取服务实例
        /// </summary>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 从容器中获取服务实例
        /// </summary>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 从容器中获取服务实例
        /// </summary>
        public virtual T Resolve<T>(params Parameter[] parameters) where T : class
        {
            var scope = Scope();
            return scope.Resolve<T>(parameters);
        }

        /// <summary>
        /// 从容器中获取一组服务实例
        /// </summary>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// 从容器中获取服务实例（可空）
        /// </summary>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new Exception("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No constructor was found that had all the dependencies satisfied.");
        }

        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        public virtual ILifetimeScope Scope()
        {
            try
            {
                return Container.BeginLifetimeScope();
            }
            catch (Exception)
            {
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        /// <summary>
        /// 尝试从容器中获取服务实例
        /// </summary>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }
    }
}
