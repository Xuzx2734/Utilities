using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class RegexUtilities
    {
        /// <summary>
        /// 匹配 “{}” {} 里面为大小写字母
        /// </summary>
        /// <returns>匹配的数组</returns>
        public static string[] RegexAngle(string str)
        {
            var matchs = Regex.Matches(str, @"({[\w\d|\.]*}+)");
            List<string> matchList = new List<string>();
            foreach (Match item in matchs)
            {
                string keys = item.Value;
                keys = keys.Replace("{", "").Replace("}", "");
                matchList.Add(keys);
            }
            return matchList.ToArray();
        }

        #region 新建的正则方法， 待实践使用测试

        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDigit(this string input)
        {
            Regex reg = new Regex("^[0-9]*$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为n位数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="size">数字要求的位数</param>
        /// <returns></returns>
        public static bool IsVaildDigit(this string input, int size)
        {
            Regex reg = new Regex("^d{"+ size.ToString()+",}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为m-n位的数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsVaildDigit(this string input, int begin, int end)
        {
            Regex reg = new Regex("^d{" + $"{begin},{end}" + ",}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 带1-2位小数的正数或负数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PointDigit(this string input)
        {
            Regex reg = new Regex("^(-)?d+(.d{1,2})$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 正数、负数、和小数：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsRationalNumber(this string input)
        {
            Regex reg = new Regex("^(-|+)?d+(.d+)?$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 有两位小数的正实数：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveNumWith2Point(this string input)
        {
            Regex reg = new Regex("^[0-9]+(.[0-9]{2})?$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 英文和数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsCharOrDigit(string input)
        {
            Regex reg = new Regex("^[A-Za-z0-9]+$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVaildEmail(string input)
        {
            Regex reg = new Regex("^w+([-+.]w+)*@w+([-.]w+)*.w+([-.]w+)*$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为域名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDomainName(string input)
        {
            Regex reg = new Regex("[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(/.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+/.?");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为InternetURL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            Regex reg = new Regex("[a-zA-z]+://[^s]*");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNum(string input)
        {
            Regex reg = new Regex("^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])d{8}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为国内电话
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChTelPhone(string input)
        {
            Regex reg = new Regex("d{3}-d{8}|d{4}-d{7}");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 判断电话号码正则表达式（支持手机号码，3-4位区号，7-8位直播号码，1－4位分机号）:
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTelPhone(string input)
        {
            Regex reg = new Regex("((d{11})|^((d{7,8})|(d{4}|d{3})-(d{7,8})|(d{4}|d{3})-(d{7,8})-(d{4}|d{3}|d{2}|d{1})|(d{7,8})-(d{4}|d{3}|d{2}|d{1}))$)");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为身份证号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVaildIDNum(string input)
        {
            Regex reg = new Regex("(^d{15}$)|(^d{18}$)|(^d{17}(d|X|x)$)");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否合法账号(字母开头，允许5-16字节，允许字母数字下划线)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVaildAccount(string input)
        {
            Regex reg = new Regex("^[a-zA-Z][a-zA-Z0-9_]{4,15}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为合法密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVaildPassword(string input)
        {
            Regex reg = new Regex("^(?=.*d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{8,10}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在 8-10 之间)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStrongPassWord(string input)
        {
            Regex reg = new Regex("^(?=.*d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{8,10}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为强密码(必须包含大小写字母和数字的组合，可以使用特殊字符，长度在8-10之间)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStrongPassWordWithChars(string input)
        {
            Regex reg = new Regex("^(?=.*d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为日期格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateFormat(string input)
        {
            Regex reg = new Regex("^d{4}-d{1,2}-d{1,2}");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为xml文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsXMLFile(string input)
        {
            Regex reg = new Regex(@"^([a-zA-Z]+-?)+[a-zA-Z0-9]+\.[x|X][m|M][l|L]$");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为HTML
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHtmlElement(string input)
        {
            Regex reg = new Regex("<(S*?)[^>]*>.*?|<.*? />");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 去除首尾空白字符(包括空格、制表符、换页符等等)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Trim(string input)
        {
            Regex reg = new Regex("^s*|s*");

            return reg.Replace(input, string.Empty);
        }

        /// <summary>
        /// 是否为腾讯QQ号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsQQAcount(string input)
        {
            Regex reg = new Regex("[1-9][0-9]{4,}");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为中国邮政编码(中国邮政编码为6位数字)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChPostalCode(string input)
        {
            Regex reg = new Regex("[1-9]d{5}(?!d) ");

            return reg.IsMatch(input);
        }

        /// <summary>
        /// 是否为IP地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            Regex reg = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|[01]?\d?\d)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d?\d))");

            return reg.IsMatch(input);
        }

        #endregion

    }
}
