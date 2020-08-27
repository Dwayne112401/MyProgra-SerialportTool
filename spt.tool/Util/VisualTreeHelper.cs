using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace spt.tool.Util
{
    public static  class VisualTreeUtil
    {
        /// <summary>
        /// 得到控件所在的顶级控件
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <returns></returns>
        public static T GetParentWindow<T>(DependencyObject obj) where T:class
        {
            var control = VisualTreeHelper.GetParent(obj);
            while (control != null)
            {
                var parent = VisualTreeHelper.GetParent(control);

                if (parent == null)
                {
                    return control as T;
                }
                control = parent;
            }

           return  null;
        }

    }
}
