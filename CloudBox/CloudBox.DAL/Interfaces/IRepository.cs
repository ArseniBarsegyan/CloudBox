using System;
using System.Collections.Generic;

namespace CloudBox.DAL.Interfaces
{
    public interface IRepository
    {
        ICollection<string> GetAllFilesByUserId(int id);
        void WriteDataToDataBase(int userId, string fileName, byte[] data, DateTime uploadTime);
        void RenameFileInDataBase(int userId, string oldFileName, string newFileName);
    }
}