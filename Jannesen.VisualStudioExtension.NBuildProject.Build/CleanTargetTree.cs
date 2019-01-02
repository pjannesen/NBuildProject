using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.Build.Framework;
using Jannesen.VisualStudioExtension.NBuildProject.Build.Library;

namespace Jannesen.VisualStudioExtension.NBuildProject.Build
{
    public class CleanTargetTree: BaseTask
    {
        [Required]
        public                  string              TargetDirectory             { get; set;  }

        protected   override    bool                Run()
        {
            string      directory = FullFileName(TargetDirectory);

            if (Directory.Exists(directory)) {
                Log.LogMessage(MessageImportance.High, "Cleanup tree: " + directory);
                _cleanupDirectory(directory);
            }

            return true;
        }

        private                 bool                _cleanupDirectory(string directory)
        {
            List<FileSystemInfo>    entrys = new List<FileSystemInfo>((new DirectoryInfo(directory)).EnumerateFileSystemInfos());
            int                     n = 0;

            foreach (FileSystemInfo entry in entrys) {
                if ((entry.Attributes & (FileAttributes.Directory | FileAttributes.Device | FileAttributes.ReparsePoint | FileAttributes.Hidden | FileAttributes.System)) == FileAttributes.Directory) {
                    string  path = directory + "\\" + entry.Name;

                    if (_cleanupDirectory(path)) {
                        try {
                            Directory.Delete(path);
                            ++n;
                        }
                        catch(Exception err) {
                            throw new BuildException("Can't delete directory: " + path, err);
                        }
                    }
                }
            }

            foreach (FileSystemInfo entry in entrys) {
                if ((entry.Attributes & (FileAttributes.Directory | FileAttributes.Device | FileAttributes.ReparsePoint | FileAttributes.Hidden | FileAttributes.System)) == (FileAttributes)0) {
                    string  path = directory + "\\" + entry.Name;

                    try {
                        File.Delete(path);
                    }
                    catch(Exception err) {
                        throw new BuildException("Can't delete file: " + path, err);
                    }
                    ++n;
                }
            }

            return n == entrys.Count;
        }
    }
}
