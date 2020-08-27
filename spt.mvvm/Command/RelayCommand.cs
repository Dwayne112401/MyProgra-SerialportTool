using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace spt.mvvm.Command
{
    /// <summary>
    /// 普通命令
    /// </summary>
    public class RelayCommand :ICommand
    {
        private Action<object> _excute;
        private Func<object, bool> _canExcute;

        /// <summary>
        /// 当发生更改时，发生影响是否应该执行该命令
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
      
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="excute"></param>
        /// <param name="canExcute"></param>
        public RelayCommand(Action<object> excute, Func<object, bool> canExcute)
        {
            this._excute = excute;
            this._canExcute = canExcute;
        }

        /// <summary>
        /// 定义确定此命令是否可在其当前状态下执行的方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExcute == null || _canExcute.Invoke(parameter);
        }

        /// <summary>
        /// 定义在调用此命令时要调用的方法
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _excute?.Invoke(parameter);
        }
    }
}
