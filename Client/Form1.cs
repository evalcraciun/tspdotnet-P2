using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using MyPhotosWCF;

namespace GUI
{
    public partial class Form1 : Form
    {
        MyPhotosClient client = new MyPhotosClient();
        private DateTime dt;
        private string dirSelected;
        private string fileNameSelected;
        public Form1()
        {
            InitializeComponent();
            //PopulateTreeView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TreeNode rootNode;
            var folders = client.GetAllFolders();
            foreach (var x in folders)
            {
                System.IO.DirectoryInfo info = new DirectoryInfo(x.Location);
                if (info.Exists)
                {
                    rootNode = new TreeNode(info.Name);
                    rootNode.Tag = info;
                    treeView1.Nodes.Add(rootNode);
                }
            }
        }

        private void createFolder(object sender, EventArgs e)
        {
            MyPhotosClient client = new MyPhotosClient();
            DialogBox dlg = new DialogBox();
            if (dlg.ShowDialog() == DialogResult.Yes)
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string folderPath = Path.Combine(projectDirectory, dlg.FolderName);

                if (!Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    dt = Directory.GetCreationTime(folderPath);

                    //using client API
                    client.AddFolder(dlg.FolderName, folderPath, dt.ToString());

                    string message = "Folder created succesfully!";
                    string caption = "Success";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    // MessageBox result;
                    MessageBox.Show(message, caption, buttons);
                    treeView1.Refresh();
                } else
                {
                    //show a message to user
                    MessageBox.Show("Your folder already exists. Please provide other name!", "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView1.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            dirSelected = nodeDirInfo.FullName;

            ListViewItem item = null;
            ListViewItem.ListViewSubItem lvi;

            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                // name of file
                string fName = Path.GetFileNameWithoutExtension(file.Name);
                item = new ListViewItem(fName, 1);

                // type of file .png, .jpg, .mp4
                lvi = new ListViewItem.ListViewSubItem();
                lvi.Text = file.Extension;
                item.SubItems.Add(lvi);

                // size of file in Bytes
                lvi = new ListViewItem.ListViewSubItem();
                lvi.Text = file.Length.ToString();
                item.SubItems.Add(lvi);

                //DateTime of file
                lvi = new ListViewItem.ListViewSubItem();
                lvi.Text = file.LastAccessTime.ToString();
                item.SubItems.Add(lvi);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void uploadFile(object sender, EventArgs e)
        {
            //open a default directory
            DirectoryInfo info = new DirectoryInfo(@"../..");
            int myNodeCount = treeView1.Nodes.Count;

            if (myNodeCount == 1)
            {
                //save this path in a global var
                dirSelected = info.Parent.FullName + "\\" + treeView1.SelectedNode.FullPath;
            }

            //Make use of OpenFileDialog and select images and media
            string fileContent = string.Empty;
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;|Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                // Remove path from the file name.
                string fName = Path.GetFileName(filePath);
                string currentFilePath = filePath.Remove(filePath.IndexOf(fName));

                //copy file from one location to selected dir
                System.IO.File.Copy(Path.Combine(currentFilePath, fName), Path.Combine(dirSelected, fName), true);
                FileInfo fi = new FileInfo(filePath);
                string justFileName = Path.GetFileNameWithoutExtension(fi.Name);

                // New file location   
                string fullFileName = dirSelected + "\\" + justFileName;
                // Get file extension   
                string extn = fi.Extension;
                // Get file size  
                var size = fi.Length.ToString();
                // Creation, last access, and last write time   
                DateTime creationTime = fi.CreationTime;

                DialogBoxUpload upload = new DialogBoxUpload();
                if (upload.ShowDialog() == DialogResult.Yes)
                {
                    //save file to DB
                    string fiDesc = upload.FileDescription;
                    string fileTags = upload.FileTags;
                    
                    //use host api
                    var folderId = client.GetFolderId(dirSelected);
                    var sysDate = DateTime.Now.ToString();

                    //use host api
                    client.AddFile(justFileName, fiDesc, extn, size, fullFileName, fileTags, folderId, sysDate);
                    // Prompt a message to user;
                    MessageBox.Show("File saved to DB", "Success", MessageBoxButtons.OK);
                }
            }
        }

        private void fileSelected(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
            foreach (ListViewItem item in items)
            {
                fileNameSelected = item.Text;
                //use host api
                var fileName = client.GetFileName(item.Text);
                var fileP = fileName.FilePath + fileName.FileType;
                if (fileP.EndsWith(".png") || fileP.EndsWith(".jpg") || fileP.EndsWith(".PNG") || fileP.EndsWith(".JPG"))
                {
                    Image file = Image.FromFile(fileP);
                    pictureBox1.Image = file;
                    textBoxFileDesc.Text = fileName.FileDesc;
                    textBoxFileTags.Text = fileName.FileTags;
                }
                else
                {
                    pictureBox1.Image = null;
                    textBoxFileDesc.Text = fileName.FileDesc;
                    textBoxFileTags.Text = fileName.FileTags;
                }

            }
        }

        private void updateFileInfo(object sender, EventArgs e)
        {
            string desc = textBoxFileDesc.Text;
            string tags = textBoxFileTags.Text;

            //use host api
            client.UpdateFile(fileNameSelected, desc, tags);
            MessageBox.Show("File Updated to DB", "Success", MessageBoxButtons.OK);
        }

        private void onSearch(object sender, EventArgs e)
        {
            string toSearch = textBoxSearch.Text;
            bool withTags = checkBoxSearch.Checked;
            var sqlQuery = client.SearchInDesc(toSearch);
            var query = client.SearchInDescAndTags(toSearch);

            //decide where to search your word
            if (withTags)
            {
                //use host api
                sqlQuery = client.SearchInDescAndTags(toSearch);
            }

            DirectoryInfo di = new DirectoryInfo(dirSelected);

            ListViewItem item = null;
            ListViewItem.ListViewSubItem lvi;
            listView1.Items.Clear();

            foreach (FileInfo file in di.GetFiles())
            {
                string fName = Path.GetFileNameWithoutExtension(file.Name);
                foreach (var text in query)
                {
                    if (fName == text.FileName)
                    {

                        item = new ListViewItem(fName, 1);

                        lvi = new ListViewItem.ListViewSubItem();
                        lvi.Text = file.Extension;
                        item.SubItems.Add(lvi);

                        lvi = new ListViewItem.ListViewSubItem();
                        lvi.Text = file.Length.ToString();
                        item.SubItems.Add(lvi);

                        lvi = new ListViewItem.ListViewSubItem();
                        lvi.Text = file.LastAccessTime.ToString();
                        item.SubItems.Add(lvi);
                        listView1.Items.Add(item);
                    }
                }
            }
        }

        private void deleteFile(object sender, EventArgs e)
        {
            //sql to delete folder from DB
            var newSelected = treeView1.SelectedNode;
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            string path = nodeDirInfo.ToString() + "\\" + listView1.SelectedItems[0].Text;

            DialogResult result;
            result = MessageBox.Show("Do you want to delete this file ?", "Info", MessageBoxButtons.YesNo);

            string fpath = client.GetFile(path).FilePath + client.GetFile(path).FileType;

            if (result == DialogResult.Yes)
            {
                // delete form DB
                client.DeleteFile(path);
                try
                {
                    // 
                    pictureBox1.Image = null;
                    // If file found, delete it
                    System.IO.File.SetAttributes(fpath, FileAttributes.Normal);
                    System.IO.File.Delete(fpath);
                }

                catch (Exception exp)
                {

                    Console.WriteLine("Gettign some exp", exp);

                }

            }
        }

        private void nodeRightClick(object sender, MouseEventArgs e)
        {
            // Make sure this is the right button.
            if (e.Button != MouseButtons.Right) return;
            contextMenuStrip1.Show(this, new Point(e.X + 150, e.Y + 30));
        }
    }
}
