using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common
{
    public class PermissionObject
    {
        public Permission Permission { get; private set; }
        public string Description
        {
            get
            {
                FieldInfo field = Permission.GetType().GetField(Permission.ToString());
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
                return attribute?.Description ?? Permission.ToString();
            }
        }

        public PermissionObject(Permission permission)
        {
            Permission = permission;
        }

        public static implicit operator PermissionObject(Permission permission)
        {
            return new PermissionObject(permission);
        }
    }

    public enum Permission
    {
        [Description("CanInsert")]
        Insert,
        [Description("CanEdit")]
        Edit,
        [Description("CanDelete")]
        Delete,
        [Description("CanView")]
        View
    }
}
