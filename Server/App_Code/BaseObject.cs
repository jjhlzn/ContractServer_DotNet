using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// BaseObject 的摘要说明
/// </summary>
public abstract class BaseObject
{
    public void resetNullProperty(string name)
    {
        //对属性get值
        PropertyInfo pii = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        if (null != pii && pii.CanRead)
        {
            object obj_Name = pii.GetValue(this, null);
            if (obj_Name == null)
            {
                //对属性set值
                PropertyInfo pi = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (null != pi && pi.CanWrite)
                {
                    pi.SetValue(this, "", null);
                }
            }
        }
    }

    public void resetNullProperties()
    {
        string[] properties = GetNeedResetProperties();
        foreach (var propertyName in properties)
        {
            resetNullProperty(propertyName);
        }
    }

    public abstract string[] GetNeedResetProperties();
}