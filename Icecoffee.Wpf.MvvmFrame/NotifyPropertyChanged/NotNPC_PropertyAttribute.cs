﻿using System;

namespace IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged
{
    /// <summary>
    /// 指定不要切入此特性标记的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class NotNPC_PropertyAttribute : Attribute
    {
    }
}