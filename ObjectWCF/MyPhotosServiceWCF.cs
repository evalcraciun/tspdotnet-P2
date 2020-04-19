using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using MyPhotosWCF;
using MyPhotosWCF.ApiStatic;

namespace ObjectWCF
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MyPhotosServiceWCF : IMyPhotos
    {
        public void AddFile(string FileName, string FileDesc, string FileType, string FileSize, string FilePath, string FileTags, int FolderId, string FileDate)
        {
           ApiStatic.AddFile(FileName, FileDesc, FileType, FileSize, FilePath, FileTags, FolderId, FileDate);
        }

        public void AddFolder(string folderName, string folderLocation, string folderDate)
        {
            ApiStatic.AddFolder(folderName, folderLocation, folderDate);
        }

        public void DeleteFile(string name)
        {
            ApiStatic.DeleteFile(name);
        }

        public List<Folder> GetAllFolders()
        {
            return ApiStatic.GetAllFolders();
        }

        public File GetFile(string name)
        {
            return ApiStatic.GetFile(name);
        }

        public File GetFileName(string name)
        {
            return ApiStatic.GetFileName(name);
        }

        public int GetFolderId(string folder)
        {
            return ApiStatic.GetFolderId(folder);
        }

        public List<File> SearchInDesc(string text)
        {
            return ApiStatic.SearchInDesc(text);
        }

        public List<File> SearchInDescAndTags(string text)
        {
            return ApiStatic.SearchInDescAndTags(text);
        }

        public void UpdateFile(string selectedFile, string desc, string tags)
        {
            ApiStatic.UpdateFile(selectedFile, desc, tags);
        }
    }
}
