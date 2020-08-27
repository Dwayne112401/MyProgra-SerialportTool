using spt.model;
using spt.mvvm.Command;
using spt.tool.Serialport;
using spt.tool.Util;
using spt.ui.share;
using spt.ui.share.PropWindow;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace spt.vm
{
    public class MainWindowVM: NotifyPropertyChanged
    {
        private SerialportParameterList _serialportParameter;
        private CurrentSerialportParamter _currentSerialportParamter;
        private MySerialport mySerialport;
        private string _sendText="";
        private string _receiveText="";
        private int _cycleTime=0;
        private Window window;

        #region 数据源
        /// <summary>
        /// 串口配置列表
        /// </summary>
        public SerialportParameterList SerialportParameterList
        {
            get => _serialportParameter;
            set => SetProperty(ref _serialportParameter, value);
        }

        /// <summary>
        /// 当前选择的串口配置
        /// </summary>
        public CurrentSerialportParamter CurrentSerialportParamter
        {
            get => _currentSerialportParamter;
            set => OnPropertyChanged();
        }


        /// <summary>
        /// 发送数据
        /// </summary>
        public string SendText
        {
            get => _sendText;
            set => SetProperty(ref _sendText,value);
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        public string ReceiveText
        {
            get => _receiveText;
            set => SetProperty(ref _receiveText,value);
        }
        private void  ReceiveTextDelegate(string text)
        {
            ReceiveText+= text;
            ReceiveByteCount = mySerialport.ReceiveByteCount;
        }

        /// <summary>
        /// 发送的数据格式
        /// </summary>
        public bool SendModel
        {
            get
            {
                return mySerialport.SendModel==MySerialport.DataModel.Hex?true:false;
            }
            set
            {
                if (value == true)
                {
                    mySerialport.SendModel = MySerialport.DataModel.Hex;
                    SendText = ScaleUtil.TextToHex(SendText);
                }
                else
                {
                    mySerialport.SendModel = MySerialport.DataModel.Text;
                    SendText = ScaleUtil.HexToText(SendText);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 接受的数据格式
        /// </summary>
        public bool ReceiveModel
        {
            get
            {
                return mySerialport.RecieveModel == MySerialport.DataModel.Hex ? true : false;
            }
            set
            {
                if (value == true)
                {
                    mySerialport.RecieveModel = MySerialport.DataModel.Hex;
                    ReceiveText = ScaleUtil.TextToHex(ReceiveText);
                }
                else
                {
                    mySerialport.RecieveModel = MySerialport.DataModel.Text;
                    ReceiveText = ScaleUtil.HexToText(ReceiveText);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 发送的统计总数
        /// </summary>
        public long SendByteCount
        {
            get => mySerialport.SendByteCount;
            set => OnPropertyChanged();
        }

        /// <summary>
        /// 接受的统计总数
        /// </summary>
        public long ReceiveByteCount
        {
            get => mySerialport.ReceiveByteCount;
            set => OnPropertyChanged();
        }

        /// <summary>
        /// 自动发送的时间
        /// </summary>
        public int CycleTime
        {
            get => _cycleTime;
            set
            {
                if (_cycleTime < 10) _cycleTime = 10;
                SetProperty(ref _cycleTime, value);
            }
        }

        /// <summary>
        /// 串口打开是否成功
        /// </summary>
        public bool IsOpen
        {
            get => mySerialport.IsOpen;
            set => OnPropertyChanged();
        }
        #endregion

        #region 动作
        /// <summary>
        /// 打开端口
        /// </summary>
        public ICommand OpenCommand { get; set; }
        private void ExcuteOpenCommand(object args)
        {
            try
            {
                var btn = (args as RoutedEventArgs).OriginalSource as Button;
                if (btn.Content.ToString() == "打开端口")
                {
                    mySerialport.Open(CurrentSerialportParamter.PortName,
                                      CurrentSerialportParamter.BaudRate,
                                      CurrentSerialportParamter.DataBits,
                                      CurrentSerialportParamter.Parity,
                                      CurrentSerialportParamter.StopBits);

                    //TODO:为何要两个都复制，Button 的 Content 才会变化
                    btn.Content = "关闭端口";
                }
                else
                {
                    mySerialport.Close();
                    btn.Content = "打开端口";
                }

                //打开发送按钮
                IsOpen = mySerialport.IsOpen;
            }
            catch (Exception ex)
            {
                var content = new WarningWindow()
                {
                    DataContext = new PropWindowVM()
                    {
                        MessageTitle = "错误信息",
                        MessageText = ex.Message,
                        ButtonWarnningText="我知道了",
                        Opacity = 0.7,
                    },
                };

                window.SafeShowPopupWindow(content);
            }
        }
        private bool CancelOpenCommand(object args)
        {
            return true;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public ICommand SendCommand { get; set; }
        private void ExcuteSendCommand(object args)
        {
            mySerialport.Send(SendText);
        }
        private bool CancelSendComand(object args)
        {
            return true;
        }

        /// <summary>
        /// 自动发送
        /// </summary>
        public ICommand AutoSendCommand { get; set; }
        private bool IsAutoSend = false;
        public async  void ExcuteAutoSendCommand(object args)
        {
            var btn = (args as RoutedEventArgs).OriginalSource as Button;
            IsAutoSend = !IsAutoSend;
            btn.Content = IsAutoSend ? "停止发送" : "自动发送";
            await Task.Run(() =>
            {
                while (IsAutoSend==true)
                {
                    mySerialport.Send(SendText);
                    SendByteCount = mySerialport.SendByteCount;
                    System.Threading.Thread.Sleep(CycleTime);
                }
            });
        }

        /// <summary>
        /// 清空发送区
        /// </summary>
        public ICommand ClearSendCommand { get; set; }
        private void ExcuteClearSendCommand(object args)
        {
            SendText = "";
        }

        /// <summary>
        /// 清空接受取
        /// </summary>
        public ICommand ClearReceiveCommand { get; set; }
        private void ExcuteClearReceiveCommand(object args)
        {
            ReceiveText = "";
        }

        /// <summary>
        /// 清零统计
        /// </summary>
        public ICommand ClearByteCountCommand { get; set; }
        private void ExcuteClearByteCount(object args)
        {
            SendByteCount =mySerialport.SendByteCount= 0;
            ReceiveByteCount =mySerialport.ReceiveByteCount= 0;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public ICommand CloseCommand { get; set; }
        private void ExcuteCloseCommand(object args)
        {
            var content = new MessageWindow()
            {
                DataContext = new PropWindowVM()
                {
                    MessageTitle = "提示信息",
                    MessageText = "是否关闭窗体？请点击'确定'或者'取消'",
                    ButtonOkText = "确定",
                    ButtonCancelText = "取消",
                    Opacity = 0.7,
                },
            };
            if (window.SafeShowPopupWindow(content) == true)
            {
                window.Close();
            }
        }

        public ICommand LoadCommand { get; set; }
        private void ExcuteLoadCommand(object args)
        {
            var img = (args as RoutedEventArgs).OriginalSource as Image;
            window = VisualTreeUtil.GetParentWindow<Window>(img);
        }
        #endregion


        #region .ctor

        public MainWindowVM()
        {
            //串口操作对象
            mySerialport = new MySerialport();

            //预加载参数
            var partityList = new List<Parity>();
            foreach (Parity o in Enum.GetValues(typeof(Parity)))
            {
                partityList.Add(o);
            }
            var stopBits = new List<StopBits>();
            foreach (StopBits o in Enum.GetValues(typeof(StopBits)))
            {
                stopBits.Add(o);
            }
            _serialportParameter = new SerialportParameterList()
            {
                PortNameList = new List<string>(mySerialport.PortNameList),
                BaudRateList = new List<int>()
                {
                    600, 1200, 2400, 4800, 9600,
                    14400, 19200, 28800, 38400,
                    57600, 115200, 230400, 460800,
                },
                DataBitsList = new List<int>()
                {
                    5, 6, 7, 8
                },
                ParityList   = partityList,
                StopBitsList = stopBits,
            };

            //当前参数
            _currentSerialportParamter = new CurrentSerialportParamter()
            {
                PortName = _serialportParameter.PortNameList[0],
                BaudRate = _serialportParameter.BaudRateList[4],
                DataBits = _serialportParameter.DataBitsList[3],
                Parity   = _serialportParameter.ParityList[0],
                StopBits = _serialportParameter.StopBitsList[1],
            };

            //启动接受数据线程
            mySerialport.ReceiveAsync(ReceiveTextDelegate);

            //初始化字段
            SendText = "";
            ReceiveText = "";
            CycleTime = 10;

            //加载得到窗体
            LoadCommand = new RelayCommand(ExcuteLoadCommand, null);

            //关闭窗体
            CloseCommand = new RelayCommand(ExcuteCloseCommand, null);

            //打开/关闭串口
            OpenCommand = new RelayCommand(ExcuteOpenCommand, CancelOpenCommand);

            //发送数据
            SendCommand = new RelayCommand(ExcuteSendCommand, CancelSendComand);

            //自动发送数据
            AutoSendCommand = new RelayCommand(ExcuteAutoSendCommand, null);

            //清除发送区
            ClearSendCommand = new RelayCommand(ExcuteClearSendCommand, null);

            //清除接受区
            ClearReceiveCommand = new RelayCommand(ExcuteClearReceiveCommand, null);

            //清零统计
            ClearByteCountCommand = new RelayCommand(ExcuteClearByteCount, null);
        }
        #endregion
    }
}
