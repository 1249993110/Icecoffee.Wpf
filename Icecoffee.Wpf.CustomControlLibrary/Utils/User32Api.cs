using System;
using System.Runtime.InteropServices;

namespace IceCoffee.Wpf.CustomControlLibrary.Utils
{
    public static class User32Api
    {
        /// <summary>
        /// 当用户从窗口菜单(前身叫做系统菜单或控制菜单)选择一个命令或者当用户选择最大化按钮，
        /// 最小化按钮，恢复按钮或关闭按钮时，消息接收窗口便会收到这种消息。
        /// </summary>
        public const uint WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// 左键弹起消息
        /// </summary>
        public const uint WM_LBUTTONUP = 0x202;

        /// <summary>
        /// 移动窗口
        /// </summary>
        public const uint SC_MOVE = 0xf012;

        /// <summary>
        /// 调整窗口大小
        /// </summary>
        public const uint SC_SIZE = 0xF000;

        /// <summary>
        /// 窗体样式
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// 窗体的扩展样式
        /// </summary>
        public const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateWindowEx(
            int dwExStyle,                                //窗口的扩展风格
            string lpszClassName,                         //指向注册类名的指针
            string lpszWindowName,                        //指向窗口名称的指针
            int style,                                    //窗口风格
            int x,                                        //窗口的水平位置
            int y,                                        //窗口的垂直位置
            int width,                                    //窗口的宽度
            int height,                                   //窗口的高度
            IntPtr hWndParent,                            //父窗口的句柄
            IntPtr hMenu,                                 //菜单的句柄或是子窗口的标识符
            IntPtr hInst,                                 //应用程序实例的句柄
            [MarshalAs(UnmanagedType.AsAny)] object pvParam//指向窗口的创建数据
        );

        /// <summary>
        /// 设置鼠标的坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public extern static void SetCursorPos(int x, int y);
    }
}