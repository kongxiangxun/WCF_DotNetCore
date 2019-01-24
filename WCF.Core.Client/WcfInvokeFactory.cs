using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WCF.Core.Client
{
    public class WcfInvokeFactory
    {
        #region WCF服务工厂
        public static T CreateServiceByUrl<T>(string url)
        {
            return CreateServiceByUrl<T>(url, BindingTypeEnum.Basichttpbinding);
        }

        public static T CreateServiceByUrl<T>(string url, BindingTypeEnum bingType)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) throw new NotSupportedException("This url is not Null or Empty!");
                EndpointAddress address = new EndpointAddress(url);
                Binding binding = CreateBinding(bingType);
                ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
                return factory.CreateChannel();
            }
            catch (Exception ex)
            {
                throw new Exception("创建服务工厂出现异常.");
            }
        }
        #endregion

        #region 创建传输协议
        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">传输协议名称</param>
        /// <returns></returns>
        private static Binding CreateBinding(BindingTypeEnum bingdType)
        {
            Binding bindinginstance = null;
            if (bingdType == BindingTypeEnum.Basichttpbinding)
            {
                BasicHttpBinding ws = new BasicHttpBinding();
                ws.MaxBufferSize = 2147483647;
                ws.MaxBufferPoolSize = 2147483647;
                ws.MaxReceivedMessageSize = 2147483647;
                ws.ReaderQuotas.MaxStringContentLength = 2147483647;
                ws.CloseTimeout = new TimeSpan(0, 30, 0);
                ws.OpenTimeout = new TimeSpan(0, 30, 0);
                ws.ReceiveTimeout = new TimeSpan(0, 30, 0);
                ws.SendTimeout = new TimeSpan(0, 30, 0);

                bindinginstance = ws;
            }
            else if (bingdType == BindingTypeEnum.Nettcpbinding)
            {
                NetTcpBinding ws = new NetTcpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Mode = SecurityMode.None;
                bindinginstance = ws;
            }
            return bindinginstance;

        }
        #endregion
    }
    public enum BindingTypeEnum
    {
        Basichttpbinding = 0,
        Nettcpbinding = 1,
        Wshttpbinding = 2
    }
}
