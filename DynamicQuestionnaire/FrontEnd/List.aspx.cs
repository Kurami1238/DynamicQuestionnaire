using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.FrontEnd
{
    public partial class List : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private const int _pageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageIndexText = this.Request.QueryString["Page"];
            int pageIndex = (string.IsNullOrWhiteSpace(pageIndexText))
                ? 1
                : Convert.ToInt32(pageIndexText);
            string Caption = this.Request.QueryString["Caption"];
            if (!string.IsNullOrWhiteSpace(Caption))
                this.txbT.Text = Caption;

            string StartDateText = this.Request.QueryString["StartDate"];
            DateTime StartDate = (string.IsNullOrWhiteSpace(StartDateText))
                ? Convert.ToDateTime("1900/1/1")
                : Convert.ToDateTime(StartDateText);
            string EndDateText = this.Request.QueryString["EndDate"];
            DateTime EndDate = (string.IsNullOrWhiteSpace(EndDateText))
                ? DateTime.MaxValue
                : Convert.ToDateTime(EndDateText);

            if (!IsPostBack)
            {
                List<Question> qList = new List<Question>();
                int totalRows = 0;
                if (Caption == null)
                    qList = this._qmgr.GetQuestionList(_pageSize, pageIndex, out totalRows);
                else
                    qList = this._qmgr.GetQuestionList(Caption,StartDate, EndDate, _pageSize, pageIndex, out totalRows);
                this.gv.DataSource = qList;
                this.gv.DataBind();

                this.Pager.TotalRow = totalRows;
                this.Pager.PageIndex = pageIndex;
                this.Pager.Bind("Caption", Caption);
            }
            // 提示使用者訊息
            if (HttpContext.Current.Session["Msg"] != null)
            {
                this.msgmsg.Value = HttpContext.Current.Session["Msg"].ToString();
                HttpContext.Current.Session["Msg"] = null;
            }
        }

        protected void btnS_Click(object sender, EventArgs e)
        {

            DateTime S = Convert.ToDateTime("1900/1/1");
            DateTime E = DateTime.MaxValue;
            string caption = string.IsNullOrWhiteSpace(this.txbT.Text)
                ? "" : this.txbT.Text;
            List<string> errormsg = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.txbS.Text))
            {
                if (DateTime.TryParse(this.txbS.Text, out DateTime DateStart))
                    S = DateStart;
                else
                    errormsg.Add("開始請輸入日期");
            }
            if (!string.IsNullOrWhiteSpace(this.txbE.Text))
            {
                if (DateTime.TryParse(this.txbE.Text, out DateTime DateEnd))
                    E = DateEnd;
                else
                    errormsg.Add("結束請輸入日期");
            }
            if (errormsg.Count > 0)
            {
                this.ltlmsg.Text = string.Empty;
                foreach (var x in errormsg)
                {
                    string s = x.Replace(x, x + Environment.NewLine);
                    this.ltlmsg.Text += s;
                }
                return;
            }
            string url = this.Request.Url.LocalPath + "?Caption=" + caption + "&StartDate=" + S.ToString() + "&EndDate=" + E.ToString();
            Response.Redirect(url);
        }
    }
}