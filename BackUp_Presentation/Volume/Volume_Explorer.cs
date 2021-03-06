﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUp
{
    public partial class Volume_Explorer : Form
    {
         Journal.NTFSVolume _Volume;
        List<Utilities.NetworkDrive> ManuallyMappedDrives;
        public Volume_Explorer()
        {
            InitializeComponent();
            ManuallyMappedDrives = new List<Utilities.NetworkDrive>();
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if(_Volume != null)
                _Volume.Dispose();
        }
        private void selectDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var s = new Select_Drive(Selected_Drive);
            s.ShowDialog(this);
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_Volume == null)
                MessageBox.Show("You have not selected a drive yet.");
            else
            {
                _Volume.Map_Volume();
                TreeNode rootNode = new TreeNode(_Volume.Root.Name);
                rootNode.Tag = _Volume.Root;
                GetDirs(_Volume.Root.Children, rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }
        private void Selected_Drive(string path)
        {
            try
            {
                if(_Volume != null)
                {
                    _Volume.Dispose();
                    treeView1.Nodes.Clear();
                }
                var f = ManuallyMappedDrives.FirstOrDefault();
                _Volume = new Journal.NTFSVolume(path.Substring(0, 1));
 
            } catch(Exception e)
            {
                if(e.Message.ToLower() == "access is denied")
                    MessageBox.Show("You must run this program as an administrator to access that functionality.");
                else  MessageBox.Show(e.Message);
            }

        }

        private void GetDirs(List<Journal.Volume.NTFS_File> files, TreeNode n)
        {
            TreeNode aNode;
            foreach(var subDir in files.Where(a => a.IsFolder))
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                var subSubDirs = subDir.Children;
                if(subSubDirs.Any())
                    GetDirs(subSubDirs, aNode);

                n.Nodes.Add(aNode);
            }
        }
        void treeView1_NodeMouseClick(object sender,
            TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView1.Items.Clear();
            var nodeDirInfo = (Journal.Volume.NTFS_File)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach(var dir in nodeDirInfo.Children.Where(a => a.IsFolder))
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(item, dir.FolderCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, dir.FileCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, "Directory")
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach(var file in nodeDirInfo.Children.Where(a => a.IsFile))
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(item, file.FolderCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, file.FileCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, "File")
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_Volume == null)
                MessageBox.Show("You have not selected a drive yet.");
            else
            {
                var old = _Volume.Refresh();
                var change_files = _Volume.Update(old);

            }

        }

        private void volumeFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new BackUp_Presentation.Volume.Volume_Mapper(ManuallyMappedDrives.Add);
            f.Show();
        }




    }
}
