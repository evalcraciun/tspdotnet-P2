using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotosWCF.ApiStatic
{
    public static class ApiStatic
    {
        public static int GetFolderId(string folder)
        {
            using (Model1Container context = new Model1Container())
            {
                var query = from ctx in context.Folders
                            where ctx.Location == folder
                            select ctx;
                return query.FirstOrDefault<Folder>().Id;
            }
        }

        public static File GetFile(string name)
        {
            using (Model1Container context = new Model1Container())
            {
                var query = from ctx in context.Files
                            where ctx.FilePath == name
                            select ctx;
                return query.SingleOrDefault();
            }
        }

        public static File GetFileName(string name)
        {
            using (Model1Container context = new Model1Container())
            {
                context.Configuration.ProxyCreationEnabled = false;
                var query = from ctx in context.Files
                            where ctx.FileName == name
                            select ctx;
                return query.FirstOrDefault();
            }
        }

        public static void DeleteFile(string name)
        {
            using (Model1Container context = new Model1Container())
            {
                context.Files.Remove(context.Files.Single(a => a.FilePath == name));
                context.SaveChanges();
            }
        }

        public static List<File> SearchInDescAndTags(string text)
        {
            using (Model1Container context = new Model1Container())
            {
                context.Configuration.ProxyCreationEnabled = false;
                var items = (from item in context.Files
                             where item.FileDesc.Contains(text) ||
                             item.FileTags.Contains(text)
                             select item);

                return items.Distinct().ToList();
            }
        }

        public static List<File> SearchInDesc(string text)
        {
            using (Model1Container context = new Model1Container())
            {
                context.Configuration.ProxyCreationEnabled = false;
                var items = (from item in context.Files
                             where item.FileDesc.Contains(text)
                             select item);

                return items.Distinct().ToList();
            }
        }

        public static void AddFile(string FileName, string FileDesc, string FileType, string FileSize, string FilePath, string FileTags, int FolderId, string FileDate)
        {
            using (Model1Container context = new Model1Container())
            {
                File f = new File()
                {
                    FileName = FileName,
                    FileDesc = FileDesc,
                    FileType = FileType,
                    FileSize = FileSize,
                    FilePath = FilePath,
                    FileTags = FileTags,
                    FolderId = FolderId,
                    FileDate = FileDate
                };
                context.Files.Add(f);
                context.SaveChanges();
            }
        }

        public static void UpdateFile(string selectedFile, string desc, string tags)
        {
            using (Model1Container context = new Model1Container())
            {
                var query = (from row in context.Files
                             where row.FileName == selectedFile
                             select row).First();
                query.FileDesc = desc;
                query.FileTags = tags;
                context.SaveChanges();
            }
        }

        public static List<Folder> GetAllFolders()
        {
            List<Folder> lp = new List<Folder>();
            using (Model1Container context = new Model1Container())
            {
                context.Configuration.ProxyCreationEnabled = false;
                lp = context.Folders.ToList();
            }

            return lp;
        }

        public static void AddFolder(string folderName, string folderLocation, string folderDate)
        {
            using (Model1Container context = new Model1Container())
            {
                Folder folder = new Folder()
                {
                    Name = folderName,
                    Location = folderLocation,
                    Date = folderDate
                };

                context.Folders.Add(folder);
                context.SaveChanges();
            }
        }
    }
}
