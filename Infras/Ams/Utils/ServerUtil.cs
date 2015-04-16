using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Ams.Utils
{
    public static class ServerUtil
    {
        private static string _rootPath;

        public static string MapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            return null;
        }

        public static string RootPath()
        {
            if (_rootPath == null)
            {
                _rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string newValue = Path.DirectorySeparatorChar.ToString();
                _rootPath = _rootPath.Replace("/", newValue);
            }
            return _rootPath;
        }

        
    }
}
