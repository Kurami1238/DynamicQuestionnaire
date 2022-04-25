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
    public partial class Detail : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private Guid _qID;
        private List<QuestionList> _qtll;
        private Question _qt;
        protected void Page_Init(object sender, EventArgs e)
        {
            Guid qID = Guid.Empty;
            Question qt = new Question();
            string qIDtext = this.Request.QueryString["ID"];
            List<QuestionList> qtll = new List<QuestionList>();
            // 檢測跳頁籤
            if (HttpContext.Current.Session["ChangeTab"] != null)
            {
                //string s = HttpContext.Current.Session["ChangeTab"].ToString();
                //Response.Redirect(Request.RawUrl + $"#nav-{s}");
                this.changetab.Value = HttpContext.Current.Session["ChangeTab"].ToString();
                HttpContext.Current.Session["ChangeTab"] = null;
            }
            // 初始化開始 有帶ID進來就是編輯模式
            if (qIDtext != null)
            {
                if (Guid.TryParse(qIDtext, out qID))
                {
                    // 問卷頁
                    qt = this._qmgr.GetQuestion(qID, out qtll);
                    this._qID = qID;
                    this._qtll = qtll;
                    this._qt = qt;
                    this.txbQname.Text = qt.QName;
                    this.txbQsetume.Text = qt.QSetume;
                    this.txbS.Text = qt.DateStart.ToString();
                    if (qt.DateEnd != null)
                        this.txbE.Text = qt.DateEnd.ToString();
                    this.ckbState.Checked = qt.State == 1 ? true : false;
                    if (HttpContext.Current.Session["Editqt"] == null)
                    {
                        HttpContext.Current.Session["Editqt"] = qt;
                        HttpContext.Current.Session["Editqtll"] = qtll;
                    }
                    else
                    {
                        qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
                    }
                    // 問題頁
                    this.gv.DataSource = qtll;
                    this.gv.DataBind();
                }
            }
            else
            {
                // 問卷頁
                this.txbS.Text = DateTime.Now.ToString();
                this.ckbState.Checked = true;
                if (HttpContext.Current.Session["Question"] != null)
                {
                    qt= (Question)HttpContext.Current.Session["Question"];
                    this.txbQname.Text = qt.QName;
                    this.txbQsetume.Text = qt.QSetume;
                    this.txbS.Text = qt.DateStart.ToString();
                    if (qt.DateEnd != null)
                        this.txbE.Text = qt.DateEnd.ToString();
                }
                if (HttpContext.Current.Session["QuestionListl"] != null)
                     qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
                // 問題頁
                this.gv.DataSource = qtll;
                this.gv.DataBind();
            }
            this.ph.DataBind();
        }
        protected void cancer_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx", true);
        }
        protected void gogogo_Click(object sender, EventArgs e)
        {
            List<string> errormsg = new List<string>();
            string name = "";
            string setume = "";
            DateTime ds = DateTime.Now;
            DateTime? de = null;
            if (!string.IsNullOrWhiteSpace(this.txbQname.Text))
                name = this.txbQname.Text;
            else
                errormsg.Add("問卷名稱不得為空白");
            if (!string.IsNullOrWhiteSpace(this.txbQsetume.Text))
                setume = this.txbQsetume.Text;
            else
                errormsg.Add("描述內容不得為空白");
            if (!DateTime.TryParse(this.txbS.Text, out ds))
                errormsg.Add("開始時間不得為空白");
            if (errormsg.Count > 0)
            {
                this.ltlquestionmsg.Text = string.Empty;
                foreach (var x in errormsg)
                {
                    string s = x.Replace(x, x + Environment.NewLine);
                    this.ltlquestionmsg.Text += s;
                }
                return;
            }
            Question qt = new Question();
            if (HttpContext.Current.Session["Editqt"] != null)
            {
                qt = (Question)HttpContext.Current.Session["Editqt"];
                qt.QName = name;
                qt.QSetume = setume;
                qt.DateStart = ds;
                //qt.DateEnd = Convert.ToDateTime(this.txbE.Text) != null ? Convert.ToDateTime(this.txbE.Text) : de; 
                qt.DateEnd = DateTime.TryParse(this.txbE.Text, out DateTime dte) == false ? de : dte;
                qt.State = this.ckbState.Checked == true ? 1 : 2;
                HttpContext.Current.Session["Editqt"] = qt;
                HttpContext.Current.Session["QuestionListl"] = null;

            }
            else
            {
                qt = new Question()
                {
                    QuestionID = Guid.NewGuid(),
                    QuestionListID = Guid.NewGuid(),
                    QName = name,
                    QSetume = setume,
                    DateStart = ds,
                    DateEnd = DateTime.TryParse(this.txbE.Text,out DateTime dte) == false ? de : dte,
                    State = this.ckbState.Checked == true ? 1 : 2,
                };
                HttpContext.Current.Session["Question"] = qt;
                HttpContext.Current.Session["QuestionListl"] = null;
                HttpContext.Current.Session["Editqtll"] = null;
            }
            HttpContext.Current.Session["ChangeTab"] = "mondai";
            this.changetab.Value = "mondai";
        }
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<QuestionList> qtll = new List<QuestionList>();
            switch (e.CommandName)
            {

                case "btnEdit":
                    if (HttpContext.Current.Session["Editqtll"] != null)
                        qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
                    else
                        qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
                    for (var i = 0; i < qtll.Count; i++)
                        {
                            if (string.Compare(qtll[i].NaiyoListID.ToString(), e.CommandArgument.ToString()) == 0)
                            {
                                this.txbquestion.Text = qtll[i].Title;
                                for (var j = 0; j < qtll[i].NaiyoList.Count; j++)
                                {
                                    if (i != qtll[i].NaiyoList.Count - 1)
                                        this.txbNaiyo.Text += qtll[i].NaiyoList[j].Naiyo + ';';
                                    else
                                        this.txbNaiyo.Text += qtll[i].NaiyoList[j].Naiyo;
                                }
                                this.ddlType.SelectedValue = qtll[i].Type.ToString();
                                this.ckbhituyou.Checked = qtll[i].Zettai == 1 ? true : false;
                            // 加入編輯QTLL的session 讓 qtl的加入辨認是否要覆蓋qtl
                            HttpContext.Current.Session["EditNowqtl"] = qtll[i];
                            }
                        }
                    break;
            }
        }
        protected void btnCreateMondai_Click(object sender, EventArgs e)
        {
            List<string> errormsg = new List<string>();
            string title = "";
            List<string> naiyolist = new List<string>();
            switch (this.ddlType.SelectedValue)
            {
                case "1":
                case "2":
                    if (!string.IsNullOrWhiteSpace(this.txbquestion.Text))
                        title = this.txbquestion.Text;
                    else
                        errormsg.Add("問題不得為空白");
                    string[] spnl;
                    if (!string.IsNullOrWhiteSpace(this.txbNaiyo.Text))
                    {
                        spnl = this.txbNaiyo.Text.Split(';');
                        foreach (var x in spnl)
                        {
                            naiyolist.Add(x);
                        }
                    }
                    else
                        errormsg.Add("回答不得為空白");
                    break;

                default:
                    if (!string.IsNullOrWhiteSpace(this.txbquestion.Text))
                        title = this.txbquestion.Text;
                    else
                        errormsg.Add("問題不得為空白");
                    break;
            }
            if (errormsg.Count > 0)
            {
                this.ltlmondaimsg.Text = string.Empty;
                foreach (var x in errormsg)
                {
                    string s = x.Replace(x, x + Environment.NewLine);
                    this.ltlmondaimsg.Text += s;
                }
                return;
            }
            QuestionList qtl = new QuestionList();
            Question qt = new Question();
            NaiyoList nyl = new NaiyoList();
            List<NaiyoList> nyll = new List<NaiyoList>();
            List<QuestionList> qtll = new List<QuestionList>();
            // 判斷是不是從gv的編輯模式上來的
            if (HttpContext.Current.Session["EditNowqtl"] == null)
            {
                switch (this.ddlTitle.SelectedValue)
                {
                    case "1":
                        if (HttpContext.Current.Session["Editqt"] != null)
                        {
                            qt = (Question)HttpContext.Current.Session["Editqt"];
                            qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
                        }
                        else
                        {
                            qt = (Question)HttpContext.Current.Session["Question"];
                            if (HttpContext.Current.Session["QuestionListl"] != null)
                                qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
                        }
                        qtl = new QuestionList()
                        {
                            QuestionListID = qt.QuestionListID,
                            Title = title,
                            Type = Convert.ToInt32(this.ddlType.SelectedValue),
                            Zyunban = qtll.Count + 1,
                            Zettai = this.ckbhituyou.Checked == true ? 1 : 2,
                        };
                        // Type= 1,2 把回答加進qtl
                        switch (qtl.Type)
                        {
                            case 1:
                            case 2:
                                qtl.NaiyoListID = Guid.NewGuid();
                                Guid.TryParse(qtl.NaiyoListID.ToString(), out Guid nlID);
                                for (var i = 0; i < naiyolist.Count; i++)
                                {
                                    nyl = new NaiyoList()
                                    {
                                        NaiyoListID = nlID,
                                        Naiyo = naiyolist[i],
                                    };
                                    nyll.Add(nyl);
                                }
                                qtl.NaiyoList = nyll;
                                break;
                            default:
                                break;
                        }
                        qtll.Add(qtl);
                        if (HttpContext.Current.Session["Editqt"] != null)
                            HttpContext.Current.Session["Editqtll"] = qtll;
                        else
                            HttpContext.Current.Session["QuestionListl"] = qtll;
                        break;
                    // 常用問題管理
                    default:
                        break;
                }
            }
            else
            {
                if (HttpContext.Current.Session["Editqtll"] != null)
                    qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
                else
                    qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
                qtl = (QuestionList)HttpContext.Current.Session["EditNowqtl"];
                for (var i = 0; i < qtll.Count; i++)
                {
                    // 比對qtll內與編輯模式上來的ID 找到以後替換內容
                    if (string.Compare(qtll[i].NaiyoListID.ToString(), qtl.NaiyoListID.ToString()) == 0)
                    {
                        qtll[i].Title = this.txbquestion.Text;
                        qtll[i].Type = Convert.ToInt32( this.ddlType.SelectedValue);
                        qtll[i].Zettai = this.ckbhituyou.Checked == true ? 1 : 2;
                        switch (qtll[i].Type)
                        {
                            case 1:
                            case 2:
                                if (qtll[i].NaiyoListID == null)
                                    qtll[i].NaiyoListID = Guid.NewGuid();
                                Guid.TryParse(qtl.NaiyoListID.ToString(), out Guid nlID);
                                for (var j = 0; j < naiyolist.Count; j++)
                                {
                                    nyl = new NaiyoList()
                                    {
                                        NaiyoListID = nlID,
                                        Naiyo = naiyolist[j],
                                    };
                                    nyll.Add(nyl);
                                }
                                qtll[i].NaiyoList = nyll;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (HttpContext.Current.Session["Editqtll"] != null)
                    HttpContext.Current.Session["Editqtll"] = qtll;
                else
                    HttpContext.Current.Session["QuestionListl"] = qtll;
                HttpContext.Current.Session["EditNowqtl"] = null;
            }
            this.gv.DataSource = qtll;
            this.gv.DataBind();
            HttpContext.Current.Session["ChangeTab"] = "mondai";
        }
        protected void btnCancerMondai_Click(object sender, EventArgs e)
        {
            List<QuestionList> qtll = new List<QuestionList>();
            if (HttpContext.Current.Session["Editqtll"] != null)
            {
                qtll = this._qtll;
                HttpContext.Current.Session["Editqtll"] = this._qtll;
            }
            HttpContext.Current.Session["QuestionListl"] = null;
            this.gv.DataSource = qtll;
            this.gv.DataBind();
        }
        protected void btnMondaigogogo_Click(object sender, EventArgs e)
        {
            Question qt = new Question();
            List<QuestionList> qtll = new List<QuestionList>();
            if (HttpContext.Current.Session["Editqt"] != null)
            {
                qtll =(List<QuestionList>)HttpContext.Current.Session["Editqtll"];
                qt = (Question)HttpContext.Current.Session["Editqt"];
                this._qmgr.UpdateQuestion(qt, qtll);
            }
            else
            {
                qt = (Question)HttpContext.Current.Session["Question"];
                qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
                this._qmgr.CreateQuestion(qt, qtll);
            }
            HttpContext.Current.Session["Msg"] = "新增問卷成功";
            Response.Redirect("List.aspx",true);
        }
        protected void btnDeleteMondai_Click(object sender, ImageClickEventArgs e)
        {
            List<string> list = new List<string>();
            List<QuestionList> qtll = new List<QuestionList>();
            if (HttpContext.Current.Session["Editqtll"] != null)
                qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
            else
                qtll = (List<QuestionList>)HttpContext.Current.Session["QuestionListl"];
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
                // 刪除清單內的問題

                for (var i = 0; i < list.Count; i++)
                {
                    for (var j = 0; j < qtll.Count; j++)
                    {
                        if (string.Compare(list[i], qtll[j].Zyunban.ToString()) == 0)
                        {
                            qtll.Remove(qtll[j]);
                        }
                    }
                }
                // 重新排序
                for (var i = 0; i < qtll.Count; i++)
                {
                    qtll[i].Zyunban = i+1;
                }
            }
            if (HttpContext.Current.Session["Editqtll"] != null)
                HttpContext.Current.Session["Editqtll"] = qtll;
            else
                HttpContext.Current.Session["QuestionListl"] = qtll;
            this.gv.DataSource = qtll;
            this.gv.DataBind();
        }
    }
}