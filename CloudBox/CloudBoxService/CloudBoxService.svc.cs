using System;
using System.Collections.Generic;
using MySql.Web.Security;
using WebMatrix.WebData;

namespace CloudBoxService
{
    public class CloudBoxService : ICloudBoxService
    {
        public bool ValidatePassword(string username, string password)
        {
            MySqlSimpleMembershipProvider provider1 = new MySqlSimpleMembershipProvider();
            return WebSecurity.Login(username, password);
        }

        public bool UploadFilesToServer(IEnumerable<byte[]> filesCollection)
        {
            throw new NotImplementedException();
        }
    }
}
