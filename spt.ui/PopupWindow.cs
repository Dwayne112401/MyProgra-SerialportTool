using System.Windows;
using System.Windows.Media;

namespace spt.ui.share
{
    /// <summary>
    ///  弹窗
    /// </summary>
    public static  class PopupWindow
    {
        /// <summary>
        /// 弹窗提示
        /// </summary>
        /// <param name="window"></param>
        /// <param name="content">为窗体的内容</param>
        /// <returns>返回的数值为，弹窗关闭后，content的DataContext(是一个bool值)</returns>
        public static  bool  SafeShowPopupWindow(this Window window,object content)
        {
            var win = new Window()
            {
                Content = content,
                Width = window.Width,
                Height = window.Height,
                Left = window.Left,
                Top = window.Top,
                WindowStyle = WindowStyle.None,
                Background = Brushes.Transparent,
                AllowsTransparency = true,
            };

            //打开一个窗口，并关闭新打开的窗口时，才返回
            bool bwin = !(bool)win.ShowDialog();

            //点击窗体的“确定”按钮，则窗体的DataContext==true
            return bwin && (bool)win.DataContext;
        }
    }
}
