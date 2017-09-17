﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudBox.WebUI.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.ICloudBoxService")]
    public interface ICloudBoxService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/ValidateUser", ReplyAction="http://tempuri.org/ICloudBoxService/ValidateUserResponse")]
        bool ValidateUser(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/ValidateUser", ReplyAction="http://tempuri.org/ICloudBoxService/ValidateUserResponse")]
        System.Threading.Tasks.Task<bool> ValidateUserAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetAllDirectoriesByPath", ReplyAction="http://tempuri.org/ICloudBoxService/GetAllDirectoriesByPathResponse")]
        string[] GetAllDirectoriesByPath(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetAllDirectoriesByPath", ReplyAction="http://tempuri.org/ICloudBoxService/GetAllDirectoriesByPathResponse")]
        System.Threading.Tasks.Task<string[]> GetAllDirectoriesByPathAsync(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetAllFilesByPath", ReplyAction="http://tempuri.org/ICloudBoxService/GetAllFilesByPathResponse")]
        string[] GetAllFilesByPath(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetAllFilesByPath", ReplyAction="http://tempuri.org/ICloudBoxService/GetAllFilesByPathResponse")]
        System.Threading.Tasks.Task<string[]> GetAllFilesByPathAsync(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/CheckIfDirectoryWithUserNameExists", ReplyAction="http://tempuri.org/ICloudBoxService/CheckIfDirectoryWithUserNameExistsResponse")]
        void CheckIfDirectoryWithUserNameExists(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/CheckIfDirectoryWithUserNameExists", ReplyAction="http://tempuri.org/ICloudBoxService/CheckIfDirectoryWithUserNameExistsResponse")]
        System.Threading.Tasks.Task CheckIfDirectoryWithUserNameExistsAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/UploadFilesToServer", ReplyAction="http://tempuri.org/ICloudBoxService/UploadFilesToServerResponse")]
        bool UploadFilesToServer(string userName, string password, byte[] fileContent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/UploadFilesToServer", ReplyAction="http://tempuri.org/ICloudBoxService/UploadFilesToServerResponse")]
        System.Threading.Tasks.Task<bool> UploadFilesToServerAsync(string userName, string password, byte[] fileContent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/CreateFolderIfNotExists", ReplyAction="http://tempuri.org/ICloudBoxService/CreateFolderIfNotExistsResponse")]
        string CreateFolderIfNotExists(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/CreateFolderIfNotExists", ReplyAction="http://tempuri.org/ICloudBoxService/CreateFolderIfNotExistsResponse")]
        System.Threading.Tasks.Task<string> CreateFolderIfNotExistsAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/RemoveElement", ReplyAction="http://tempuri.org/ICloudBoxService/RemoveElementResponse")]
        void RemoveElement(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/RemoveElement", ReplyAction="http://tempuri.org/ICloudBoxService/RemoveElementResponse")]
        System.Threading.Tasks.Task RemoveElementAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/Upload", ReplyAction="http://tempuri.org/ICloudBoxService/UploadResponse")]
        string Upload(byte[] file, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/Upload", ReplyAction="http://tempuri.org/ICloudBoxService/UploadResponse")]
        System.Threading.Tasks.Task<string> UploadAsync(byte[] file, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetFileLink", ReplyAction="http://tempuri.org/ICloudBoxService/GetFileLinkResponse")]
        string GetFileLink(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudBoxService/GetFileLink", ReplyAction="http://tempuri.org/ICloudBoxService/GetFileLinkResponse")]
        System.Threading.Tasks.Task<string> GetFileLinkAsync(string path);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICloudBoxServiceChannel : CloudBox.WebUI.ServiceReference1.ICloudBoxService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CloudBoxServiceClient : System.ServiceModel.ClientBase<CloudBox.WebUI.ServiceReference1.ICloudBoxService>, CloudBox.WebUI.ServiceReference1.ICloudBoxService {
        
        public CloudBoxServiceClient() {
        }
        
        public CloudBoxServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CloudBoxServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CloudBoxServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CloudBoxServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool ValidateUser(string username, string password) {
            return base.Channel.ValidateUser(username, password);
        }
        
        public System.Threading.Tasks.Task<bool> ValidateUserAsync(string username, string password) {
            return base.Channel.ValidateUserAsync(username, password);
        }
        
        public string[] GetAllDirectoriesByPath(string userName, string path) {
            return base.Channel.GetAllDirectoriesByPath(userName, path);
        }
        
        public System.Threading.Tasks.Task<string[]> GetAllDirectoriesByPathAsync(string userName, string path) {
            return base.Channel.GetAllDirectoriesByPathAsync(userName, path);
        }
        
        public string[] GetAllFilesByPath(string userName, string path) {
            return base.Channel.GetAllFilesByPath(userName, path);
        }
        
        public System.Threading.Tasks.Task<string[]> GetAllFilesByPathAsync(string userName, string path) {
            return base.Channel.GetAllFilesByPathAsync(userName, path);
        }
        
        public void CheckIfDirectoryWithUserNameExists(string username) {
            base.Channel.CheckIfDirectoryWithUserNameExists(username);
        }
        
        public System.Threading.Tasks.Task CheckIfDirectoryWithUserNameExistsAsync(string username) {
            return base.Channel.CheckIfDirectoryWithUserNameExistsAsync(username);
        }
        
        public bool UploadFilesToServer(string userName, string password, byte[] fileContent) {
            return base.Channel.UploadFilesToServer(userName, password, fileContent);
        }
        
        public System.Threading.Tasks.Task<bool> UploadFilesToServerAsync(string userName, string password, byte[] fileContent) {
            return base.Channel.UploadFilesToServerAsync(userName, password, fileContent);
        }
        
        public string CreateFolderIfNotExists(string path) {
            return base.Channel.CreateFolderIfNotExists(path);
        }
        
        public System.Threading.Tasks.Task<string> CreateFolderIfNotExistsAsync(string path) {
            return base.Channel.CreateFolderIfNotExistsAsync(path);
        }
        
        public void RemoveElement(string path) {
            base.Channel.RemoveElement(path);
        }
        
        public System.Threading.Tasks.Task RemoveElementAsync(string path) {
            return base.Channel.RemoveElementAsync(path);
        }
        
        public string Upload(byte[] file, string path) {
            return base.Channel.Upload(file, path);
        }
        
        public System.Threading.Tasks.Task<string> UploadAsync(byte[] file, string path) {
            return base.Channel.UploadAsync(file, path);
        }
        
        public string GetFileLink(string path) {
            return base.Channel.GetFileLink(path);
        }
        
        public System.Threading.Tasks.Task<string> GetFileLinkAsync(string path) {
            return base.Channel.GetFileLinkAsync(path);
        }
    }
}
