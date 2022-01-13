using IceCoffee.Wpf.MvvmFrame.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace IceCoffee.Wpf.MvvmFrame.Messaging
{
    /// <summary>
    /// 在不同的同事对象之间提供松散耦合的消息传递。所有对对象的引用都被弱存储，以防止内存泄漏。
    /// </summary>
    public class Messenger : IMessenger
    {
        private static readonly object _creationLock = new object();

        private static IMessenger _defaultInstance;

        private readonly object _registerLock = new object();

        private Dictionary<Type, List<IWeakAction>> _recipientsStrictAction;

        private bool _isCleanupRegistered;

        /// <summary>
        /// 获取Messenger的默认实例，允许以静态方式注册和发送消息
        /// </summary>
        public static IMessenger Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (_creationLock)
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new Messenger();
                        }
                    }
                }

                return _defaultInstance;
            }
        }

        #region IMessenger Members

        /// <summary>
        /// 为消息类型TMessage注册接收者
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="recipient"></param>
        /// <param name="action"></param>
        /// <param name="keepTargetAlive"></param>
        public virtual void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            lock (_registerLock)
            {
                var messageType = typeof(TMessage);

                if (_recipientsStrictAction == null)
                {
                    _recipientsStrictAction = new Dictionary<Type, List<IWeakAction>>();
                }

                lock (_recipientsStrictAction)
                {
                    List<IWeakAction> list = null;

                    if (_recipientsStrictAction.TryGetValue(messageType, out list) == false)
                    {
                        list = new List<IWeakAction>();
                        _recipientsStrictAction.Add(messageType, list);
                    }

                    WeakAction<TMessage> weakAction = new WeakAction<TMessage>(recipient, action);

                    list.Add(weakAction);
                }
            }

            RequestCleanup();
        }

        /// <summary>
        /// 向已注册的接收者发送消息。此消息将使用其中一个注册方法到达为此消息类型注册的所有接收者。
        /// </summary>
        /// <typeparam name="TMessage">将发送的消息类型。</typeparam>
        /// <param name="message">要发送给已注册接收者的消息。</param>
        public virtual void Send<TMessage>(TMessage message)
        {
            SendToTargetOrType(message, null);
        }

        /// <summary>
        /// 向已注册的接收者发送消息。此消息将只到达使用其中一个Register方法为此消息类型注册的接收者以及targetType的接收者。
        /// </summary>
        /// <typeparam name="TMessage">将发送的消息类型。</typeparam>
        /// <typeparam name="TTarget">将接收消息的接收者类型。消息将不会发送到其他类型的接收者。</typeparam>
        /// <param name="message">要发送给已注册接收者的消息。</param>
        public virtual void Send<TMessage, TTarget>(TMessage message)
        {
            SendToTargetOrType(message, typeof(TTarget));
        }

        /// <summary>
        /// 向已注册的接收者发送消息。此消息将只到达使用其中一个Register方法为此消息类型注册的接收者以及targetType的接收者。
        /// </summary>
        /// <typeparam name="TMessage">将发送的消息类型。</typeparam>
        /// <param name="message">要发送给已注册接收者的消息。</param>
        /// <param name="targetType">将接收消息的接收者类型。消息将不会发送到其他类型的接收者。</param>
        public virtual void Send<TMessage>(TMessage message, Type targetType)
        {
            SendToTargetOrType(message, targetType);
        }

        /// <summary>
        /// 完全注销消息接收者。执行此方法后，接收者将不再接收任何消息。
        /// </summary>
        /// <param name="recipient"></param>
        public virtual void Unregister(object recipient)
        {
            UnregisterFromLists(recipient, _recipientsStrictAction);
        }

        /// <summary>
        /// 仅为给定类型的消息注销消息接收者。
        /// 执行此方法后，接收者将不再接收TMessage类型的消息，但仍将接收其他消息类型（如果它以前为它们注册的话）.
        /// </summary>
        /// <param name="recipient">必须注销的接收者。</param>
        /// <typeparam name="TMessage">接收者要从中注销的消息类型。</typeparam>
        public virtual void Unregister<TMessage>(object recipient)
        {
            Unregister<TMessage>(recipient, null);
        }

        /// <summary>
        /// 为给定类型的消息、给定操作注销消息接收者。其他消息类型仍将发送给接收者（如果它以前为接收者注册过）。
        /// 为消息类型TMessage、给定接收者和其他令牌（如果可用）注册的其他操作也将保持可用。
        /// </summary>
        /// <typeparam name="TMessage">接收者要从中注销的消息类型。</typeparam>
        /// <param name="recipient">必须注销的接收者。</param>
        /// <param name="action">必须为接收者和消息类型TMessage注销的操作。</param>
        public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            UnregisterFromLists(recipient, action, _recipientsStrictAction);
            RequestCleanup();
        }

        /// <summary>
        /// 为给定类型注销所有消息接收者
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        public virtual void UnregisterAllRecipientByType<TMessage>()
        {
            UnregisterAllRecipientByType(typeof(TMessage));
        }

        /// <summary>
        /// 为给定类型注销所有消息接收者
        /// </summary>
        /// <param name="messageType"></param>
        public virtual void UnregisterAllRecipientByType(Type messageType)
        {
            var lists = _recipientsStrictAction;
            if (lists == null
               || lists.Count == 0
               || lists.ContainsKey(messageType) == false)
            {
                return;
            }

            lock (lists)
            {
                foreach (var item in lists[messageType])
                {
                    if (item != null)
                    {
                        item.MarkForDeletion();
                    }
                }
            }
        }

        #endregion IMessenger Members

        /// <summary>
        /// 提供用自定义实例重写Messenger.Default实例的方法，例如用于单元测试。
        /// </summary>
        /// <param name="newMessenger">将用作Messenger.Default的实例。</param>
        public static void OverrideDefault(IMessenger newMessenger)
        {
            _defaultInstance = newMessenger;
        }

        /// <summary>
        /// 将Messenger的默认（静态）实例设置为空。
        /// </summary>
        public static void Reset()
        {
            _defaultInstance = null;
        }

        private static void CleanupList(IDictionary<Type, List<IWeakAction>> lists)
        {
            if (lists == null)
            {
                return;
            }

            lock (lists)
            {
                var listsToRemove = new List<Type>();
                foreach (var list in lists)
                {
                    var recipientsToRemove = list.Value
                        .Where(item => item == null || item.IsAlive == false)
                        .ToList();

                    foreach (var recipient in recipientsToRemove)
                    {
                        list.Value.Remove(recipient);
                    }

                    if (list.Value.Count == 0)
                    {
                        listsToRemove.Add(list.Key);
                    }
                }

                foreach (var key in listsToRemove)
                {
                    lists.Remove(key);
                }
            }
        }

        private static void SendToList<TMessage>(TMessage message,
            IEnumerable<IWeakAction> weakActionsAndTokens,
            Type messageTargetType)
        {
            if (weakActionsAndTokens != null)
            {
                // 克隆以防止用户在“接收消息”方法中注册
                var list = weakActionsAndTokens.ToList();
                var listClone = list.Take(list.Count).ToList();

                foreach (var item in listClone)
                {
                    if (item != null && item.IsAlive)
                    {
                        object target = item.Target;
                        if (target != null)
                        {
                            if (messageTargetType == null
                            || target.GetType() == messageTargetType
                            || messageTargetType.IsAssignableFrom(target.GetType()))
                            {
                                item.ExecuteWithObject(message);
                            }
                        }
                    }                    
                }
            }
        }

        private static void UnregisterFromLists(object recipient, Dictionary<Type, List<IWeakAction>> lists)
        {
            if (recipient == null
                || lists == null
                || lists.Count == 0)
            {
                return;
            }

            lock (lists)
            {
                foreach (var messageType in lists.Keys)
                {
                    foreach (var item in lists[messageType])
                    {
                        if (item != null && recipient == item.Target)
                        {
                            item.MarkForDeletion();
                        }
                    }
                }
            }
        }

        private static void UnregisterFromLists<TMessage>(
            object recipient, Action<TMessage> action,
            Dictionary<Type, List<IWeakAction>> lists)
        {
            Type messageType = typeof(TMessage);

            if (recipient == null
                || lists == null
                || lists.Count == 0
                || lists.ContainsKey(messageType) == false)
            {
                return;
            }

            lock (lists)
            {
                foreach (var item in lists[messageType])
                {
                    WeakAction<TMessage> weakActionCasted = item as WeakAction<TMessage>;

                    if (weakActionCasted != null
                        && recipient == weakActionCasted.Target
                        && (action == null || action.Method.Name == weakActionCasted.MethodName))
                    {
                        item.MarkForDeletion();
                    }
                }
            }
        }

        /// <summary>
        /// 通知Messenger应扫描和清理接收者列表。由于接收者存储为<see cref="WeakReference"/>，
        /// 因此即使Messenger将其保存在列表中，也可以对接收者进行垃圾回收。
        /// 在清理操作期间，将从列表中删除所有“已死亡”接收者。由于此操作可能需要一些时间，因此仅在应用程序空闲时执行。
        /// 因此，Messenger类的用户应该使用<see cref="RequestCleanup"/> 而不是使用<see cref="Cleanup"/>方法强制执行。
        /// </summary>
        public void RequestCleanup()
        {
            if (_isCleanupRegistered == false)
            {
                Action cleanupAction = Cleanup;

                Dispatcher.CurrentDispatcher.BeginInvoke(
                    cleanupAction,
                    DispatcherPriority.ApplicationIdle,
                    null);

                _isCleanupRegistered = true;
            }
        }

        /// <summary>
        /// 扫描接收者列表中的“死亡”实例并将其删除。由于接收者存储为<see cref="WeakReference"/>，
        /// 因此即使Messenger将其保存在列表中，也可以对接收者进行垃圾回收。在清理操作期间，将从列表中删除所有“已死亡”接收者。
        /// 由于此操作可能需要一些时间，因此仅在应用程序空闲时执行。因此，Messenger类的用户应该使用<see cref="RequestCleanup"/>
        /// 而不是使用<see cref="Cleanup"/>方法强制使用。
        /// </summary>
        public void Cleanup()
        {
            CleanupList(_recipientsStrictAction);
            _isCleanupRegistered = false;
        }

        private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType)
        {
            var messageType = typeof(TMessage);

            if (_recipientsStrictAction != null)
            {
                List<IWeakAction> list = null;

                lock (_recipientsStrictAction)
                {
                    if (_recipientsStrictAction.ContainsKey(messageType))
                    {
                        list = _recipientsStrictAction[messageType]
                            .Take(_recipientsStrictAction[messageType].Count())
                            .ToList();
                    }
                }

                if (list != null)
                {
                    SendToList(message, list, messageTargetType);
                }
            }

            RequestCleanup();
        }
    }
}