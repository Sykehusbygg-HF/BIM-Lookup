using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BimLookup.Module.Classes
{
    public static class Extensions
    {
        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                try
                {
                value = myobj?.GetType()?.GetProperty(memberInfo.Name)?.GetValue(myobj, null) ?? null;
                propertyInfo.SetValue(x, value, null);
                }
                catch(Exception ex) { Debug.Assert(false, ex.Message); }

            }
            return (T)x;
        }
        public static string BytesToString(this byte[] bytes)
        {

            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;

        }
    }
}
