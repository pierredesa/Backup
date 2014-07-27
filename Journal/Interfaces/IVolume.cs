﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Interfaces
{
    public interface IVolume : IDisposable
    {
        void Map_Volume();
        System.IO.DriveInfo Drive { get;  }
        Journal.Volume.IFile Root { get; }
        //Changes are the changes which have occured since the volume was created
        List<Journal.Volume.IFile> Changes { get; }
    }
}
