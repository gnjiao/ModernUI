﻿using System;
using System.IO;

namespace Core.Mvvm.Dialogs
{
    public interface ISelectDirectoryDialogService : IGeneralOutputDialogService<DirectoryInfo>
    {
        //IObservable<DialogEventArgs<DirectoryInfo>> Show();
    }
}