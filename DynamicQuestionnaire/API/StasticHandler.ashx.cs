using DynamicQuestionnaire.Manager;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.API
{
    /// <summary>
    /// StasticHandler 的摘要描述
    /// </summary>
    public class StasticHandler : IHttpHandler
    {
        private QuestionManager _qmgr = new QuestionManager();
        private Guid _qID;
        private List<QuestionList> _qtll;
        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("Stastic", context.Request.QueryString["Action"], true) == 0)
            {
                string QIDs = context.Request.Form["QID"];
                Guid qID = Guid.Empty;
                Question qt = new Question();
                List<Kiroku> krkl = new List<Kiroku>();
                // 初始化開始
                if (Guid.TryParse(QIDs, out qID))
                {
                    krkl = this._qmgr.GetKirokuWithStastic(qID);
                    qt = this._qmgr.GetQuestion(qID, out List<QuestionList> qtll);
                    this._qID = qID;
                    this._qtll = qtll;
                }
                // 統計問題結果數字
                List<QuestionListKazuandKiroku> qtlKandKl = new List<QuestionListKazuandKiroku>();
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
                StasticList<QuestionListKazuandKiroku> stastic = new StasticList<QuestionListKazuandKiroku>()
                {
                    SourceList = qtlKandKl,
                };
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(stastic);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
                return;
            }
        }
        public class StasticList<T>
        {
            public List<T> SourceList { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}