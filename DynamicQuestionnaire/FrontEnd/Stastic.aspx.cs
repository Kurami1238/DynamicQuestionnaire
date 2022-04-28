using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.FrontEnd
{
    public partial class Stastic : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private Guid _qID;
        private List<QuestionList> _qtll;
        protected void Page_Init(object sender, EventArgs e)
        {
            string qIDtext = this.Request.QueryString["ID"];
            Guid qID = Guid.Empty;
            Question qt = new Question();
            List<Kiroku> krkl = new List<Kiroku>();
            // 初始化開始
            if (Guid.TryParse(qIDtext, out qID))
            {
                krkl = this._qmgr.GetKirokuWithStastic(qID);
                qt = this._qmgr.GetQuestion(qID, out List<QuestionList> qtll);
                this._qID = qID;
                this._qtll = qtll;
                this.QID.Value = qID.ToString();
            }
            this.ltlQName.Text = $"{qt.QName}問卷 統計結果";
            // 確定生成幾個統計圖
            // 統計問題結果數字
            List<QuestionListKazuandKiroku> qtlKandKl = new List<QuestionListKazuandKiroku>();
            if (_qtll.Count >= 0)
            {
                for (var i = 0; i < _qtll.Count; i++)
                {
                    QuestionListKazuandKiroku qtlKandK = new QuestionListKazuandKiroku()
                    {
                        Title = _qtll[i].Title,
                        Type = _qtll[i].Type,
                        KazuandKiroku = new List<KazuandKiroku>(),
                    };
                    if (_qtll[i].NaiyoList != null)
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

            }
            else
                this.ltlmsg.Text = "目前尚無資料";
            // 生成統計圖

            //for (var i = 0; i < _qtll.Count; i++)
            //{
            //    string Namae = $"sr{i}";
            //    for (var j = 0; j < qtlKandKl[i].KazuandKiroku.Count; j++)
            //    {
            //        switch (_qtll[i].Type)
            //        {
            //            case 1:
            //                Series sr = new Series();
            //                DataTable dt = new DataTable();
            //                dt.Clear();
            //                dt.Columns.Add("Naiyo");
            //                dt.Columns.Add("Kazu");
            //                foreach (var item in qtlKandKl[i].KazuandKiroku)
            //                {
            //                    DataRow dr = dt.NewRow();
            //                    dr["Naiyo"] = item.Naiyo;
            //                    dr["Kazu"] = item.Kazu;
            //                    dt.Rows.Add(dr);
            //                }
            //                sr.Points.DataBind(
            //                    dt.DefaultView,
            //                    "Naiyo","Kazu","LegendText=Naiyo,YValues=Kazu,ToolTip=Kazu");
            //                sr.Name = Namae;
            //                sr.ChartType = SeriesChartType.Pie;
            //                //sr.Points.DataBind(qtlKandKl[i].KazuandKiroku, $"{qtlKandKl[i].KazuandKiroku[j].Naiyo}", $"{qtlKandKl[i].KazuandKiroku[j].Kazu}", $"LegendText={qtlKandKl[i].KazuandKiroku[j].Naiyo},YValues={qtlKandKl[i].KazuandKiroku[j].Kazu},ToolTip={qtlKandKl[i].KazuandKiroku[j].Kazu}");
            //                sr.ToolTip = "#LEGENDTEXT: #VAL";
            //                Chart ct = new Chart();
            //                ct.Series.Add(sr);
            //                ChartArea cta = new ChartArea();
            //                cta.Area3DStyle.Enable3D = true;
            //                ct.ChartAreas.Add(cta);
            //                this.phl.Controls.Add(ct);
            //                break;
            //            case 2:
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //}
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx");
        }
    }
}