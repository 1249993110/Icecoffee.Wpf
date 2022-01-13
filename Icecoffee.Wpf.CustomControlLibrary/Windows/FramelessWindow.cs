using IceCoffee.Common.Structures;
using IceCoffee.Common.Wpf;
using IceCoffee.Wpf.CustomControlLibrary.Primitives;
using IceCoffee.Wpf.CustomControlLibrary.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace IceCoffee.Wpf.CustomControlLibrary.Windows
{
    public class FramelessWindow : WindowBase
    {
        private enum Direction : uint
        {
            NONE = 0, LEFT = 1, RIGHT = 2, TOP = 3, LEFTTOP = 4, RIGHTTOP = 5, DOWN = 6, LEFTBOTTOM = 7, RIGHTBOTTOM = 8
        };

        #region 字段

        private const short PADDING = 5;                //鼠标区域判断的内边距
        private short _headHeight = 0;                  //双击最大化/向下还原点击域-高度
        private Direction _direction = Direction.NONE;  //窗口大小改变时，记录改变方向
        private IntPtr _handle;                         //窗口句柄

        #endregion 字段

        #region 属性

        /// <summary>
        /// 头高，双击最大化/还原
        /// </summary>
        public short HeadHeight
        {
            get { return _headHeight; }
            set { _headHeight = value; }
        }

        #endregion 属性

        #region 方法

        #region 构造方法

        public FramelessWindow()
        {
            this.MaxWidth = SystemParameters.WorkArea.Width + 20;
            this.MaxHeight = SystemParameters.WorkArea.Height + 8;
        }

        #endregion 构造方法

        #region 私有方法

        /// <summary>
        /// 判断鼠标区域
        /// </summary>
        private void Region()
        {
            //获取窗体右下角的相对坐标，即窗体的长宽，rb为rightbottom点
            IntPoint rb;
            rb.X = (int)this.Width;
            rb.Y = (int)this.Height;

            //获取光标的相对坐标，cp为cursorPoint
            Point position = Mouse.GetPosition(this);
            IntPoint cp;
            cp.X = (int)position.X;
            cp.Y = (int)position.Y;

            if (cp.X <= PADDING + 2 && cp.X >= 0 && cp.Y <= PADDING + 2 && cp.Y >= 0)
            {
                // 左上角
                _direction = Direction.LEFTTOP;
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (cp.X >= rb.X - (PADDING + 2) && cp.X <= rb.X && cp.Y >= rb.Y - (PADDING + 2) && cp.Y <= rb.Y)
            {
                // 右下角
                _direction = Direction.RIGHTBOTTOM;
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (cp.X <= 0 + PADDING + 2 && cp.X >= 0 && cp.Y >= rb.Y - (PADDING + 2) && cp.Y <= rb.Y)
            {
                // 左下角
                _direction = Direction.LEFTBOTTOM;
                this.Cursor = Cursors.SizeNESW;
            }
            else if (cp.X <= rb.X && cp.X >= rb.X - (PADDING + 2) && cp.Y >= 0 && cp.Y <= PADDING + 2)
            {
                // 右上角
                _direction = Direction.RIGHTTOP;
                this.Cursor = Cursors.SizeNESW;
            }
            else if (cp.X <= 0 + PADDING && cp.X >= 0)
            {
                // 左边
                _direction = Direction.LEFT;
                this.Cursor = Cursors.SizeWE;
            }
            else if (cp.X <= rb.X && cp.X >= rb.X - PADDING)
            {
                // 右边
                _direction = Direction.RIGHT;
                this.Cursor = Cursors.SizeWE;
            }
            else if (cp.Y >= 0 && cp.Y <= 0 + PADDING)
            {
                // 上边
                _direction = Direction.TOP;
                this.Cursor = Cursors.SizeNS;
            }
            else if (cp.Y <= rb.Y && cp.Y >= rb.Y - PADDING)
            {
                // 下边
                _direction = Direction.DOWN;
                this.Cursor = Cursors.SizeNS;
            }
            else if (_direction != Direction.NONE)
            {
                // 默认
                _direction = Direction.NONE;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void DragMoveWindow()
        {
            User32Api.SendMessage(_handle, User32Api.WM_SYSCOMMAND, (IntPtr)User32Api.SC_MOVE, IntPtr.Zero);
        }

        private void ResizeWindow(Direction direction)
        {
            User32Api.SendMessage(_handle, User32Api.WM_SYSCOMMAND, (IntPtr)(User32Api.SC_SIZE + direction), IntPtr.Zero);
        }

        #endregion 私有方法

        #region 保护方法

        protected override void OnSourceInitialized(EventArgs e)
        {
            _handle = new WindowInteropHelper(this).Handle;

            HwndSource mainWindowSrc = HwndSource.FromHwnd(_handle);
            mainWindowSrc.CompositionTarget.BackgroundColor = System.Windows.Media.Colors.Transparent;

            base.OnSourceInitialized(e);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            //向消息循环队列投递待处理任务
            this.Dispatcher.InvokeAsync(() =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(100);

                    DwmApi.LoadSystemWindowShadowEffect(_handle);
                });
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_direction == Direction.NONE)  //拖拽窗口
                {
                    if (this.WindowState == WindowState.Normal || Mouse.GetPosition(this).Y < this._headHeight)
                    {
                        this.DragMoveWindow(); //this.DragMove(); //光标会有闪烁
                    }
                }
                else //改变窗口大小
                {
                    this.ResizeWindow(_direction);
                }
            }
            else if (this.WindowState == WindowState.Normal)
            {
                Region();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) //使用冒泡事件
        {
            if (_direction != Direction.NONE)
            {
                var pos = this.PointToScreen(Mouse.GetPosition(this)); //鼠标屏幕坐标
                var leftTop = this.PointToScreen(new Point(0, 0)); //左上
                var right = this.PointToScreen(new Point(this.Width, 0)); //右
                var bottom = this.PointToScreen(new Point(0, this.Height)); //下

                if (pos.X - leftTop.X <= 2) //左
                    User32Api.SetCursorPos((int)pos.X + 1, (int)pos.Y);
                else if (pos.Y - leftTop.Y <= 2) //上
                    User32Api.SetCursorPos((int)pos.X, (int)pos.Y + 1);
                else if (right.X - pos.X <= 2) //右
                    User32Api.SetCursorPos((int)pos.X - 1, (int)pos.Y);
                else if (bottom.Y - pos.Y <= 2) //下
                    User32Api.SetCursorPos((int)pos.X, (int)pos.Y - 1);
            }
            else if (e.ClickCount == 2 && Mouse.GetPosition(this).Y <= this._headHeight) //双击最大化
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                e.Handled = true;
            }
            base.OnMouseLeftButtonDown(e);
        }

        #endregion 保护方法

        #endregion 方法
    }
}