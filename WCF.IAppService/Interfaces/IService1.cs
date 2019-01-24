using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace WCF.IAppService.Interfaces
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        // TODO: 在此添加您的服务操作
    }

}
