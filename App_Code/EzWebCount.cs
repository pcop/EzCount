using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Xml;

#region <<-- EzWebCount 簡易網頁計數器 -->>
/// <summary>
/// EzWebCount 簡易網頁計數器
/// </summary>
public class EzWebCount {

    #region <--- CountAdd 新訪客進入 -->
    /// <summary>
    /// CountAdd 新訪客進入
    /// </summary>
    public static void CountAdd() {

        _currentNumberOfUsers++;

        String _DataXmlFile = HttpContext.Current.Server.MapPath("~/App_Data/CountDat.xml");
        String _today = DateTime.Today.ToString("yyyyMMdd");

        XmlDocument XDoc = new XmlDocument();
        XDoc.Load(_DataXmlFile);

        XmlNode CounterNode = XDoc.SelectSingleNode("//counter");
        totalNumberOfUsers = Convert.ToInt32(CounterNode.Attributes["total"].Value) + 1;
        CounterNode.Attributes["total"].Value = totalNumberOfUsers.ToString();

        String _CountDatDate = CounterNode.Attributes["date"].Value;

        ///是否跨日
        Boolean _IsDayChange = (_CountDatDate != _today);

        //跨日歸零
        todayNumberOfUsers = _IsDayChange ? 1 : Convert.ToInt32(CounterNode.Attributes["day"].Value) + 1;
        CounterNode.Attributes["day"].Value = todayNumberOfUsers.ToString();
        if (_IsDayChange) CounterNode.Attributes["date"].Value = _today;

        //是否產生日紀錄檔.
        if (_IsDayChange && IsDoDayLog) {
            String _DayLogFile = HttpContext.Current.Server.MapPath("~/App_Data/CountDatLog_" + _CountDatDate + ".xml");
            File.Copy(_DataXmlFile, _DayLogFile, true);
        }

        XDoc.Save(_DataXmlFile);
    }
    #endregion

    #region <-- CountRemove 訪客離開 -->
    /// <summary>
    ///  CountRemove 訪客離開 
    /// </summary>
    public static void CountRemove() {
        _currentNumberOfUsers--;
    }
    #endregion

    #region <-- totalNumberOfUsers 累計瀏覽人數 -->
    /// <summary>
    /// totalNumberOfUsers 累計瀏覽人數
    /// </summary>
    private static Int32 _totalNumberOfUsers = 0;
    /// <summary>
    /// totalNumberOfUsers 累計瀏覽人數
    /// </summary>
    public static Int32 totalNumberOfUsers {
        get {
            return _totalNumberOfUsers;
        }
        private set {
            _totalNumberOfUsers = value;
        }
    }
    #endregion

    #region <-- todayNumberOfUsers 今日瀏覽人數 -->
    /// <summary>
    /// todayNumberOfUsers 今日瀏覽人數
    /// </summary>
    private static Int32 _todayNumberOfUsers = 0;
    /// <summary>
    /// todayNumberOfUsers 今日瀏覽人數
    /// </summary>
    public static Int32 todayNumberOfUsers {
        get {
            return _todayNumberOfUsers;
        }
        private set {
            _todayNumberOfUsers = value;
        }
    }
    #endregion

    #region <-- currentNumberOfUsers 目前線上人數. -->
    /// <summary>
    /// currentNumberOfUsers 目前線上人數.
    /// </summary>
    private static Int32 _currentNumberOfUsers = 0;
    /// <summary>
    /// currentNumberOfUsers
    /// </summary>
    public static Int32 currentNumberOfUsers {
        get {
            return _currentNumberOfUsers;
        }
        private set {
            _currentNumberOfUsers = (value < 0) ? 0 : value;
        }
    }
    #endregion

    #region <-- IsDoDayLog 是否產生每日紀錄檔 -->
    /// <summary>
    /// IsDoDayLog 是否產生每日紀錄檔
    /// </summary>
    private const Boolean IsDoDayLog = true;
    #endregion

}
#endregion

#region <<-- GetPageCount 邏輯頁 -->>
public class GetPageCount : IHttpHandler, IRequiresSessionState {

    public void ProcessRequest(HttpContext context) {
        context.Response.ContentType = "text/javascript";
        context.Response.Write(
                String.Format(
                "$(function(){{$('#web_count_online').html('{0}');$('#web_count_total').html('{1}');$('#web_count_day').html('{2}');}});",
                EzWebCount.currentNumberOfUsers, EzWebCount.totalNumberOfUsers, EzWebCount.todayNumberOfUsers)
            );
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
#endregion