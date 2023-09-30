using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class MouseEvent
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 上一次的X坐标
        /// </summary>
        public int PreviousX { get; set; }
        /// <summary>
        /// 上一次的Y坐标
        /// </summary>
        public int PreviousY { get; set; }
        /// <summary>
        /// 是否向上移动
        /// </summary>
        public bool IsMoveUp => PreviousY > Y;
        /// <summary>
        /// 是否向下移动
        /// </summary>
        public bool IsMoveDown => PreviousY < Y;
        /// <summary>
        /// 是否向右移动
        /// </summary>
        public bool IsMoveLeft => PreviousX < X;
        /// <summary>
        /// 是否向左移动
        /// </summary>
        public bool IsMoveRight => PreviousX > X;
        /// <summary>
        /// X轴方向移动的距离
        /// </summary>
        public int DistanceX => Math.Abs(X - PreviousX);
        /// <summary>
        /// Y轴方向移动的距离
        /// </summary>
        public int DistanceY => Math.Abs(Y - PreviousY);

        public void SetPrevious()
        {
            PreviousX = X;
            PreviousY = Y;
        }
    }
}
