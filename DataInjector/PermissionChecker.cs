using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DataInjector
{
    public class PermissionChecker
    {
        public static bool HasWritePermission(string path)
        {
            var permissions = new PermissionSet(PermissionState.None);
            var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
            permissions.AddPermission(writePermission);

            return permissions.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }
    }
}