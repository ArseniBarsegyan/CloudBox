﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudBox.WCFServiceHost.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.ICloudService")]
    public interface ICloudService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/ValidateUser", ReplyAction="http://tempuri.org/ICloudService/ValidateUserResponse")]
        bool ValidateUser(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/GetAllDirectoriesByPath", ReplyAction="http://tempuri.org/ICloudService/GetAllDirectoriesByPathResponse")]
        string[] GetAllDirectoriesByPath(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/GetAllFilesByPath", ReplyAction="http://tempuri.org/ICloudService/GetAllFilesByPathResponse")]
        string[] GetAllFilesByPath(string userName, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/CheckIfDirectoryWithUserNameExists", ReplyAction="http://tempuri.org/ICloudService/CheckIfDirectoryWithUserNameExistsResponse")]
        void CheckIfDirectoryWithUserNameExists(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/UploadFilesToServer", ReplyAction="http://tempuri.org/ICloudService/UploadFilesToServerResponse")]
        bool UploadFilesToServer(string userName, string password, byte[] fileContent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/CreateDirectoryIfNotExists", ReplyAction="http://tempuri.org/ICloudService/CreateDirectoryIfNotExistsResponse")]
        string CreateDirectoryIfNotExists(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/RemoveElement", ReplyAction="http://tempuri.org/ICloudService/RemoveElementResponse")]
        void RemoveElement(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/Upload", ReplyAction="http://tempuri.org/ICloudService/UploadResponse")]
        string Upload(byte[] file, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICloudService/GetFileLink", ReplyAction="http://tempuri.org/ICloudService/GetFileLinkResponse")]
        string GetFileLink(string path);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICloudServiceChannel : CloudBox.WCFServiceHost.ServiceReference1.ICloudService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CloudServiceClient : System.ServiceModel.ClientBase<CloudBox.WCFServiceHost.ServiceReference1.ICloudService>, CloudBox.WCFServiceHost.ServiceReference1.ICloudService {
        
        public CloudServiceClient() {
        }
        
        public CloudServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CloudServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CloudServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CloudServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool ValidateUser(string username, string password) {
            return base.Channel.ValidateUser(username, password);
        }
        
        public string[] GetAllDirectoriesByPath(string userName, string path) {
            return base.Channel.GetAllDirectoriesByPath(userName, path);
        }
        
        public string[] GetAllFilesByPath(string userName, string path) {
            return base.Channel.GetAllFilesByPath(userName, path);
        }
        
        public void CheckIfDirectoryWithUserNameExists(string username) {
            base.Channel.CheckIfDirectoryWithUserNameExists(username);
        }
        
        public bool UploadFilesToServer(string userName, string password, byte[] fileContent) {
            return base.Channel.UploadFilesToServer(userName, password, fileContent);
        }
        
        public string CreateDirectoryIfNotExists(string path) {
            return base.Channel.CreateDirectoryIfNotExists(path);
        }
        
        public void RemoveElement(string path) {
            base.Channel.RemoveElement(path);
        }
        
        public string Upload(byte[] file, string path) {
            return base.Channel.Upload(file, path);
        }
        
        public string GetFileLink(string path) {
            return base.Channel.GetFileLink(path);
        }
    }
}
