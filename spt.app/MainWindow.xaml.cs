using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SerialportTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TxtReceive_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            txtReceive.ScrollToEnd();
        }
    }
}
