using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.FrontEnd
{
    public partial class Form : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private Guid _qID;
        private List<QuestionList> _qtll;
        protected void Page_Init(object sender, EventArgs e)
        {
            Guid qID = Guid.Empty;
            Question qt = new Question();
            List<QuestionList> qtll = new List<QuestionList>();
            //if (!IsPostBack)
            {
                string qIDtext = this.Request.QueryString["ID"];
                if (Guid.TryParse(qIDtext, out qID))
                {
                    qt = this._qmgr.GetQuestion(qID, out qtll);
                    this._qID = qID;
                    this._qtll = qtll;
                }
                if (DateTime.Compare(qt.DateEnd, DateTime.Now) > 0)
                    this.State.Text = "投票中";
                else
                    this.State.Text = "已結束";
                this.Date.Text = $"{qt.DateStart}～{qt.DateEnd}";
                this.lblQname.Text = qt.QName;
                this.lblQSetume.Text = qt.QSetume;
                this.Total.Text = $"共{qtll.Count}個問題";
                // State 1 = 開啟, State 2 = 關閉
                if (DateTime.Compare(qt.DateStart, DateTime.Now) > 0 || DateTime.Compare(qt.DateEnd, DateTime.Now) < 0 || qt.State == 2)
                {
                    this.pnl.Enabled = false;
                    this.gogogo.Visible = false;
                }

                //this.plh.Controls.Clear(); // 清除生成的問卷控制項
                // 根據有幾個問題動態生成幾項問題
                // Type 1 = 單選方塊, Type 2 = 複選方塊, Type 3 = 文字方塊
                for (var i = 0; i < qtll.Count; i++)
                {
                    Label lbl = new Label();
                    string lbltext = $"{i + 1}. {qtll[i].Title}";
                    lbl.Text = lbltext;
                    this.plh.Controls.Add(lbl);
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
                            this.plh.Controls.Add(rdbl);
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
                            this.plh.Controls.Add(ckbl);
                            break;
                        case 3:
                            TextBox txb = new TextBox();
                            txb.ID = $"Mondai{i}";
                            txb.TextMode = TextBoxMode.MultiLine;
                            this.plh.Controls.Add(txb);
                            break;
                        case 4:
                            TextBox txbsuzi = new TextBox();
                            txbsuzi.ID = $"Mondai{i}";
                            txbsuzi.TextMode = TextBoxMode.Number;
                            this.plh.Controls.Add(txbsuzi);
                            break;
                        case 5:
                            TextBox txbemail = new TextBox();
                            txbemail.ID = $"Mondai{i}";
                            txbemail.TextMode = TextBoxMode.Email;
                            this.plh.Controls.Add(txbemail);
                            break;
                        case 6:
                            TextBox txbdate = new TextBox();
                            txbdate.ID = $"Mondai{i}";
                            txbdate.TextMode = TextBoxMode.Date;
                            this.plh.Controls.Add(txbdate);
                            break;

                    }
                }

                // 如果是從修改頁回來，則帶入值
                Kiroku krk = (Kiroku)HttpContext.Current.Session["edit"];
                if (krk != null)
                {
                    this.txbName.Text = krk.Name;
                    this.txbPhone.Text = krk.Phone;
                    this.txbEmail.Text = krk.Email;
                    this.txbAge.Text = krk.Age.ToString();
                    for (var i = 0; i < krk.KirokuList.Count; i++)
                    {
                        switch (krk.KirokuList[i].Type)
                        {
                            case 1:
                                int check = 0;
                                RadioButtonList rdb = (RadioButtonList)this.plh.FindControl($"Mondai{i}");
                                for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                                {
                                    if (string.Compare(rdb.Items[j].Text, krk.KirokuList[i].Naiyo) == 0)
                                    {
                                        rdb.Items[j].Selected = true;
                                        check = 1;
                                    }
                                    if (check == 1)
                                        break;
                                }
                                break;
                            case 2:
                                CheckBoxList ckb = (CheckBoxList)this.plh.FindControl($"Mondai{i}");
                                for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                                {
                                    for (var k = 0; k < krk.KirokuList[i].ckbNaiyo.Count; k++)
                                    {
                                        if (string.Compare(ckb.Items[j].Text, krk.KirokuList[i].ckbNaiyo[k])                == 0)
                                        {
                                            ckb.Items[j].Selected = true;
                                        }
                                    }
                                }
                                break;
                            default:
                                TextBox txb = (TextBox)this.plh.FindControl($"Mondai{i}");
                                txb.Text = krk.KirokuList[i].Naiyo;
                                break;
                        }
                    }
                }
            }
        }

        protected void cancer_Click(object sender, EventArgs e)
        {
            Response.Redirect($"List.aspx", true);
        }

        protected void gogogo_Click(object sender, EventArgs e)
        {
            List<string> errormsg = new List<string>();
            string name = string.Empty;
            int phone = 0;
            string email = string.Empty;
            int age = 0;
            // 檢查，有一個不過就return
            if (!string.IsNullOrWhiteSpace(this.txbName.Text))
                name = this.txbName.Text;
            else
                errormsg.Add("姓名不得為空白");
            if (int.TryParse(this.txbPhone.Text, out int ph))
                phone = ph;
            else
                errormsg.Add("手機不得為空白且只能輸入數字");
            if (!string.IsNullOrWhiteSpace(this.txbName.Text))
                email = this.txbEmail.Text;
            else
                errormsg.Add("Email不得為空白且須為信箱格式");
            if (int.TryParse(this.txbAge.Text, out int a))
                age = a;
            else
                errormsg.Add("年齡不得為空白且只能輸入數字");
            this.errormsg.Text = string.Empty;
            if (errormsg.Count > 0)
            {
                foreach (var x in errormsg)
                {
                    string s = x.Replace(x, x + Environment.NewLine);
                    this.errormsg.Text += s;
                }
                return;
            }
            Kiroku krk = new Kiroku()
            {
                KirokuID = Guid.NewGuid(),
                KirokuListID = Guid.NewGuid(),
                QuestionID = this._qID,
                QuestionListID = this._qtll[0].QuestionListID,
                Name = name,
                Phone = phone.ToString(),
                Email = email,
                Age = age,
                Date = DateTime.Now,
            };
            // 取得動態控制項的值
            // 根據產生了幾個控制項 抓幾次值
            List<KirokuList> krkll = new List<KirokuList>();
            for (var i = 0; i < this._qtll.Count; i++)
            {
                KirokuList krkl = new KirokuList()
                {
                    KirokuListID = krk.KirokuListID,
                    Title = this._qtll[i].Title,
                    Type = this._qtll[i].Type,
                };
                switch (this._qtll[i].Type)
                {
                    case 1:
                        for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                        {
                            int check = 0;
                            RadioButtonList rdb = (RadioButtonList)this.plh.FindControl($"Mondai{i}");
                            for (var k = 0; k < rdb.Items.Count; k++)
                            {
                                if (rdb.Items[k].Selected == true)
                                {
                                    krkl.Naiyo = rdb.Items[k].Text;
                                    krkll.Add(krkl);
                                    check = 1;
                                    break;
                                }
                            }
                            if (check == 1)
                                break;
                        }
                        break;
                    case 2:
                        List<string> ckbl = new List<string>();
                        //for (var j = 0; j < this._qtll[i].NaiyoList.Count; j++)
                        {
                            CheckBoxList ckb = (CheckBoxList)this.plh.FindControl($"Mondai{i}");
                            for (var k = 0; k < ckb.Items.Count; k++)
                            {
                                if (ckb.Items[k].Selected == true)
                                {
                                    ckbl.Add(ckb.Items[k].Text);
                                }
                            }
                        }
                        krkl.ckbNaiyo = ckbl;
                        krkll.Add(krkl);
                        break;
                    default:
                        TextBox txb = (TextBox)this.plh.FindControl($"Mondai{i}");
                        krkl.Naiyo = txb.Text;
                        krkll.Add(krkl);
                        break;
                }
                Chart
            }
            krk.KirokuList = krkll;
            HttpContext.Current.Session["Kiroku"] = krk;

            string rsrd = $"ConfirmPage.aspx?ID={krk.QuestionID}";
            Response.Redirect(rsrd);
        }
    }
}