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
        private Question _qs 
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid qID = Guid.Empty;
            Question qt = new Question();
            string qIDtext = this.Request.QueryString["ID"];

            // 初始化開始 有帶ID進來就是編輯模式
            if (qIDtext != null)
            {
                if (Guid.TryParse(qIDtext, out qID))
                {
                    qt = this._qmgr.GetQuestion(qID, out List<QuestionList> qtll);
                    this._qID = qID;
                    this._qtll = qtll;
                    this.txbQname.Text = qt.QName;
                    this.txbQsetume.Text = qt.QSetume;
                    this.txbS.Text = qt.DateStart.ToString();
                    if (qt.DateEnd != null)
                        this.txbE.Text = qt.DateEnd.ToString();
                    this.ckbState.Checked = qt.State == 1 ? true : false;
                }
            }
            else
            {
                this.txbS.Text = DateTime.Now.ToString();
                this.ckbState.Checked = true;
            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void cancer_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx",true);
        }

        protected void gogogo_Click(object sender, EventArgs e)
        {
            List<string> errormsg = new List<string>();
            string name= "";
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
            if (!DateTime.TryParse(this.txbS.Text,out ds))
                errormsg.Add("開始時間不得為空白");
            Question qt = new Question()
            {
                QuestionID = Guid.NewGuid(),
                QName = name,
                QSetume = setume,
                DateStart = ds,
                DateEnd = Convert.ToDateTime(this.txbE.Text) != null ? Convert.ToDateTime(this.txbE.Text) : de,
            };
        }
    }
}