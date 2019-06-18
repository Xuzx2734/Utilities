using Pom.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pom.Utilities
{
    /// <summary>
    /// 企业微信信息格式
    /// </summary>
    public class WXMessage {

        public WXMessage() {
            agentid = Const.AGENTID;
        }

        /// <summary>
        /// 成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。特殊情况：指定为@all，则向该企业应用的全部成员发送
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        /// </summary>
        public string toparty { get; set; }
        /// <summary>
        /// 	标签ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        /// </summary>
        public string totag { get; set; }
        /// <summary>
        /// 消息类型，text | image | video | file | textcard | news | mpnews | miniprogram_notice
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 企业应用的id，整型。可在应用的设置页面查看
        /// </summary>
        public string agentid { get; set; }
        

    }

    /// <summary>
    /// 文本消息格式
    /// </summary>
    public class TextMsg : WXMessage {

        public TextMsg() {
            msgtype = "text";
            safe = 0;
        }

        public TextModel text { get; set; }
        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        public int safe { get; set; }
    }

    public class TextModel {
        /// <summary>
        /// 消息内容，最长不超过2048个字节
        /// </summary>
        public string content { get; set; }

    }

    /// <summary>
    /// 文本消息格式
    /// </summary>
    public class TextCard : WXMessage {

        public TextCard() {
            msgtype = "textcard";
        }

        public CardModel textcard { get; set; }
    }

    public class CardModel {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// <div class=\"gray\">2016年9月26日</div> <div class=\"normal\">恭喜你抽中iPhone 7一台，领奖码：xxxx</div><div class=\"highlight\">请于2016年10月10日前联系行政同事领取</div>
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 按钮文字。 默认为“详情”， 不超过4个文字，超过自动截断。
        /// </summary>
        public string btntxt { get; set; }

    }

    /// <summary>
    /// 图文消息格式
    /// </summary>
    public class News : WXMessage {

        public News() {
            msgtype = "news";
        }

        public NewsModel news { get; set; }

    }

    public class NewsModel {
        public List<ArtModel> articles { get; set; }
    }

    public class ArtModel {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 	图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图 640x320，小图80x80。
        /// </summary>
        public string picurl { get; set; }
        /// <summary>
        /// 按钮文字，仅在图文数为1条时才生效。 默认为“阅读全文”， 不超过4个文字，超过自动截断。该设置只在企业微信上生效，微工作台（原企业号）上不生效。
        /// </summary>
        public string btntxt { get; set; }
    }

}
