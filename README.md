
# Explanation
|名词|解释|
|-|-|
|vm|ViewModel的缩写|
|Prop 弹出|


# Instructions

#### MVVM说明
1. MVVM是Model-View-ViewModel的简写。MVVM 就是将其中的View 的状态和行为抽象化，让我们将视图 UI 和业务逻辑分开。
2. MVVM模式充分利用了WPF的数据绑定机制，最大限度地降低了Xmal文件和CS文件的耦合度，也就是UI显示和逻辑代码的耦合度，如需要更换界面时，逻辑代码修改很少，甚至不用修改。
3. 与WinForm开发相比，我们一般在后置代码中会使用控件的名字来操作控件的属性来更新UI，而在WPF中通常是通过数据绑定来更新UI。
4. 在响应用户操作上，WinForm是通过控件的事件来处理，而WPF可以使用命令绑定的方式来处理，耦合度将降低。

### spt功能说明
1. 能够打开/关闭串口
2. 把能够使用的串口全部加入串口列表
3. 能够使用串口发送数据(HEX/Text)，并能够接口来自串口的数据
4. 能够统计并显示发送/接收的数据字节数

#### spt框架说明
1. spt.tool是工具类，不依赖任何项目。
2. spt.mvvm是mvvm模式的基类，不依赖任何项目。后续的ViewModel需要继承或使用内部的类
3. spt.model是spt里面的数据模型，不依赖任何项目。
4. spt.ui.share是通用的ui样式。例如：弹窗(提示式和警告式)。它依赖spt.tool和spt.mvvm
5. spt.vm是项目的ViewModel，一个界面对应一个ViewModel。它依赖spt.tool、spt.mvvm、spt.model、spt.ui.share
6. spt.app是项目主程序UI，其中的主窗体MainWindow绑定的ViewModel为spt.vm里面的MainWindowVM。

# Build & Package
1. 配置。mvvm模式中使用了"System.Windows.Interactivity.dll"配置文件，可以从"Blend for Visual studio"软件中获取；
2. 升级版本。在spt.app的Properties配置文件中，打开程序集信息进行版本调整；
4. 编译。在**Release**下编译**ssm.app**，即可完成安装包的打包。


# Solution Structure

|**Folder**|**Instructions**|
|-|-|
|\lib|动态库，一般是可能用到的文件（暂未使用）|
|\spt.app\bin| 程序编译目录位置


# Commit Notice
1. 版本号格式为：**x.x.x.x**，测试版本后带上 **.alpha**；第一位为主版本号，第二位为次版本号，第三位为变更版本号，第四位为修订版本号；
2. 重要版本需要打上tag，测试修订版本不要打tag；
3. 安装包命名：**spt_x.x.x.x.exe**；


#### 版本说明：
1. *Base*：此版本表示该软件仅仅是一个假页面链接，通常包括所有的功能和页面布局，但是页面中的功能都没有做完整的实现，只是做为整体网站的一个基础架构。
2. *Alpha*：软件的初级版本，表示该软件在此阶段以实现软件功能为主，通常只在软件开发者内部交流，一般而言，该版本软件的Bug较多，需要继续修改，是测试版本。测试人员提交Bug经开发人员修改确认之后，发布到测试网址让测试人员测试，此时可将软件版本标注为alpha版。
3. *Beta*：该版本相对于Alpha版已经有了很大的进步，消除了严重错误，但还需要经过多次测试来进一步消除，此版本主要的修改对象是软件的UI。修改的的Bug 经测试人员测试确认后可发布到外网上，此时可将软件版本标注为 beta版。
4. *RC*：该版本已经相当成熟了，基本上不存在导致错误的Bug，与即将发行的正式版本相差无几。
5. *Release*：该版本意味“最终版本”，在前面版本的一系列测试版之后，终归会有一个正式的版本，是最终交付用户使用的一个版本。该版本有时也称标准版。