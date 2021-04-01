namespace IceCoffee.Wpf.MvvmFrame.Messaging
{
    public class GenericMessage<T> : MessageBase
    {
        /// <summary>
        /// 初始化GenericMessage类的新实例。
        /// </summary>
        /// <param name="content">消息的内容</param>
        public GenericMessage(T content)
        {
            Content = content;
        }

        /// <summary>
        /// 初始化GenericMessage类的新实例。
        /// </summary>
        /// <param name="sender">消息的发送者</param>
        /// <param name="content">消息的内容</param>
        public GenericMessage(object sender, T content)
            : base(sender)
        {
            Content = content;
        }

        /// <summary>
        /// 初始化GenericMessage类的新实例。
        /// </summary>
        /// <param name="sender">消息的发送者</param>
        /// <param name="target">消息的目标。此参数可用于指示消息的目的。当然这只是一个指示，amd可能是空的。</param>
        /// <param name="content">消息的内容</param>
        public GenericMessage(object sender, object target, T content)
            : base(sender, target)
        {
            Content = content;
        }

        /// <summary>
        /// 得到或设置消息的内容
        /// </summary>
        public T Content
        {
            get;
            protected set;
        }
    }
}