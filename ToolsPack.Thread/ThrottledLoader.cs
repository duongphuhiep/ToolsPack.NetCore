using System;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace ToolsPack.Thread
{
    /// <summary>
    /// A generic loader with zero argument
    /// </summary>
    public delegate T Loader0<T>();

    /// <summary>
    /// Decorator the Loader0 with throttling.
    /// 
    /// Problem it try to solve
    /// 
    /// Loader0<int> lo = () => { select_from_database; return i; }
    /// var result = lo(); // select_from_database every call
    /// for (1..10000) { result = lo(); }  //BAD: select_from_database 10000 times
    /// 
    /// How it solve
    /// 
    /// ThrottledLoader0<int> tlo = new ThrottledLoader0<int>(lo, 500); //0.5s
    /// var result = tlo.GetValue(); // select_from_database at most once every 0.5s
    /// for (1..10000) { result = tlo.GetValue(); }  //GOOD: select_from_database only once every 0.5s
    /// 
    /// if you still can capture the idea => Google "Throttling and Debouncing"
    /// </summary>
    public class ThrottledLoader0<T>
    {
        private readonly Loader0<T> core;

        private T lastValue;
        private long lastUpdateTicks;
        private readonly long expiryInTick;
        object locker = new object();

        private bool expired => lastValue == null || (DateTime.Now.Ticks - lastUpdateTicks) > expiryInTick;

        /// <summary>
        /// Get value in the memory cache if not expired.
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {
            if (expired)
            {
                lock (locker) 
                {
                    if (expired) 
                    {
                        lastValue = core();
                        lastUpdateTicks = DateTime.Now.Ticks;
                    }
                }
            }
            return lastValue;
        }

        /// <summary>
        /// Note: expiryInSecond = expiryInTick / 10000000 (ten milion)
        /// </summary>
        /// <param name="core"></param>
        /// <param name="expiryInMilisecond"></param>
        public ThrottledLoader0(Loader0<T> core, long expiryInMilisecond)
        {
            this.core = core ?? throw new ArgumentNullException(nameof(core));
            this.expiryInTick = expiryInMilisecond * 10000; //expiryInSecond * 10000000;
        }
    }


    /// <summary>
    /// A generic loader with zero argument
    /// </summary>
    public delegate Task<T> LoaderAsync0<T>();

    /// <summary>
    /// Decorator the LoaderAsync0 with throttling.
    /// 
    /// Problem it try to solve
    /// 
    /// LoaderAsync0<int> lo = async () => { await select_from_database; return i; }
    /// var result = await lo(); // select_from_database every call
    /// for (1..10000) { result = await lo(); }  //BAD: select_from_database 10000 times
    /// 
    /// How it solve
    /// 
    /// ThrottledLoaderAsync0<int> tlo = new ThrottledLoaderAsync0<int>(lo, 500); //0.5s
    /// var result = await tlo.GetValue(); // select_from_database at most once every 0.5s
    /// for (1..10000) { result = await tlo.GetValueAsync(); }  //GOOD: select_from_database only once every 0.5s
    /// 
    /// if you still can capture the idea => Google "Throttling and Debouncing"
    /// </summary>
    public class ThrottledLoaderAsync0<T>
    {
        private readonly LoaderAsync0<T> core;

        private T lastValue;
        private long lastUpdateTicks;
        private readonly long expiryInTick;
        private readonly Channel<bool> locker = Channel.CreateBounded<bool>(1);

        private bool expired => lastValue == null || (DateTime.Now.Ticks - lastUpdateTicks) > expiryInTick;

        public async Task<T> GetValueAsync()
        {
            if (expired)
            {
                try  
                {
                    await locker.Writer.WriteAsync(true).ConfigureAwait(false); //acquire the log
                    if (expired) 
                    {
                        lastValue = await core().ConfigureAwait(false); 
                        lastUpdateTicks = DateTime.Now.Ticks;
                    }
                }
                finally 
                {
                    await locker.Reader.ReadAsync().ConfigureAwait(false); //unlock
                }
            }
            return lastValue;
        }

        /// <summary>
        /// Note: expiryInSecond = expiryInTick / 10000000 (ten milion)
        /// </summary>
        /// <param name="core"></param>
        /// <param name="expiryInMilisecond"></param>
        public ThrottledLoaderAsync0(LoaderAsync0<T> core, long expiryInMilisecond)
        {
            this.core = core ?? throw new ArgumentNullException(nameof(core));
            this.expiryInTick = expiryInMilisecond * 10000; //expiryInSecond * 10000000;
        }
    }
}
