using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudBox.WCFService.Helpers;
using MySql.Web.Security;
using WebMatrix.WebData;

namespace CloudBox.WCFService
{
    public class CloudService : ICloudService
    {
        public bool ValidateUser(string username, string password)
        {
            MySqlSimpleMembershipProvider provider = new MySqlSimpleMembershipProvider();
            return WebSecurity.Login(username, password);
        }

        //Get all directories from passed path ('UserName\...')
        public IEnumerable<string> GetAllDirectoriesByPath(string path)
        {
            var directoriesPaths = Directory.GetDirectories(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + path);
            for (var i = 0; i < directoriesPaths.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(directoriesPaths[i]);
                directoriesPaths[i] = fileInfo.Name + "[" + fileInfo.CreationTime;
            }
            return directoriesPaths;
        }

        //Get all files from passed path ('UserName\...')
        public IEnumerable<string> GetAllFilesByPath(string path)
        {
            var filesPaths = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + path);
            for (var i = 0; i < filesPaths.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(filesPaths[i]);
                filesPaths[i] = fileInfo.Name + "[" + fileInfo.CreationTime;
            }
            return filesPaths;
        }

        //Check if Server contains user folder. If not, create it
        public void CheckIfDirectoryWithUserNameExists(string username)
        {
            var usersDirectories = Directory.GetDirectories(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\");
            var directoriesNames = usersDirectories.Select(directoryFullName => new FileInfo(directoryFullName).Name);
            if (!directoriesNames.Contains(username))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + username);
            }
        }

        //Remove file or directory by passed path ('UserName\...\DirectoryName(fileName.extension)')
        public void RemoveElement(string path)
        {
            var fullPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + path;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
        }

        //Create directory by passed path ('UserName\...\DirectoryName')
        public string CreateDirectoryIfNotExists(string path)
        {
            var fullPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + path;
            if (Directory.Exists(fullPath))
            {
                return ConstantHelper.FolderAlreadyExists;
            }
            Directory.CreateDirectory(fullPath);
            return ConstantHelper.FolderCreated;
        }

        //Write content by passed path ('UserName\...\fileName.extension')
        public string Upload(byte[] file, string path)
        {
            File.WriteAllBytes(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + path, file);
            return ConstantHelper.FileUploaded;
        }

        //return file link by passed path ('UserName\...\fileName.extension')
        public string GetFileLink(string path)
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + @"Accounts\" + path;
        }
    }
}
