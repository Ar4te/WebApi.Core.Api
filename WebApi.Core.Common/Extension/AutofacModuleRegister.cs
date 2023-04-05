using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;

namespace WebApi.Core.Common.Extension
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册Service
            var assemblysServices = Assembly.Load("WebApi.Core.Service");
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerDependency()  // 默认模式，每次调用都会重新实例化对象；每次请求都创建一个新的对象
                .AsImplementedInterfaces() // 以接口方式进行注入，注入这些类的所有的公共接口作为服务
                .EnableInterfaceInterceptors(); // 引入Autofac.Extras.DynamicProxy；应用拦截器

            // 注册Repository
            var assemblysRepository = Assembly.Load("WebApi.Core.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository)
                .InstancePerDependency()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors();
        }
    }
}
