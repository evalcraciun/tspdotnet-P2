using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class DialogBoxUpload : Form
    {
        public string FileDescription
        {
            get { return textBoxDescription.Text; }
        }

        public string FileTags
        {
            get { return textBoxTags.Text; }
        }

        public DialogBoxUpload()
        {
            InitializeComponent();
        }
    }
}
