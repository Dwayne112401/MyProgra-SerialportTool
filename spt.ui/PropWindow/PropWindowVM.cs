using spt.mvvm.Command;
using spt.tool.Util;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace spt.ui.share.PropWindow
{
    public class PropWindowVM : NotifyPropertyChanged
    {
        private string _messageTitle;
        private string _messageText;
        private string _buttonOkText;
        private string _butttonCancelText;
        private string _buttonWarningText;
        private double _opacity=0.65;

        #region 数据源
        /// <summary>
        /// 提示标题
        /// </summary>
        public string MessageTitle
        {
            get => _messageTitle;
            set => SetProperty(ref _messageTitle, value);
        }

        /// <summary>
        /// 提示内容
        /// </summary>
        public string MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }

        /// <summary>
        /// 确定按钮的文本
        /// </summary>
        public string ButtonOkText
        {
            get => _buttonOkText;
            set => SetProperty(ref _buttonOkText, value);
        }

        /// <summary>
        /// 取消按钮的文本
        /// </summary>
        public string ButtonCancelText
        {
            get => _butttonCancelText;
            set => SetProperty(ref _butttonCancelText, value);
        }

        /// <summary>
        /// 警告文本的提示内容
        /// </summary>
        public string ButtonWarnningText
        {
            get => _buttonWarningText;
            set => SetProperty(ref _buttonWarningText, value);
        }

        /// <summary>
        /// 弹窗的透明度
        /// </summary>
        public double Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }
        #endregion

        #region 动作
        /// <summary>
        /// 关闭浮窗
        /// </summary>
        public ICommand CloseCommand { get; set; }
        private void ExcuteCloseCommand(object args)
        {
            var btn = (args as RoutedEventArgs).OriginalSource as Button;
            var win = VisualTreeUtil.GetParentWindow<Window>(btn);
            if (btn.Content.ToString() == ButtonOkText)
            {
                win.DataContext = true;
            }
            else
            {
                win.DataContext = false;
            }
            win.Close();
        }
        #endregion

        public PropWindowVM()
        {
            CloseCommand = new RelayCommand(ExcuteCloseCommand, null);
        }
    }
}
