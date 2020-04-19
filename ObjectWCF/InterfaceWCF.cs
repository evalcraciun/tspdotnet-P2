using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyPhotosWCF;

namespace ObjectWCF
{
    [ServiceContract]
    [ServiceKnownType(typeof(File))]
    interface InterfaceFile
    {
        [OperationContract]
        File GetFile(string name);

        [OperationContract]
        File GetFileName(string name);

        [OperationContract]
        void DeleteFile(string name);

        [OperationContract]
        List<File> SearchInDescAndTags(string text);
        
        [OperationContract]
        List<File> SearchInDesc(string text);

        [OperationContract]
        void AddFile(string FileName, string FileDesc, string FileType, string FileSize, string FilePath, string FileTags, int FolderId, string FileDate);

        [OperationContract]
        void UpdateFile(string selectedFile, string desc, string tags);
    }

    [ServiceContract]
    [ServiceKnownType(typeof(Folder))]
    interface InterfaceFolder
    {
        [OperationContract]
        int GetFolderId(string folder);

        [OperationContract]
        List<Folder> GetAllFolders();

        [OperationContract]
        void AddFolder(string folderName, string folderLocation, string folderDate);
    }

    [ServiceContract]
    interface IMyPhotos : InterfaceFile, InterfaceFolder
    {
    }
}
