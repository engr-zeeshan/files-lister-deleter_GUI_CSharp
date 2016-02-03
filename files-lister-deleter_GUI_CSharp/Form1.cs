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

namespace files_lister_deleter_GUI_CSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path = "";
        string ext = "";
        
        /* 
         * replacePathString(string[])
         * function responsible for replacing user provided path by empty string
         */
        private string[] replacePathString(string[] rootDir)
        {
            for (int i = 0; i < rootDir.Length; i++)
            {
                rootDir[i] = rootDir[i].Replace(path, "");
            }
            return rootDir;
        }

        /*  
        * traceFiles(string,string)
        * function responsible to detect files and then find for sub-directories
        * if sub-directories are found, then its a recursive function
        */

        private void traceFiles(string rootDir, string extension)
        {

            string[] files;
            string[] dirs;
    
            files = Directory.GetFiles(rootDir, "*." + extension);
            
            // call replacePathString function to replace the path
            files = replacePathString(files);

            // populate the checkedlistbox1
            checkedListBox1.Items.AddRange(files);

            // check for sub directories
            dirs = Directory.GetDirectories(rootDir);

            // if sub-directories are found, search for files in those too
            if (dirs.Length != 0)
            {
                foreach (string d in dirs)
                {
                    traceFiles(d, extension);
                }
            }
            return;
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            // clear the list if already exists
            checkedListBox1.Items.Clear();

            // get path provided by user in text box
            path = text_box_path.Text;
            // get extension provided by user in text box
            ext = text_box_extension.Text;
            
            // display message box if path or extension is empty
            if (path == "" || ext == "") {
                MessageBox.Show("Please provide both path and extension","Required Info",
                    MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            
            //check for exception of path provided by user
            try
            {
                Directory.GetDirectories(path);
            }
            catch (Exception ex)
            {
                if (ex is DirectoryNotFoundException)
                    MessageBox.Show("Path not found");
                else if (ex is UnauthorizedAccessException)
                    MessageBox.Show("Unauthorized access");
                else if (ex is IOException)
                    MessageBox.Show("IO Exception");
                else
                    throw;
                return;
            }
            // call the function to trace root folder for given extension of files
            traceFiles(path, ext);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            // delete each file selected by user
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                //add path with itemChekced to complete the path that was replace earlier
                File.Delete(path + (string)itemChecked);
            }

            // clear the list if already exists
            checkedListBox1.Items.Clear();

            // call the function to trace from the root folder again for given extension of files
            traceFiles(path, ext);
        }

        private void btn_browser_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                path = folderBrowserDialog1.SelectedPath;
                text_box_path.Text = path;
            }
        }
    }
}
