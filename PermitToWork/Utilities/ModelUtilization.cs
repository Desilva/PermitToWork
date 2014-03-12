using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PermitToWork.Utilities
{
    public static class ModelUtilization
    {
        public static void Clone(object from, object to)
        {
            Type t1 = to.GetType();
            Type t = from.GetType();
            foreach (PropertyInfo info in t1.GetProperties())
            {
                if (t.GetProperty(info.Name) != null)
                {
                    info.SetValue(to, t.GetProperty(info.Name).GetValue(from, null), null);
                }
            }
        }
    }
}