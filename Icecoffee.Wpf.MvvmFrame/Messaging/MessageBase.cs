namespace IceCoffee.Wpf.MvvmFrame.Messaging
{
    public class MessageBase
    {
        /// <summary>
        /// 初始化MessageBase类的新实例。
        /// </summary>
        public MessageBase()
        {
        }

        /// <summary>
        /// 初始化MessageBase类的新实例。
        /// </summary>
        /// <param name="sender">消息的原始发件人。</param>
        public MessageBase(object sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// 初始化MessageBase类的新实例。
        /// </summary>
        /// <param name="sender">消息的原始发件人。</param>
        /// <param name="target">消息的目标。此参数可用于指示消息的目的。当然这只是一个指示，amd可能是空的。</param>
        public MessageBase(object sender, object target)
            : this(sender)
        {
            Target = target;
        }

        /// <summary>
        /// 得到或设置消息的内容
        /// </summary>
        public object Sender
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获取或设置消息的预期目标。此属性可用于指示消息的目标对象。当然这只是一个指示，amd可能是空的。
        /// </summary>
        public object Target
        {
            get;
            protected set;
        }
    }
}