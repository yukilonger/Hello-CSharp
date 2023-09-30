using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Utility
{
    public class RegistryHelper
    {
        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetRegistryData(RegistryKey root, string subkey, string name)
        {
            string registData = "";
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            if (myKey != null)
            {
                registData = myKey.GetValue(name).ToString();
            }

            return registData;
        }

        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="valueKind"></param>
        public void SetRegistryData(RegistryKey root, string subkey, string name, string value, RegistryValueKind valueKind)
        {
            RegistryKey regKey = root.CreateSubKey(subkey);
            regKey.SetValue(name, value, valueKind);
            regKey.Close();
        }
        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        public void SetRegistryData(RegistryKey root, string subkey)
        {
            RegistryKey regKey = root.CreateSubKey(subkey);
            regKey.Close();
        }
        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetRegistryData(RegistryKey root, string subkey, string name, string value)
        {
            RegistryKey regKey = root.CreateSubKey(subkey);
            regKey.SetValue(name, value);
            regKey.Close();
        }

        /// <summary>
        /// 删除注册表中指定的注册表项
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        public void DeleteRegist(RegistryKey root, string subkey, string name)
        {
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            subkeyNames = myKey.GetSubKeyNames();
            foreach (string aimKey in subkeyNames)
            {
                if (aimKey == name)
                    myKey.DeleteSubKeyTree(name);
            }
        }

        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsRegistryExist(RegistryKey root, string subkey, string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            subkeyNames = myKey.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }

            return _exit;
        }
    }
}
