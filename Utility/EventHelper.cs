using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class EventHelper
    {
        private static BindingFlags bindingFlagsTemp = BindingFlags.Instance | BindingFlags.NonPublic;
        private static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase;

        /// <summary>
        /// 清空控件所有事件的处理方法.
        /// 默认过滤掉 "Disposed"
        /// </summary>
        /// <param name="control"></param>
        /// <param name="fifterEvents">需要跳过的事件,事件名包含即可,不区分大小写</param>
        public static void ClearEvent(this object control, List<string> fifterEvents)
        {
            var type = control.GetType();
            var events = type.GetEvents();
            foreach (var evt in events)
            {
                if (fifterEvents != null && fifterEvents.Any(f => evt.Name.IndexOf(f, StringComparison.OrdinalIgnoreCase) > -1))
                    continue;
                var InvocationList = control.GetEventInvocationList(evt.Name);
                if (InvocationList == null || InvocationList.Length == 0)
                    continue;
                foreach (var deleg in InvocationList)
                {
                    evt.RemoveEventHandler(control, deleg);
                }
            }
        }
        /// <summary>
        /// 清空控件指定事件下的事件处理方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventname"></param>
        public static string ClearEvent(this object control, string eventname)
        {
            var invocationList = control.GetEventInvocationList(eventname);
            if (invocationList == null)
                return "";
            Type controlType = control.GetType();
            EventInfo eventInfo = controlType.GetEvent(eventname);
            foreach (Delegate dx in invocationList)
                eventInfo.RemoveEventHandler(control, dx);
            return invocationList[0].Method.Name;
        }

        /// <summary>
        /// 手动触发控件的指定事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventname"></param>
        /// <param name="e">事件参数,一般第一个参数sender默认为控件,所以就省略了,只需要传第二个参数</param>
        public static void TriggerEvent(this object control, string eventname, object e)
        {
            var InvocationList = control.GetEventInvocationList(eventname);
            if (InvocationList == null)
                return;
            foreach (Delegate dx in InvocationList)
                dx.DynamicInvoke(control, e);
        }

        /// <summary>
        /// 获取指定事件的委托
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventname"></param>
        /// <returns></returns>
        public static Delegate[] GetEventInvocationList(this object control, string eventname)
        {
            if (control == null) return null;
            if (string.IsNullOrEmpty(eventname)) return null;

            Type controlType = control.GetType();
            return GetEventInvocationList(control, controlType, eventname);
        }

        public static Delegate[] GetEventInvocationList(this object control, Type controlType, string eventname)
        {
            if (string.IsNullOrEmpty(eventname)) return null;
            PropertyInfo propertyInfo = controlType.GetProperty("Events", bindingFlags);
            if (propertyInfo == null)
            {
                if (controlType.BaseType.FullName == "System.Object")
                    return null;
                return GetEventInvocationList(control, controlType.BaseType, eventname);
            }
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);

            FieldInfo fieldInfo = controlType.GetField("Event" + eventname, bindingFlags);
            if (fieldInfo == null) fieldInfo = controlType.GetField("Event_" + controlType.Name + eventname, bindingFlags);//有的控件时需要加上控件类型名作为事件名,
            if (fieldInfo == null) fieldInfo = controlType.GetField(eventname, bindingFlags);//也许有的不需要加前缀
            if (fieldInfo == null)
            {
                //有的必须递归从 BaseType 才能取到事件字段
                if (controlType.BaseType.FullName == "System.Object")
                    return null;
                return GetEventInvocationList(control, controlType.BaseType, eventname);
            }
            var fieldInfoValue = fieldInfo.GetValue(control);
            Delegate d = eventHandlerList[fieldInfoValue];
            if (d == null) return null;

            return d.GetInvocationList();
        }

        public static void AddEventHandler(this object control, object source, string functionName)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                return;
            MethodInfo methodInfo = source.GetType().GetMethod(functionName, bindingFlagsTemp);
            if (methodInfo == null) 
                return;
            Delegate @delegate = Delegate.CreateDelegate(typeof(EventHandler), source, methodInfo);
            control.GetType().GetEvent("Click").RemoveEventHandler(control, @delegate);
            control.GetType().GetEvent("Click").AddEventHandler(control, @delegate);
        }
    }
}
