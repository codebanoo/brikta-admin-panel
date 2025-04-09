using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrameWork
{
    public class FolderManager
    {
        public static void EmptyFolder(DirectoryInfo directory)
        {
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    EmptyFolder(subdirectory);
                    subdirectory.Delete();
                }
            }
            catch (Exception exc)
            { }
        }
    }
}
