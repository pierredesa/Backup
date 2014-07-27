﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Volume
{
    public interface IFile
    {
        string Name();
        IFile Parent { get; set; }
        List<IFile> Children { get; }
        bool IsFile();
        bool IsFolder();
        int FileCount();
        int FolderCount();
        UInt32 Changes { get; }

    }
}
