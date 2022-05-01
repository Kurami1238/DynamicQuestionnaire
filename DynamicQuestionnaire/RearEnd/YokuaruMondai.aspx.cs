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
    public partial class YokuaruMondai : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        List<Mondai> _mdl;
        protected void Page_Init(object sender, EventArgs e)
        {
            List<Mondai> mdl = this._qmgr.GetMondaiList();
            this._mdl = mdl;
            // 初始化開始
            if (HttpContext.Current.Session["Nowmdl"] == null)
                HttpContext.Current.Session["Nowmdl"] = mdl;
            else
                mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            this.gv.DataSource = mdl;
            this.gv.DataBind();
            // 提示使用者訊息
            if (HttpContext.Current.Session["Msg"] != null)
            {
                this.msgmsg.Value = HttpContext.Current.Session["Msg"].ToString();
                HttpContext.Current.Session["Msg"] = null;
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
            List<Mondai> mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            Mondai md = new Mondai();
            if (HttpContext.Current.Session["EditNowmd"] == null)
            {
                md = new Mondai()
                {
                    MondaiID = Guid.NewGuid(),
                    Title = this.txbquestion.Text,
                    Type = Convert.ToInt32(this.ddlType.SelectedValue),
                    Naiyo = this.txbNaiyo.Text,
                    Zettai = this.ckbhituyou.Checked == true ? 1 : 2,
                };
                mdl.Add(md);
                HttpContext.Current.Session["Nowmdl"] = mdl;
            }
            else
            {
                md = (Mondai)HttpContext.Current.Session["EditNowmd"];
                for (var i = 0; i < mdl.Count; i++)
                {
                    if (string.Compare(mdl[i].MondaiID.ToString(), md.MondaiID.ToString()) == 0)
                    {
                        mdl[i].Title = this.txbquestion.Text;
                        mdl[i].Naiyo = this.txbNaiyo.Text;
                        mdl[i].Zettai = this.ckbhituyou.Checked == true ? 1 : 2;
                        mdl[i].Type = Convert.ToInt32(this.ddlType.SelectedValue);
                    }
                }
                HttpContext.Current.Session["Nowmdl"] = mdl;
                HttpContext.Current.Session["EditNowmd"] = null;
            }
            this.gv.DataSource = mdl;
            this.gv.DataBind();
        }
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<Mondai> mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            switch (e.CommandName)
            {
                case "btnEdit":
                    for (var i = 0; i < mdl.Count; i++)
                    {
                        if (string.Compare(mdl[i].MondaiID.ToString(), e.CommandArgument.ToString()) == 0)
                        {
                            this.txbquestion.Text = mdl[i].Title;
                            this.txbNaiyo.Text = mdl[i].Naiyo;
                            this.ddlType.SelectedValue = mdl[i].Type.ToString();
                            this.ckbhituyou.Checked = mdl[i].Zettai == 1 ? true : false;
                            // 加入編輯QTLL的session 讓 qtl的加入辨認是否要覆蓋qtl
                            HttpContext.Current.Session["EditNowmd"] = mdl[i];
                        }
                    }
                    break;
            }
        }
        protected void btnDeleteMondai_Click(object sender, ImageClickEventArgs e)
        {
            List<Mondai> mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            List<string> list = new List<string>();
            for (var i = 0; i < this.gv.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gv.Rows[i].FindControl($"ckb");
                if (ckb.Checked == true)
                {
                    Label lbl = (Label)this.gv.Rows[i].FindControl("lblTitle");
                    string s = lbl.Text;
                    list.Add(s);
                }
            }
            if (list.Count > 0)
            {
                // 刪除清單內的問題
                for (var i = 0; i < list.Count; i++)
                {
                    for (var j = 0; j < mdl.Count; j++)
                    {
                        if (string.Compare(list[i], mdl[j].Title.ToString()) == 0)
                        {
                            mdl.Remove(mdl[j]);
                        }
                    }
                }
            }
            HttpContext.Current.Session["Nowmdl"] = mdl;
            this.gv.DataSource = mdl;
            this.gv.DataBind();
        }

        protected void btnCancerMondai_Click(object sender, EventArgs e)
        {
            List<Mondai> mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            mdl = this._mdl;
            HttpContext.Current.Session["Nowmdl"] = mdl;
            this.gv.DataSource = mdl;
            this.gv.DataBind();
        }

        protected void btnMondaigogogo_Click(object sender, EventArgs e)
        {
            List<Mondai> mdl = (List<Mondai>)HttpContext.Current.Session["Nowmdl"];
            this._qmgr.CreateMondaiList(mdl);
            HttpContext.Current.Session["Msg"] = "新增常用問題成功";
            Response.Redirect("YokuaruMondai.aspx", true);
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlType.SelectedValue != "1")
                if (this.ddlType.SelectedValue != "2")
                    this.ph.Visible = false;
            if (this.ddlType.SelectedValue == "1")
                this.ph.Visible = true;
            if (this.ddlType.SelectedValue == "2")
                this.ph.Visible = true;
        }
    }
}