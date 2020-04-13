using System;
using System.Collections.Concurrent;

namespace ToolsPack.Thread
{
    /// <summary>
    ///     http://johnculviner.com/achieving-named-lock-locker-functionality-in-c-4-0/
    /// </summary>
    public class NamedLocker<T>
    {
        private readonly ConcurrentDictionary<T, object> _lockDict = new ConcurrentDictionary<T, object>();

        /// <summary>
        /// get a lock for use with a lock(){} block
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetLock(T name)
        {
            return _lockDict.GetOrAdd(name, s => new object());
        }

        /// <summary>
        /// run a short lock inline using a lambda
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public TResult RunWithLock<TResult>(T name, Func<TResult> body)
        {
            lock (_lockDict.GetOrAdd(name, s => new object()))
                return body();
        }

        /// <summary>
        /// run a short lock inline using a lambda
        /// </summary>
        /// <param name="name"></param>
        /// <param name="body"></param>
        public void RunWithLock(T name, Action body)
        {
            lock (_lockDict.GetOrAdd(name, s => new object()))
                body();
        }

        /// <summary>
        /// remove an old lock object that is no longer needed
        /// </summary>
        /// <param name="name"></param>
        public void RemoveLock(T name)
        {
            object o;
            _lockDict.TryRemove(name, out o);
        }
    }
}