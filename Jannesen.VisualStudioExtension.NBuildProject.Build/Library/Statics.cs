using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jannesen.VisualStudioExtension.NBuildProject.Build.Library
{
    static class Statics
    {
        public  static          string              NormelizeFullPath(string path)
        {
            var     parts = new List<string>(path.Split(new char[] { '/', '\\'}));
            int     rootlength;

            if (parts[0].Length == 2 && parts[0][1] == ':')
                rootlength = 1;
            else
            if (parts.Count > 3 && parts[0].Length == 0 && parts[1].Length == 0)
                rootlength = 4;
            else
                throw new ArgumentException("Invallid full path '" + path + "'.");

            for (int i = 0 ; i < parts.Count ; ) {
                switch(parts[i]) {
                case ".":
                    parts.RemoveAt(i);
                    break;

                case "..":
                    if (rootlength > i-1)
                        throw new ArgumentException("Invallid full path '" + path + "'.");

                    parts.RemoveRange(i-1, 2);
                    i -= 1;
                    break;

                default:
                    ++i;
                    break;
                }
            }

            var rtn = new StringBuilder();

            for (int i = 0 ; i < parts.Count ; ++i) {
                if (i > 0)
                    rtn.Append('\\');

                rtn.Append(parts[i]);
            }

            return rtn.ToString();
        }
    }
}
