using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private const int _pageSize = 10;
        List<Kiroku> _krkl;

        protected void Page_Init(object sender, EventArgs e)
        {
            Guid qID = Guid.Empty;
            Question qt = new Question();
            string qIDtext = this.Request.QueryString["ID"];
            List<QuestionList> qtll = new List<QuestionList>();
            List<Kiroku> krkl = new List<Kiroku>();
            string pageIndexText = this.Request.QueryString["Page"];
            int pageIndex = (string.IsNullOrWhiteSpace(pageIndexText))
                ? 1
                : Convert.ToInt32(pageIndexText);
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
                    List<Mondai> mdl = this._qmgr.GetMondaiList();
                    this.ddlTitle.DataSource = mdl;
                    this.ddlTitle.DataTextField = "Title";
                    this.ddlTitle.DataValueField = "MondaiID";
                    this.ddlTitle.DataBind();
                    // 填寫資料頁
                    krkl = this._qmgr.GetKirokuWithStastic(qID, _pageSize, pageIndex, out int totalRows);
                    this._krkl = krkl;
                    this.gvKiroku.DataSource = krkl;
                    this.gvKiroku.DataBind();
                    this.Pager.TotalRow = totalRows;
                    this.Pager.PageIndex = pageIndex;
                    this.Pager.Bind();
                    // 有帶kirokuID進來的話
                    Kiroku krk = new Kiroku();
                    if (HttpContext.Current.Session["siryou"] != null)
                    {
                        string kirokuIDs = HttpContext.Current.Session["siryou"].ToString();
                        if (Guid.TryParse(kirokuIDs, out Guid kirokuid))
                        {
                            for (var i = 0; i < krkl.Count; i++)
                            {
                                if (string.Compare(kirokuid.ToString(), krkl[i].KirokuID.ToString()) == 0)
                                {
                                    krk = krkl[i];
                                }
                            }
                            this.txbkirokuname.Text = krk.Name;
                            this.txbkirokuphone.Text = krk.Phone;
                            this.txbkirokuemail.Text = krk.Email;
                            this.txbkirokuage.Text = krk.Age.ToString();
                            this.ltlkirokudate.Text = $"填寫時間： {krk.Date}";
                            // 動態生成控制項
                            for (var i = 0; i < qtll.Count; i++)
                            {
                                Label lbl = new Label();
                                string lbltext = $"{i + 1}. {qtll[i].Title}";
                                if (qtll[i].Zettai == 1)
                                    lbltext = $"{i + 1}. {qtll[i].Title} *";
                                lbl.Text = lbltext;
                                this.plhkiroku.Controls.Add(lbl);
                                switch (qtll[i].Type)
                                {
                                    case 1:
                                        RadioButtonList rdbl = new RadioButtonList();
                                        rdbl.ID = $"Mondai{i}";
                                        for (var j = 0; j < qtll[i].NaiyoList.Count; j++)
                                        {
                                            ListItem lti = new ListItem();
                                            lti.Value = $"{i}_{j}";
                                            lti.Text = qtll[i].NaiyoList[j].Naiyo;
                                            rdbl.Items.Add(lti);
                                        }
                                        this.plhkiroku.Controls.Add(rdbl);
                                        break;
                                    case 2:
                                        CheckBoxList ckbl = new CheckBoxList();
                                        ckbl.ID = $"Mondai{i}";
                                        for (var j = 0; j < qtll[i].NaiyoList.Count; j++)
                                        {
                                            ListItem ckb = new ListItem();
                                            ckb.Value = $"{i}_{j}";
                                            ckb.Text = qtll[i].NaiyoList[j].Naiyo;
                                            ckbl.Items.Add(ckb);
                                        }
                                        this.plhkiroku.Controls.Add(ckbl);
                                        break;
                                    case 3:
                                        TextBox txb = new TextBox();
                                        txb.ID = $"Mondai{i}";
                                        txb.TextMode = TextBoxMode.MultiLine;
                                        this.plhkiroku.Controls.Add(txb);
                                        break;
                                    case 4:
                                        TextBox txbsuzi = new TextBox();
                                        txbsuzi.ID = $"Mondai{i}";
                                        txbsuzi.TextMode = TextBoxMode.Number;
                                        this.plhkiroku.Controls.Add(txbsuzi);
                                        break;
                                    case 5:
                                        TextBox txbemail = new TextBox();
                                        txbemail.ID = $"Mondai{i}";
                                        txbemail.TextMode = TextBoxMode.Email;
                                        this.plhkiroku.Controls.Add(txbemail);
                                        break;
                                    case 6:
                                        TextBox txbdate = new TextBox();
                                        txbdate.ID = $"Mondai{i}";
                                        txbdate.TextMode = TextBoxMode.Date;
                                        this.plhkiroku.Controls.Add(txbdate);
                                        break;

                                }
                            }
                            // 帶入值
                            for (var i = 0; i < krk.KirokuList.Count; i++)
                            {
                                switch (krk.KirokuList[i].Type)
                                {
                                    case 1:
                                        RadioButtonList rdb = (RadioButtonList)this.plhkiroku.FindControl($"Mondai{i}");
                                        for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                                        {
                                            if (string.Compare(rdb.Items[j].Text, krk.KirokuList[i].Naiyo) == 0)
                                            {
                                                rdb.Items[j].Selected = true;
                                                break;
                                            }
                                        }
                                        break;
                                    case 2:
                                        CheckBoxList ckb = (CheckBoxList)this.plhkiroku.FindControl($"Mondai{i}");
                                        for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                                        {
                                            for (var k = 0; k < krk.KirokuList[i].ckbNaiyo.Count; k++)
                                            {
                                                if (string.Compare(ckb.Items[j].Text, krk.KirokuList[i].ckbNaiyo[k]) == 0)
                                                {
                                                    ckb.Items[j].Selected = true;
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        TextBox txb = (TextBox)this.plhkiroku.FindControl($"Mondai{i}");
                                        txb.Text = krk.KirokuList[i].Naiyo;
                                        break;
                                }
                            }
                            this.pnlsiryou2fuuin.Enabled = false;
                            this.pnlsiryou2.Visible = true;
                            this.pnlsiryou1.Visible = false;
                        }
                    }
                    // 統計頁
                    // 統計問題結果數字
                    List<QuestionListKazuandKiroku> qtlKandKl = new List<QuestionListKazuandKiroku>();
                    for (var i = 0; i < _qtll.Count; i++)
                    {
                        QuestionListKazuandKiroku qtlKandK = new QuestionListKazuandKiroku()
                        {
                            Title = _qtll[i].Title,
                            Type = _qtll[i].Type,
                            Zettai = _qtll[i].Zettai,
                            KazuandKiroku = new List<KazuandKiroku>(),
                        };
                        for (var j = 0; j < _qtll[i].NaiyoList.Count; j++)
                        {
                            KazuandKiroku kak = new KazuandKiroku()
                            {
                                Naiyo = _qtll[i].NaiyoList[j].Naiyo,
                                Kazu = 0,
                            };
                            for (var k = 0; k < krkl.Count; k++)
                            {
                                for (var x = 0; x < krkl[k].KirokuList.Count; x++)
                                {
                                    switch (krkl[k].KirokuList[x].Type)
                                    {
                                        case 1:
                                            if (string.Compare(krkl[k].KirokuList[x].Naiyo, _qtll[i].NaiyoList[j].Naiyo) == 0)
                                            {
                                                kak.Kazu += 1;
                                            }
                                            break;
                                        case 2:
                                            for (var y = 0; y < krkl[k].KirokuList[x].ckbNaiyo.Count; y++)
                                            {
                                                if (string.Compare(krkl[k].KirokuList[x].ckbNaiyo[y], _qtll[i].NaiyoList[j].Naiyo) == 0)
                                                {
                                                    kak.Kazu += 1;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            qtlKandK.KazuandKiroku.Add(kak);
                        }
                        qtlKandKl.Add(qtlKandK);
                    }
                    // 動態生成統計
                    for (var i = 0; i < qtlKandKl.Count; i++)
                    {
                        Label lbl = new Label();
                        string lbltext = $"{i + 1}. {qtll[i].Title}";
                        if (qtlKandKl[i].Zettai == 1)
                            lbltext = $"{i + 1}. {qtll[i].Title} *";
                        lbl.Text = lbltext;
                        this.plhstatic.Controls.Add(lbl);
                        switch (qtlKandKl[i].Type)
                        {
                            case 1:
                                for (var j = 0; j < qtlKandKl[i].KazuandKiroku.Count; j++)
                                {
                                    Label lblnaiyo = new Label();
                                    double a = qtlKandKl[i].KazuandKiroku[j].Kazu;
                                    double b = qtlKandKl.Count;
                                    double kazupa = a / b;
                                    string lblnaiyotext = $" 第{j + 1}單選 {kazupa * 100}% ({qtlKandKl[i].KazuandKiroku[j].Kazu})";
                                    lblnaiyo.Text = lblnaiyotext;
                                    this.plhstatic.Controls.Add(lblnaiyo);
                                }
                                break;
                            case 2:
                                for (var j = 0; j < qtlKandKl[i].KazuandKiroku.Count; j++)
                                {
                                    Label lblnaiyo = new Label();
                                    double a = qtlKandKl[i].KazuandKiroku[j].Kazu;
                                    double b = qtlKandKl.Count;
                                    double kazupa = a / b;
                                    string lblnaiyotext = $" 第{j + 1}複選 {kazupa * 100}% ({qtlKandKl[i].KazuandKiroku[j].Kazu})";
                                    lblnaiyo.Text = lblnaiyotext;
                                    this.plhstatic.Controls.Add(lblnaiyo);
                                }
                                break;
                            default:
                                Label lblnaiyodefault = new Label();
                                string lblnaiyodefaulttext = "-";
                                lblnaiyodefault.Text = lblnaiyodefaulttext;
                                this.plhstatic.Controls.Add(lblnaiyodefault);
                                break;
                        }
                    }
                }
            }
            else
            {
                // 問卷頁
                this.txbS.Text = DateTime.Now.ToString();
                this.ckbState.Checked = true;
                if (HttpContext.Current.Session["Question"] != null)
                {
                    qt = (Question)HttpContext.Current.Session["Question"];
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
                // 填寫資料頁
                this.gvKiroku.DataSource = krkl;
                this.gvKiroku.DataBind();
                int totalRows = 0;
                this.Pager.TotalRow = totalRows;
                this.Pager.PageIndex = pageIndex;
                this.Pager.Bind();
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
                    DateEnd = DateTime.TryParse(this.txbE.Text, out DateTime dte) == false ? de : dte,
                    State = this.ckbState.Checked == true ? 1 : 2,
                };
                HttpContext.Current.Session["Question"] = qt;
                HttpContext.Current.Session["QuestionListl"] = null;
                HttpContext.Current.Session["Editqtll"] = null;
            }
            HttpContext.Current.Session["ChangeTab"] = "mondai";
            this.changetab.Value = "mondai";
            Response.Redirect(Request.RawUrl + "#nav-mondai");
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
                    HttpContext.Current.Session["ChangeTab"] = "nav-mondai";
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
                        qtll[i].Type = Convert.ToInt32(this.ddlType.SelectedValue);
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
            this.ltlmondaimsg.Text = "";
        }
        protected void btnMondaigogogo_Click(object sender, EventArgs e)
        {
            Question qt = new Question();
            List<QuestionList> qtll = new List<QuestionList>();
            if (HttpContext.Current.Session["Editqt"] != null)
            {
                if (this._krkl.Count > 0)
                {
                    this.ltlmondaimsg.Text = $"目前已經有 {this._krkl.Count} 筆紀錄，所以無法更改題目。";
                    return;
                }
                qtll = (List<QuestionList>)HttpContext.Current.Session["Editqtll"];
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
            Response.Redirect("List.aspx", true);
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
                    qtll[i].Zyunban = i + 1;
                }
            }
            if (HttpContext.Current.Session["Editqtll"] != null)
                HttpContext.Current.Session["Editqtll"] = qtll;
            else
                HttpContext.Current.Session["QuestionListl"] = qtll;
            this.gv.DataSource = qtll;
            this.gv.DataBind();
        }
        protected void btnTocsv_Click(object sender, EventArgs e)
        {

            List<Kiroku> krkl = new List<Kiroku>();
            krkl = this._qmgr.GetKirokuWithStastic(this._qID);
            if (krkl.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Email");
                dt.Columns.Add("Age");
                dt.Columns.Add("Date");
                dt.Columns.Add("Title");
                dt.Columns.Add("Type");
                dt.Columns.Add("Naiyo");
                // 放入資料
                for (var i = 0; i < krkl.Count; i++)
                {
                    for (var j = 0; j < krkl[i].KirokuList.Count; j++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Name"] = krkl[i].Name;
                        dr["Phone"] = krkl[i].Phone;
                        dr["Email"] = krkl[i].Email;
                        dr["Age"] = krkl[i].Age;
                        dr["Date"] = krkl[i].Date;
                        dr["Title"] = krkl[i].KirokuList[j].Title;
                        dr["Type"] = krkl[i].KirokuList[j].Type;
                        if (krkl[i].KirokuList[j].Type == 2)
                        {
                            string s = "";
                            for (var k = 0; k < krkl[i].KirokuList[j].ckbNaiyo.Count; k++)
                            {
                                if (i != krkl[i].KirokuList[j].ckbNaiyo.Count - 1)
                                    s += $"{krkl[i].KirokuList[j].ckbNaiyo[k]}&";
                                else
                                    s += krkl[i].KirokuList[j].ckbNaiyo[k];
                            }
                            dr["Naiyo"] = s;
                        }
                        else
                            dr["Naiyo"] = krkl[i].KirokuList[j].Naiyo;
                        dt.Rows.Add(dr);
                    }
                }
                this.CreateCSVFile(dt, "C:\\temp");
            }
            else
            {
                this.ltlsiryoumsg.Text = "無資料可匯出";
                return;
            }

        }
        public void CreateCSVFile(DataTable dt, string strFilePath)
        {
            #region Export Grid to CSV     
            if (!Directory.Exists(strFilePath)) // 假如資料夾不存在，先建立
                Directory.CreateDirectory(strFilePath);
            string fileName = this._qt.QName + DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + ".csv";
            string newFilePath = Path.Combine(strFilePath, fileName);
            // Create the CSV file to which grid data will be exported.    
            StreamWriter sw = new StreamWriter(newFilePath, false);

            // First we will write the headers.    
            //DataTable dt = m_dsProducts.Tables[0];    
            int iColCount = dt.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.    
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            #endregion
        }
        protected void gvKiroku_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "siryou":
                    HttpContext.Current.Session["siryou"] = e.CommandArgument;
                    Response.Redirect(Request.RawUrl + "#nav-siryou");
                    break;
            }
        }
        protected void btnkirokucancer_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["siryou"] = null;
            this.pnlsiryou2.Visible = false;
            this.pnlsiryou1.Visible = true;
            Response.Redirect(Request.RawUrl + "#nav-siryou");
        }
        protected void ddlTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Mondai> mdl = this._qmgr.GetMondaiList();
            for (var i = 0; i < mdl.Count; i++)
            {
                if (string.Compare(this.ddlTitle.SelectedValue, mdl[i].MondaiID.ToString()) == 0)
                {
                    this.txbquestion.Text = mdl[i].Title;
                    this.txbNaiyo.Text = mdl[i].Naiyo;
                    this.ddlType.SelectedValue = mdl[i].Type.ToString();
                    this.ckbhituyou.Checked = mdl[i].Zettai == 1 ? true : false;
                }
            }
        }
    }
}