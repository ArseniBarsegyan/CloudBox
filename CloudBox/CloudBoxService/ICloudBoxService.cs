using System.Collections.Generic;
using System.ServiceModel;

namespace CloudBoxService
{
    [ServiceContract]
    public interface ICloudBoxService
    {
        [OperationContract]
        bool ValidatePassword(string username, string password);

        [OperationContract]
        bool UploadFilesToServer(IEnumerable<byte[]> filesCollection);
    }
}
