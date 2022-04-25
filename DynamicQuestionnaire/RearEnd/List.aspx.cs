using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.RearEnd
{
    public partial class List : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private const int _pageSize = 10;
        private List<Question> _delqtl;
        private List<Question> _cdelqtl;
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
                    qList = this._qmgr.GetQuestionList(Caption, StartDate, EndDate, _pageSize, pageIndex, out totalRows);
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
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            List<string> list = new List<string>(); 

            for (var i = 0; i < this.gv.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gv.Rows[i].FindControl($"ckb");
                if (ckb.Checked == true)
                {
                    Label lbl = (Label)this.gv.Rows[i].FindControl("ltlZyunban");
                    string s = lbl.Text;
                    list.Add(s);
                }
            }
            if (list.Count > 0)
            {
                List<Question> Deleteqtl = this._qmgr.GetQuestionList(list, out List<Question> CantDqtl);
                this._delqtl = Deleteqtl;
                this._cdelqtl = CantDqtl;
                string Ds = "";
                for (var i = 0; i < Deleteqtl.Count; i++)
                {
                    if (i != Deleteqtl.Count - 1)
                        Ds += $"{Deleteqtl[i].QName}, ";
                    else
                        Ds += $"{Deleteqtl[i].QName}";
                }
                string deleteS = $"欲刪除的問卷：{Ds}";
                this.ltldlmsg.Text = deleteS;
                //this.ltlDeleteModalContent.Text = deleteS;
                if (CantDqtl.Count > 0)
                {
                    string CDs = "";
                    for (var i = 0; i < CantDqtl.Count; i++)
                    {
                        if (i != CantDqtl.Count - 1)
                            CDs += $"{CantDqtl[i].QName}的紀錄有： {CantDqtl[i].NowKirokusu} 筆,";
                        else
                            CDs += $"{CantDqtl[i].QName}的紀錄有： {CantDqtl[i].NowKirokusu} 筆";
                    }
                    string cantDs = $"其中 【{CDs}】無法刪除。";
                    this.ltldlmsg.Text = $"{deleteS} | {cantDs}";
                    //this.ltlDeleteModalContent.Text = $"{deleteS}{Environment.NewLine}{cantDs}";
                }
                this.btndl.Visible = true;
            }
            
        }
        protected void btndl_Click(object sender, EventArgs e)
        {
            // 排除已經有紀錄的問卷
            for (var i = 0; i < this._cdelqtl.Count; i++)
            {
                foreach (var x in this._delqtl)
                {
                    if (this._cdelqtl[i].Zyunban == x.Zyunban)
                    {
                        this._delqtl.Remove(x);
                    }
                }
            }
            this._qmgr.DeleteQuestion(this._delqtl);
            HttpContext.Current.Session["Msg"] = "刪除成功";
            Response.Redirect(this.Request.Url.LocalPath, true);
        }
        protected void kakuzitudelete_Click(object sender, EventArgs e)
        {
            // 排除已經有紀錄的問卷
            for (var i = 0; i < this._cdelqtl.Count; i++)
            {
                foreach (var x in this._delqtl)
                {
                    if (this._cdelqtl[i].Zyunban == x.Zyunban)
                    {
                        this._delqtl.Remove(x);
                    }
                }
            }
            this._qmgr.DeleteQuestion(this._delqtl);
            HttpContext.Current.Session["Msg"] = "刪除成功";
            Response.Redirect(this.Request.Url.LocalPath, true);
        }
        protected void btnCreate_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Detail.aspx", true);
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

        protected void btndlcancer_Click(object sender, EventArgs e)
        {
            string url = this.Request.Url.LocalPath;
            Response.Redirect(url);
        }
    }
}