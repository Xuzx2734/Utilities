using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
   public static class ConvertExtend {

        /// <summary>
        ///  To Md5    转化为md5
        ///  xzx 20180620
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input) {

            using (MD5 md5Hash = MD5.Create()) {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
            
        }
        /// <summary>
        /// base64 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64Encode(string input) {

            if (string.IsNullOrEmpty(input)) throw new ArgumentException("传入非法字符串");

            var inputBytes = Encoding.UTF8.GetBytes(input);

            return Convert.ToBase64String(inputBytes);
        }

        /// <summary>
        /// base 64 解密
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64Str) {

            if (string.IsNullOrEmpty(base64Str)) throw new ArgumentException("传入非法字符串");

            var base64StrBytes = Convert.FromBase64String(base64Str);

            return Encoding.UTF8.GetString(base64StrBytes);

        }

        /// <summary>
        ///  datetime 转为时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTime2UnixTimes(this DateTime dt) {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long timeStamp = (long) (dt - startTime).TotalSeconds;

            return timeStamp.ToString();
        }

        /// <summary>
        /// 时间戳转为 datetime
        /// </summary>
        /// <param name="unixTimeStamp">单位秒</param>
        /// <returns></returns>
        public static DateTime UnixTimes2DateTime(long unixTimeStamp) {

            if (unixTimeStamp < 0) throw new ArgumentException("非法参数");
            
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            DateTime dt = startTime.AddSeconds(unixTimeStamp);

            return dt;
        }

        /// <summary>
        ///  图片转为base 64
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        //public static string Image2Base64(Image image,  System.Drawing.Imaging.ImageFormat format) {
        //    using (MemoryStream ms = new MemoryStream()) {

        //        image.Save(ms, format);

        //        byte[] imageBytes = ms.ToArray();

        //        string base64String = Convert.ToBase64String(imageBytes);

        //        return base64String;
        //    }
        //}

        ///// <summary>
        /////  base64转为图片
        ///// </summary>
        ///// <param name="base64String"></param>
        ///// <returns></returns>
        //public static Image Base64ToImage(string base64String) {
        //    byte[] imageBytes = Convert.FromBase64String(base64String);

        //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

        //    ms.Write(imageBytes, 0, imageBytes.Length);

        //    Image image = Image.FromStream(ms, true);

        //    return image;
        //}

        /// <summary>
        /// 比特数转为kb 、mb 显示格式
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Byte2Fomatstr(long size) {

            string format = string.Empty;

            var kb = size / 1024;

            if(kb >= 1000) {
                format = (kb / 1000).ToString() + "MB";
            }else {
                format = kb.ToString() + "KB";
            }

            return format;
        }

    }
}
