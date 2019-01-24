using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Toolkits;
using WCF.IAppService.Interfaces;

namespace WCF.Core.Client
{

    class Program
    {
        static void Main(string[] args)
        {

            //Init Container
            IServiceProvider serviceProvider = InitServiceProvider();

            //Calling WCF
            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                IProductService productService = serviceScope.ServiceProvider.GetService<IProductService>();//This is a remote interface
                string products = productService.GetProducts();
                Console.WriteLine(products);
            }

            Console.ReadKey();
            
            #region 其他普通方式

            string url = "http://127.0.0.1:8734/MyWcfServiceTest/Service1/";
            // url = $"net.tcp://{ip}:8734/MyWcfServiceTest/Service1.svc";
            IService1 proxy = WcfInvokeFactory.CreateServiceByUrl<IService1>(url, BindingTypeEnum.Nettcpbinding);
            var result = proxy.GetData(3);
            Console.WriteLine(result.ToString());

            // url = $"net.tcp://{ip}:8734/MyWcfServiceTest/Service2.svc";
            IService2 proxy2 = WcfInvokeFactory.CreateServiceByUrl<IService2>(url, BindingTypeEnum.Nettcpbinding);
            var result2 = proxy2.GetData(3);
            Console.WriteLine(result2.ToString());

            #endregion

            Console.ReadKey();
        }

        static IServiceProvider InitServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assembly wcfInterfaceAssembly = Assembly.Load("WCF.IAppService");

            //获取WCF接口类型集
            IEnumerable<Type> types = wcfInterfaceAssembly.GetTypes().Where(type => type.IsInterface);

            //获取服务代理泛型类型
            Type proxyGenericType = typeof(ServiceProxy<>);

            //注册WCF接口
            foreach (Type type in types)
            {
                Type proxyType = proxyGenericType.MakeGenericType(type);
                PropertyInfo propChannel = proxyType.GetProperty(ServiceProxy.ChannelPropertyName, type);

                serviceCollection.AddTransient(proxyType, proxyType);
                serviceCollection.AddTransient(type, factory => propChannel.GetValue(factory.GetService(proxyType)));
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
