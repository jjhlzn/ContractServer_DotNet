using System;
using System.Security.Cryptography;
using System.Text;


    /// <summary>
    /// MD5Util 的摘要说明。
    /// </summary>
    public class MD5Util
    {
        public MD5Util()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(encypStr);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            var result = str.ToLower();
            return result;
        }

        //string retStr;
        //MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

        ////创建md5对象
        //byte[] inputBye;
        //byte[] outputBye;

        ////使用GB2312编码方式把字符串转化为字节数组．
        //try
        //{
        //    inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
        //}
        //catch (Exception ex)
        //{
        //    inputBye = Encoding.GetEncoding("UTF-8").GetBytes(encypStr);
        //}
        //outputBye = m5.ComputeHash(inputBye);

        //retStr = System.BitConverter.ToString(outputBye);
        //retStr = retStr.Replace("-", "").ToUpper();
        //return retStr;
        //}
    }

