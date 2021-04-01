using System;

namespace IceCoffee.Wpf.MvvmFrame.Messaging
{
    public interface IMessenger
    {
        /// <summary>
        /// 注册一个接收消息的类型
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="recipient">收件人</param>
        /// <param name="action"></param>
        void Register<TMessage>(object recipient, Action<TMessage> action);

        void Send<TMessage>(TMessage message);

        /// <summary>
        /// 给注册的类型发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="message"></param>
        void Send<TMessage, TTarget>(TMessage message);

        /// <summary>
        /// 向已注册的收件人发送邮件。此邮件将只到达使用其中一个Register方法为此邮件类型注册的收件人以及targetType的收件人。
        /// </summary>
        /// <typeparam name="TMessage">将发送的消息类型。</typeparam>
        /// <param name="message">要发送给已注册收件人的邮件。</param>
        /// <param name="targetType">将接收邮件的收件人类型。邮件将不会发送到其他类型的收件人。</param>
        void Send<TMessage>(TMessage message, Type targetType);

        /// <summary>
        /// 完全注销消息接收者。执行此方法后，接收者将不再接收任何消息。
        /// </summary>
        /// <param name="recipient"></param>
        void Unregister(object recipient);

        /// <summary>
        /// 仅为给定类型的消息注销消息接收者。
        /// 执行此方法后，接收者将不再接收TMessage类型的消息，但仍将接收其他消息类型（如果它以前为它们注册的话）.
        /// </summary>
        /// <param name="recipient">必须注销的接收者。</param>
        /// <typeparam name="TMessage">接收者要从中注销的消息类型。</typeparam>
        void Unregister<TMessage>(object recipient);

        /// <summary>
        /// 为给定类型的消息、给定操作注销消息接收者。其他消息类型仍将发送给接收者（如果它以前为接收者注册过）。
        /// 为消息类型TMessage、给定接收者和其他令牌（如果可用）注册的其他操作也将保持可用。
        /// </summary>
        /// <typeparam name="TMessage">接收者要从中注销的消息类型。</typeparam>
        /// <param name="recipient">必须注销的接收者。</param>
        /// <param name="action">必须为接收者和消息类型TMessage注销的操作。</param>
        void Unregister<TMessage>(object recipient, Action<TMessage> action);

        /// <summary>
        /// 为给定类型注销所有消息接收者
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        void UnregisterAllRecipientByType<TMessage>();

        /// <summary>
        /// 为给定类型注销所有消息接收者
        /// </summary>
        /// <param name="messageType"></param>
        void UnregisterAllRecipientByType(Type messageType);
    }
}