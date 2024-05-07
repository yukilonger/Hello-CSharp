using System;
namespace DesignPattern
{
	public class Singleton
	{
		private static volatile Singleton instance = null;

        private Singleton()
		{
		}

		public static Singleton Instance()
		{
			if(instance == null)
			{
				instance = new Singleton();
			}
			return instance;
		}

        public override string ToString()
        {
            return "单例模式";
        }
    }

    /// <summary>
    /// 单例模式的实现
    /// </summary>
    public class Singleton2
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton2 uniqueInstance = null;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton2()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton2 GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton2();
                    }
                }
            }
            return uniqueInstance;
        }
    }
}

