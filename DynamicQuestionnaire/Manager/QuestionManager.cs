using DynamicQuestionnaire.Helpers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Manager
{
    public class QuestionManager
    {
        // Type 1 單選方塊
        // Type 2 複選方塊
        // Type 3 文字方塊

        public List<Question> GetQuestionList(int pageSize, int pageIndex, out int totalRows)
        {
            int skip = pageSize * (pageIndex - 1); // 計算跳頁數
            if (skip < 0)
                skip = 0;
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT TOP {pageSize} * 
                    FROM Questions
                    WHERE 
                        QuestionID NOT IN 
                            ( 
                            SELECT TOP {skip} QuestionID
                            FROM Questions
                            ORDER BY DateEnd DESC
                            )
                        ORDER BY DateEnd DESC
                ";
            string commandCountText =
                $@"  SELECT COUNT(QuestionID)
                    FROM Questions
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Question> QuestionList = new List<Question>();
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Question po = this.BuildQuestionListContent(reader);
                            QuestionList.Add(po);
                        }
                        reader.Close();

                        // 取得總筆數
                        // 因為使用同一個command，不同的查詢，必須使用不同的參數集合
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        totalRows = (int)command.ExecuteScalar();
                        // command.ExecuteScalar 只會回傳一個資料 為Object
                        return QuestionList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionList", ex);
                throw;
            }
        }
        /// <summary>取問卷 </summary>
        /// <param name="hosii">標題</param>
        /// <param name="S">開始</param>
        /// <param name="E">結束</param>
        /// <param name="pageSize">幾筆為單位</param>
        /// <param name="pageIndex">頁數</param>
        /// <param name="totalRows">搜尋結果總數</param>
        /// <returns></returns>
        public List<Question> GetQuestionList(string hosii, DateTime S, DateTime E, int pageSize, int pageIndex, out int totalRows)
        {
            int skip = pageSize * (pageIndex - 1); // 計算跳頁數
            if (skip < 0)
                skip = 0;
            string Zyouken = " ";
            Zyouken += $" (QName LIKE '%'+@{hosii}+'%') AND (DateStart >= @S AND DateEnd <= @E) ";
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT TOP {pageSize} * 
                    FROM Questions
                    WHERE 
                        QuestionID NOT IN 
                            ( 
                            SELECT TOP {skip} QuestionID
                            FROM Questions
                            WHERE {Zyouken}
                            ORDER BY DateEnd DESC
                            )
                        AND {Zyouken}
                        ORDER BY DateEnd DESC
                ";
            string commandCountText =
                $@"  SELECT COUNT(QuestionID)
                    FROM Questions
                    WHERE {Zyouken}
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Question> QuestionList = new List<Question>();
                        connection.Open();

                        command.Parameters.AddWithValue($"@{hosii}", hosii);
                        command.Parameters.AddWithValue($"@S", S);
                        command.Parameters.AddWithValue($"@E", E);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Question po = this.BuildQuestionListContent(reader);
                            QuestionList.Add(po);
                        }
                        reader.Close();

                        // 取得總筆數
                        // 因為使用同一個command，不同的查詢，必須使用不同的參數集合
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;

                        command.Parameters.AddWithValue($"@{hosii}", hosii);
                        command.Parameters.AddWithValue($"@S", S);
                        command.Parameters.AddWithValue($"@E", E);
                        totalRows = (int)command.ExecuteScalar();
                        // command.ExecuteScalar 只會回傳一個資料 為Object
                        return QuestionList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionList", ex);
                throw;
            }
        }
        private Question BuildQuestionListContent(SqlDataReader reader)
        {
            return new Question()
            {
                QuestionID = (Guid)reader["QuestionID"],
                QuestionListID = (Guid)reader["QuestionListID"],
                QName = (string)reader["QName"],
                QSetume = (string)reader["QSetume"],
                DateStart = (DateTime)reader["DateStart"],
                DateEnd = (DateTime)reader["DateEnd"],
                State = (int)reader["State"],
                Zyunban = (int)reader["Zyunban"],
            };
        }
        public Question GetQuestion(Guid Questionid, out List<QuestionList> qtll)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Questions
                    WHERE QuestionID = @QuestionID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QuestionID", Questionid);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            Question qt = this.BuildQuestionListContent(reader);
                            qtll = this.GetQuestionListNaiyoList(qt.QuestionListID);
                            return qt;
                        }
                        qtll = new List<QuestionList>();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPost", ex);
                throw;
            }
        }
        public List<QuestionList> GetQuestionListNaiyoList(Guid QuestionListid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM QuestionList
                    WHERE QuestionListID = @QuestionListID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QuestionListID", QuestionListid);
                        conn.Open();
                        List<QuestionList> qtll = new List<QuestionList>();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            QuestionList qtl = new QuestionList()
                            {
                                QuestionListID = (Guid)reader["QuestionListID"],
                                Title = (string)reader["Title"],
                                Type = (int)reader["Type"],
                                NaiyoListID = reader["NaiyoListID"] as Guid?,
                            };
                            Guid nlID = Guid.Empty;
                            if (qtl.NaiyoListID != null)
                            {
                                nlID = (Guid)qtl.NaiyoListID;
                                List<NaiyoList> nll = this.GetNaiyoList(nlID);
                                qtl.NaiyoList = nll;
                            }
                            qtll.Add(qtl);
                        }
                        return qtll;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionListNaiyoList", ex);
                throw;
            }
        }
        public List<NaiyoList> GetNaiyoList(Guid NaiyoListid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT Naiyo
                    FROM NaiyoList
                    WHERE NaiyoListID = @NaiyoListid";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@NaiyoListid", NaiyoListid);
                        conn.Open();
                        List<NaiyoList> nll = new List<NaiyoList>();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            NaiyoList nl = new NaiyoList()
                            {
                                Naiyo = (string)reader["Naiyo"],
                            };
                            nll.Add(nl);
                        }
                        return nll;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetNaiyoList", ex);
                throw;
            }
        }
        public void CreateKiroku(Kiroku krk)
        {
            List<Kiroku> krkl = this.GetKiroku(krk.QuestionID);
            int Zyunban;
            if (krkl.Count > 0)
                Zyunban = krkl.Count + 1;
            else
                Zyunban = 1;
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  INSERT INTO Kirokus
                    (KirokuID, KirokuListID, QuestionID, QuestionListID, Name, Phone, Email, Age, Date, Zyunban)
                    VALUES
                    (@KirokuID, @KirokuListID, @QuestionID, @QuestionListID, @Name, @Phone, @Email, @Age, @Date, @Zyunban)
                   ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue(@"KirokuID", krk.KirokuID);
                        command.Parameters.AddWithValue(@"KirokuListID", krk.KirokuListID);
                        command.Parameters.AddWithValue(@"QuestionID", krk.QuestionID);
                        command.Parameters.AddWithValue(@"QuestionListID", krk.QuestionListID);
                        command.Parameters.AddWithValue(@"Name", krk.Name);
                        command.Parameters.AddWithValue(@"Phone", krk.Phone);
                        command.Parameters.AddWithValue(@"Email", krk.Email);
                        command.Parameters.AddWithValue(@"Age", krk.Age);
                        command.Parameters.AddWithValue(@"Date", krk.Date);
                        command.Parameters.AddWithValue(@"Zyunban", Zyunban);
                        command.ExecuteNonQuery();

                    }
                }
                for (var i = 0; i < krk.KirokuList.Count; i++)
                {
                    this.CreateKirokuList(krk.KirokuList[i]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.CreateKiroku", ex);
                throw;
            }
        }
        public void CreateKirokuList(KirokuList krkl)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  INSERT INTO KirokuList
                    (KirokuListID, Title, Type, Naiyo)
                    VALUES
                    (@KirokuListID, @Title, @Type, @Naiyo)
                   ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        switch (krkl.Type)
                        {
                            case 2:
                                for (var i = 0; i < krkl.ckbNaiyo.Count; i++)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue(@"KirokuListID", krkl.KirokuListID);
                                    command.Parameters.AddWithValue(@"Title", krkl.Title);
                                    command.Parameters.AddWithValue(@"Type", krkl.Type);
                                    command.Parameters.AddWithValue(@"Naiyo", krkl.ckbNaiyo[i]);
                                    command.ExecuteNonQuery();
                                }
                                break;
                            default:
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue(@"KirokuListID", krkl.KirokuListID);
                                command.Parameters.AddWithValue(@"Title", krkl.Title);
                                command.Parameters.AddWithValue(@"Type", krkl.Type);
                                command.Parameters.AddWithValue(@"Naiyo", krkl.Naiyo);
                                command.ExecuteNonQuery();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.CreateKirokuList", ex);
                throw;
            }
        }
        public List<Kiroku> GetKiroku(Guid qtID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM Kirokus
                    WHERE QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Kiroku> krkl = new List<Kiroku>();
                        connection.Open();
                        command.Parameters.AddWithValue("@QuestionID", qtID);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Kiroku po = this.BuildKirokuContent(reader);
                            krkl.Add(po);
                        }
                        return krkl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetKiroku", ex);
                throw;
            }
        }
        public List<Kiroku> GetKirokuWithStastic(Guid qtID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM Kirokus
                    WHERE QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Kiroku> krkl = new List<Kiroku>();
                        connection.Open();
                        command.Parameters.AddWithValue("@QuestionID", qtID);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Kiroku krk = this.BuildKirokuContent(reader);
                            // 整合KirokuList進krk
                            List<KirokuList> krkll = this.GetKirokuList(krk.KirokuListID);
                            krk.KirokuList = krkll;
                            krkl.Add(krk);
                        }
                        return krkl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetKirokuWithStastic", ex);
                throw;
            }
        }
        public List<KirokuList> GetKirokuList(Guid klID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM KirokuList
                    WHERE KirokuListID = @KirokuListID AND Type != 2
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<KirokuList> krkll = new List<KirokuList>();
                        connection.Open();
                        command.Parameters.AddWithValue("@KirokuListID", klID);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            KirokuList krkl = new KirokuList()
                            {
                                KirokuListID = (Guid)reader["KirokuListID"],
                                Title = (string)reader["Title"],
                                Type = (int)reader["Type"],
                                Naiyo = (string)reader["Naiyo"],
                            };
                            krkll.Add(krkl);
                        }
                        // 因為去除了ckb的結果 需要先搜尋有多少ckb的題目 在一個一個加進來
                        // 找目前是ckb且krklID為一樣的字串列
                        List<string> sl = this.GetKirokuListckbTitle(klID);
                        for (var i = 0; i < sl.Count; i++)
                        {
                            // 符合條件的新增一個krkl
                            KirokuList krkl = this.GetKirokuListckb(klID, sl[i]);
                            // 整合進上面未含Type = 2的LIST
                            krkll.Add(krkl);
                        }
                        return krkll;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetKirokuList", ex);
                throw;
            }
        }
        public List<string> GetKirokuListckbTitle(Guid klID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT DISTINCT Title 
                    FROM KirokuList
                    WHERE KirokuListID = @KirokuListID AND Type = 2
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<string> sl = new List<string>();
                        connection.Open();
                        command.Parameters.AddWithValue("@KirokuListID", klID);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string s = (string)reader["Title"];
                            sl.Add(s);
                        }
                        return sl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetKirokuListckbTitle", ex);
                throw;
            }
        }
        public KirokuList GetKirokuListckb(Guid klID,string Title)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * 
                    FROM KirokuList
                    WHERE KirokuListID = @KirokuListID AND Title = @Title
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<string> ckbl = new List<string>();
                        connection.Open();
                        command.Parameters.AddWithValue("@KirokuListID", klID);
                        command.Parameters.AddWithValue("@Title", Title);
                        SqlDataReader reader = command.ExecuteReader();
                        KirokuList krkl = new KirokuList();
                        while (reader.Read())
                        {
                            krkl.KirokuListID = (Guid)reader["KirokuListID"];
                            krkl.Title = (string)reader["Title"];
                            krkl.Type = (int)reader["Type"];
                            string ckb = (string)reader["Naiyo"];
                            ckbl.Add(ckb);
                        }
                        krkl.ckbNaiyo = ckbl;
                        return krkl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetKirokuListckb", ex);
                throw;
            }
        }
        private Kiroku BuildKirokuContent(SqlDataReader reader)
        {
            return new Kiroku()
            {
                KirokuID = (Guid)reader["KirokuID"],
                KirokuListID = (Guid)reader["KirokuListID"],
                QuestionID = (Guid)reader["QuestionID"],
                QuestionListID = (Guid)reader["QuestionListID"],
                Name = (string)reader["Name"],
                Phone = (string)reader["Phone"],
                Email = (string)reader["Email"],
                Age = (int)reader["Age"],
                Date = (DateTime)reader["Date"],
                Zyunban = (int)reader["Zyunban"],
            };
        }
    }
}