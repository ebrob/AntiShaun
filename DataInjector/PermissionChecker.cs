using System;
using System.Security;
using System.Security.Permissions;

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