using System.Collections.Generic;
using System.ServiceModel;

namespace CloudBox.WCFService
{    
    [ServiceContract]
    public interface ICloudService
    {
        [OperationContract]
        bool ValidateUser(string username, string password);

        [OperationContract]
        IEnumerable<string> GetAllDirectoriesByPath(string path);

        [OperationContract]
        IEnumerable<string> GetAllFilesByPath(string path);

        [OperationContract]
        void CheckIfDirectoryWithUserNameExists(string username);

        [OperationContract]
        string CreateDirectoryIfNotExists(string path);

        [OperationContract]
        void RemoveElement(string path);

        [OperationContract]
        string Upload(byte[] file, string path);

        [OperationContract]
        string GetFileLink(string path);
    }
}
