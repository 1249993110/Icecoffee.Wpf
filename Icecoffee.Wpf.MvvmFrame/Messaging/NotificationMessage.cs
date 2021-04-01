namespace IceCoffee.Wpf.MvvmFrame.Messaging
{
    /// <summary>
    /// 将字符串消息（通知）传递给收件人。
    /// <para>通常，通知被定义为静态类中的唯一字符串。
    /// 若要定义唯一字符串，可以使用Guid.NewGuid（）.ToString（）或任何其他唯一标识符。</para>
    /// </summary>
    public class NotificationMessage : MessageBase
    {
        /// <summary>
        /// 初始化NotificationMessage类的新实例。
        /// </summary>
        /// <param name="notification">包含要传递给收件人的任意消息的字符串</param>
        public NotificationMessage(string notification)
        {
            Notification = notification;
        }

        /// <summary>
        /// 初始化NotificationMessage类的新实例。
        /// </summary>
        /// <param name="sender">消息的发送者。</param>
        /// <param name="notification">包含要传递给收件人的任意消息的字符串</param>
        public NotificationMessage(object sender, string notification)
            : base(sender)
        {
            Notification = notification;
        }

        /// <summary>
        /// 初始化NotificationMessage类的新实例。
        /// </summary>
        /// <param name="sender">消息的发送者。</param>
        /// <param name="target">消息的目标。此参数可用于指示消息的目的。当然这只是一个指示，amd可能是空的。</param>
        /// <param name="notification">包含要传递给收件人的任意消息的字符串</param>
        public NotificationMessage(object sender, object target, string notification)
            : base(sender, target)
        {
            Notification = notification;
        }

        /// <summary>
        /// 获取包含要传递给收件人的任意消息的字符串。
        /// </summary>
        public string Notification
        {
            get;
            private set;
        }
    }
}