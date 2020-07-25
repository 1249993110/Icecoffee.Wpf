using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.Wpf.CustomControlLibrary.Utils
{
    /// <summary>
    /// Microsoft桌面窗口管理器DWM 的公用界面，用作Aero效果
    /// </summary>
    public static class DwmApi
    {
        [StructLayout(LayoutKind.Sequential)]//默认先后顺序排列
        public struct MARGINS
        {
            public int left;
            public int top;
            public int right;
            public int buttom;
        }
        public enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }
        public enum DWMNCRENDERINGPOLICY : uint
        {
            DWMNCRP_USEWINDOWSTYLE,
            DWMNCRP_DISABLED,
            DWMNCRP_ENABLED,
            DWMNCRP_LAST
        }
        /// <summary>
        /// 检测Aero是否为打开
        /// </summary>
        /// <param name="enabledptr"></param>
        [DllImport("Dwmapi.dll")]
        public static extern void DwmIsCompositionEnabled(ref int enabledptr);

        /// <summary>
        /// 启用非客户区域渲染；窗口样式将被忽略。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="attr"></param>
        /// <param name="attrValue"></param>
        /// <param name="attrSize"></param>
        /// <returns></returns>
        [DllImport("Dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

        /// <summary>
        /// 创建“玻璃板”效果；将窗口框架扩展到工作区。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        [DllImport("Dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margin);

        /// <summary>
        /// 加载系统窗口阴影效果
        /// </summary>
        public static void LoadSystemWindowShadowEffect(IntPtr handle)
        {
            try
            {
                int flag = 0;
                DwmApi.DwmIsCompositionEnabled(ref flag);
                if (flag > 0)
                {
                    int ncrp = (int)DwmApi.DWMNCRENDERINGPOLICY.DWMNCRP_ENABLED;
                    DwmApi.DwmSetWindowAttribute(handle, DwmApi.DWMWINDOWATTRIBUTE.NCRenderingPolicy, ref ncrp, sizeof(int));
                    DwmApi.MARGINS mar = new DwmApi.MARGINS() { left = -1, top = -1, right = -1, buttom = -1 };
                    DwmApi.DwmExtendFrameIntoClientArea(handle, ref mar);
                }
            }
            catch
            {
            }
        }
    }
}
