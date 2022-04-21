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
    public partial class ConfirmPage : System.Web.UI.Page
    {
        private Kiroku _kiroku;
        private QuestionManager _qmgr = new QuestionManager();
        private Guid _qID;
        private List<QuestionList> _qtll;
        protected void Page_Init(object sender, EventArgs e)
        {
            this._kiroku = (Kiroku)HttpContext.Current.Session["Kiroku"];
            Kiroku krk = this._kiroku;
            Guid qID = Guid.Empty;
            Question qt = new Question();
            string qIDtext = this.Request.QueryString["ID"];

            // 初始化開始
            if (Guid.TryParse(qIDtext, out qID))
            {
                qt = this._qmgr.GetQuestion(qID, out List<QuestionList> qtll);
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
            this.ltlName.Text = krk.Name;
            this.ltlPhone.Text = krk.Phone;
            this.ltlEmail.Text = krk.Email;
            this.ltlAge.Text = krk.Age.ToString();
            // 解析動態生成控制項的值
            for (var i = 0; i < krk.KirokuList.Count; i++)
            {
                Label lbl = new Label();
                lbl.Text = $"{i + 1}. {krk.KirokuList[i].Title}";
                this.plh.Controls.Add(lbl);
                
                switch (krk.KirokuList[i].Type)
                {
                    case 1:
                        Label kotae = new Label();
                        kotae.Text = $"  {krk.KirokuList[i].Naiyo}";
                        this.plh.Controls.Add(kotae);
                        break;
                    case 2:
                        for (var j = 0; j < krk.KirokuList[i].ckbNaiyo.Count; j++)
                        {
                            Label ckbkotae = new Label();
                            ckbkotae.Text = $"  {krk.KirokuList[i].ckbNaiyo[j]}";
                            this.plh.Controls.Add(ckbkotae);
                        }
                        break;
                    case 3:
                        Label txbkotae = new Label();
                        txbkotae.Text = $"  {krk.KirokuList[i].Naiyo}";
                        this.plh.Controls.Add(txbkotae);
                        break;
                }
            }
        }

        protected void edit_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["edit"] = this._kiroku;
            Response.Redirect($"Form.aspx?ID={this._qID}",true);
        }

        protected void gogo_Click(object sender, EventArgs e)
        {
            this._qmgr.CreateKiroku(this._kiroku);
            HttpContext.Current.Session["Msg"] = "送出成功";
            Response.Redirect($"List.aspx", true);

        }
    }
}