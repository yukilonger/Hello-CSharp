using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_
{
    /// <summary>
    /// 任务控制器
    /// </summary>
    public class TaskControl
    {
        public string Name { get; set; }

        /// <summary>
        /// 暂停
        /// </summary>
        public ManualResetEvent ResetEvent { get; set; }

        /// <summary>
        /// 暂停否
        /// </summary>
        public bool IsEventReset { get; set; }

        /// <summary>
        /// 重置
        /// </summary>
        public bool IsReset { get; set; }

        /// <summary>
        /// 取消
        /// </summary>
        public CancellationTokenSource CancelSource { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelDateTime { get; set; }

        /// <summary>
        /// 任务时长，单位：秒
        /// </summary>
        public int Duration
        {
            get
            {
                if (CancelDateTime == null) return 0;
                return CancelDateTime.Value.Subtract(StartDateTime).Seconds;
            }
        }

        /// <summary>
        /// 暂停时间
        /// </summary>
        public DateTime? EventResetDateTime { get; set; }

        /// <summary>
        /// 任务控制器
        /// </summary>
        /// <param name="initCancelSource">Initialize CancellationTokenSource</param>
        public TaskControl(bool initCancelSource = false)
        {
            ResetEvent = new ManualResetEvent(true);
            IsReset = false;

            if(initCancelSource)
            {
                CancelSource = new CancellationTokenSource();
            }
        }

        public void Wait()
        {
            ResetEvent.WaitOne();
        }

        public void Pause()
        {
            ResetEvent.Reset();
            IsEventReset = true;
        }

        public void Restart()
        {
            ResetEvent.Set();
            IsEventReset = false;
        }

        public void Cancel()
        {
            try
            {
                CancelSource?.Cancel();
                ResetEvent.Set();
            }
            catch { }
        }

        ~TaskControl()
        {
            try
            {
                CancelSource?.Dispose();
            }
            catch { }
        }
    }
}
