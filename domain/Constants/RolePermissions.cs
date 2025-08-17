using System.Collections.Generic;

namespace AuctionSystem.Domain.Constants
{
    // كلاس يحتوي على صلاحيات كل دور في النظام
    public static class RolePermissions
    {
        // قاموس يربط اسم الدور بالقائمة الخاصة بالصلاحيات
        private static readonly Dictionary<string, List<string>> _permissions = new()
        {
            { "Admin", new List<string> { "CreateProject", "ApproveBid", "ManageUsers", "DeleteProject" } },
            { "BuyerUser", new List<string> { "SubmitBid", "ViewProjects" } },
            { "SellerUser", new List<string> { "CreateProject", "ViewBids" } }
        };

        // دالة لإرجاع قائمة الصلاحيات لدور معين
        public static List<string> GetPermissionsForRole(string roleName)
        {
            if (_permissions.ContainsKey(roleName))
                return _permissions[roleName];

            return new List<string>(); // إرجاع قائمة فارغة إذا الدور غير موجود
        }
    }
}
