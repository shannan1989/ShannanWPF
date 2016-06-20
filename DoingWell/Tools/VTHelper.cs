using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Shannan.DoingWell.Tools
{
    class VTHelper
    {
        /// <summary>
        /// 查找某种类型的子控件，并返回一个List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            List<T> childList = new List<T>();
            DependencyObject child = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && ((T)child).GetType() == typename)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

    }
}
