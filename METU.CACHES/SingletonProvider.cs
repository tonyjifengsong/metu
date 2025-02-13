﻿namespace METU.CACHES
{

    public class SingletonProvider<T> where T : class, new()
    {
        private SingletonProvider()
        {
        }

        private static T _instance;
        // 用于lock块的对象
        private static readonly object _synclock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synclock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();

                        }
                    }
                }
                return _instance;
            }
            set { _instance = value; }
        }
    }
}
