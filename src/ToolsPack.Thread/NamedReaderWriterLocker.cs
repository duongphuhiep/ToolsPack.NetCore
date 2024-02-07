using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ToolsPack.Thread
{
    /// <summary>
    ///     http://johnculviner.com/achieving-named-lock-locker-functionality-in-c-4-0/
    /// </summary>
    public class NamedReaderWriterLocker<T>
    {
        private readonly ConcurrentDictionary<T, ReaderWriterLockSlim> _lockDict =
            new ConcurrentDictionary<T, ReaderWriterLockSlim>();

        /// <summary>
        /// Get the locker
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReaderWriterLockSlim GetLock(T name)
        {
            return _lockDict.GetOrAdd(name, s => new ReaderWriterLockSlim());
        }

        /// <summary>
        /// shortcut to use the locker to run a lambda function
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public TResult RunWithReadLock<TResult>(T name, Func<TResult> body)
        {
            ReaderWriterLockSlim rwLock = GetLock(name);
            try
            {
                rwLock.EnterReadLock();
                return body();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// shortcut to use the locker to run a lambda function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="body"></param>
        public void RunWithReadLock(T name, Action body)
        {
            ReaderWriterLockSlim rwLock = GetLock(name);
            try
            {
                rwLock.EnterReadLock();
                body();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// shortcut to use the locker to run a lambda function
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public TResult RunWithWriteLock<TResult>(T name, Func<TResult> body)
        {
            ReaderWriterLockSlim rwLock = GetLock(name);
            try
            {
                rwLock.EnterWriteLock();
                return body();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// shortcut to use the locker to run a lambda function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="body"></param>
        public void RunWithWriteLock(T name, Action body)
        {
            ReaderWriterLockSlim rwLock = GetLock(name);
            try
            {
                rwLock.EnterWriteLock();
                body();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}