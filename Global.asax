<%@ Application Language="C#" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e) {
        // 應用程式啟動時執行的程式碼
    }

    void Application_End(object sender, EventArgs e) {
        //  應用程式關閉時執行的程式碼
    }

    void Application_Error(object sender, EventArgs e) {
        // 發生未處理錯誤時執行的程式碼
    }

    void Session_Start(object sender, EventArgs e) {
        EzWebCount.CountAdd();
        Session.Add("_newvs", 1);
    }

    void Session_End(object sender, EventArgs e) {
        EzWebCount.CountRemove();
    }
       
</script>
