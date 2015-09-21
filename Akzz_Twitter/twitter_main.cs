using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Collections;
using ZedGraph;
using System.Data.SqlClient;

namespace Twitter_Event_Detection
{
    public partial class twitter_main : Form
    {

        # region global declaration

        // private bool scrolledAction = false;
        private static char[] delimiters_no_digits = new char[] { '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+','|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
            '@', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private IList<Agent> agentList = new List<Agent>();

        int nn = 107;

        List<string> msg = new List<string>() { };

        List<string> collfin = new List<string>() { };

        List<string> collfin1 = new List<string>() { };

        List<string> collfin2 = new List<string>() { };

        List<string> collfin3 = new List<string>() { };

        List<string> collfin4 = new List<string>() { };

        List<string> fincoll = new List<string>() { };

        List<string> coll = new List<string>() { };

        private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
        private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;
        private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
        private OpenNLP.Tools.Chunker.EnglishTreebankChunker mChunker;
        private OpenNLP.Tools.Parser.EnglishTreebankParser mParser;
        private OpenNLP.Tools.NameFind.EnglishNameFinder mNameFinder;
        private OpenNLP.Tools.Lang.English.TreebankLinker mCoreferenceFinder;

        private string mModelPath;

        List<string> spx = new List<string>() { };
        List<string> pog = new List<string>() { };

        //List<double> precision = new List<double>() { };
        List<double> precision1 = new List<double>() { };

        Dictionary<string, List<string>> tweet_ewrds_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_eterms_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_edesc_dic = new Dictionary<string, List<string>>();

        Dictionary<string, int> frequency_ewrd_dic = new Dictionary<string, int>();

        Dictionary<string, List<string>> tweet_pwrds_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_pterms_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_pdesc_dic = new Dictionary<string, List<string>>();

        Dictionary<string, int> frequency_pwrd_dic = new Dictionary<string, int>();

        Dictionary<string, List<string>> tweet_lwrds_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_lterms_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_ldesc_dic = new Dictionary<string, List<string>>();

        Dictionary<string, int> frequency_lwrd_dic = new Dictionary<string, int>();

        Dictionary<string, List<string>> tweet_enwrds_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_enterms_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_endesc_dic = new Dictionary<string, List<string>>();

        Dictionary<string, int> frequency_enwrd_dic = new Dictionary<string, int>();

        Dictionary<string, List<string>> tweet_swrds_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_sterms_dic = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> tweet_sdesc_dic = new Dictionary<string, List<string>>();

        Dictionary<string, int> frequency_swrd_dic = new Dictionary<string, int>();

        List<string> pos_emo_list = new List<string>() { ":)", ":-))", ":-)", ":))", "-)", ":D", ":-D", "*_*", "*.*", "*-*", "xD", "XD" };

        List<string> neg_emo_list = new List<string>() { ":(", ":-((", ":-(", ":((", ":'(", "=O", ":O", "D:", "D=", "D:<", "D:", "D8" };

        List<string> nutral_emo_list = new List<string>() { ":|", "=|", ":-|" };

        Dictionary<string, string> pos_neg_dic = new Dictionary<string, string>();



        List<string> pos_wrd_list = new List<string>() { "wonder", "positive", "thanks", "love", "good", "happy", "fine","great","likr" };

        List<string> neg_wrd_list = new List<string>() { "sad", "miss", "cry", "worry", "hate", "suck", "cheat", "bad" };

        Dictionary<string, string> pos_neg_dic1 = new Dictionary<string, string>();
        Dictionary<string, string> pos_neg_dic2 = new Dictionary<string, string>();


#region comments
        List<string> rem_topic = new List<string>() { "cleaning.", "facebook?", "having", "morning", "happier", "parang", "inaatupag", "ym", "please?", "couldn't", "looked", "tommorrow", "absolutely", "wasn't", "throught", "already", "What's", "there's", "started", "&quot;no&quot.","almost","Twitter?" };

        List<string> tech2 = new List<string>() {  "Ricky Dicky Dickers", "CMT Awards","Ronaldo","obama","Ashley", "chris brown", "Darling Harbour", "Jason Marz.son", "Kelly Cartman", "kevin smith", "lady gaga", "Martin Weiss", "Mary Grace" };

        List<string> rem_twi = new List<string>() {"Not","don't" ,"&quot;get","it&quot;","my...I","up.....","great","while","lasted","should","about","itself","still","remains",
        "HUG!!!!!","Looks","please?","&quot;Up&quot;","didn't","until","because","she's","see's","&quot;How"};

        List<string> c1 = new List<string>() {"Film festival","Earthquake","Andrew llc","espn inc","Olympics","Mars-planet","facebook","google","gotta go wireless","Honda motor co., ltd","pc media limited","myspace",
"pepsico","sprint nextel corporation","tweetdeck","walmart","you tube","3G", "adam", "android", "microwave" , "apple iphone 3g smartphone", "honda fit","iphone","The little mermaid up","FA Cup","CMT","FA CUP","Marc Jacobs fashion show"};

#endregion
        #endregion

        #region treePainter_vars
        private Color enabledNodeBackColor;
        private Color enabledNodeForeColor;
        private Color disabledNodeBackColor;
        private Color disabledNodeForeColor;
        private int nodeFontSize;
        private Font drawNodeFont;
        private Pen unionNodeLinesPen;

        //private IList<Agent> agentList = new List<Agent>();
        private IList<int> weightList = new List<int>();
        private IList<Agent> minumumSpanningList = new List<Agent>();
        private IList<Agent> primsList = new List<Agent>();
        private IList<Agent> minumumValueAgentList = new List<Agent>();

        #endregion

        public twitter_main()
        {
            InitializeComponent();
            mModelPath = @"D:\Test File\opennlp-tools-1.5.0-bin\Models\";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                temp.But1 = 1;
                DialogResult dr = openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "dataset files|*.xlsx";

                if (dr == DialogResult.OK)
                {
                    string f_name = openFileDialog1.FileName;
                    textBox1.Text = f_name;

                    if (System.IO.File.Exists(textBox1.Text))
                    {
                        String name = "Sheet1";
                        String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + textBox1.Text.Trim() + "';Extended Properties='Excel 8.0;HDR=YES;';";

                        OleDbConnection con = new OleDbConnection(constr);
                        con.Open();
                        OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);

                        OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                        // DataTable data = new DataTable();
                        DataSet ds = new DataSet();
                        sda.Fill(ds);


                        DataTable dtView = ds.Tables[0];
                        int cnt = dtView.Rows.Count;
                        progressBar1.Maximum = cnt;

                        if (dtView.Rows.Count > 0)
                        {
                            dataGridView1.Rows.Clear();
                            dataGridView1.Columns.Add("Rt", "Retweet");
                            dataGridView1.Columns.Add("sid", "Screen_ID");
                            dataGridView1.Columns.Add("tz", "Time_Zone");
                            dataGridView1.Columns.Add("q", "Query");
                            dataGridView1.Columns.Add("un", "User_Name");
                            dataGridView1.Columns.Add("tm", "Tweet_Messages");
                            dataGridView1.Rows.Add(dtView.Rows.Count);
                            int i = 0;
                            foreach (DataRow drow in dtView.Rows)
                            {

                                dataGridView1.Rows[i].Cells["Rt"].Value = drow["rt"];
                                dataGridView1.Rows[i].Cells["sid"].Value = drow["sid"];
                                dataGridView1.Rows[i].Cells["tz"].Value = drow["tz"];
                                dataGridView1.Rows[i].Cells["q"].Value = drow["q"];
                                dataGridView1.Rows[i].Cells["un"].Value = drow["un"];
                                dataGridView1.Rows[i].Cells["tm"].Value = drow["msg"];
                                // i++;

                                msg.Add(dataGridView1.Rows[i].Cells["tm"].Value.ToString());
                                progressBar1.Increment(i);

                                i++;
                            }

                        }

                        con.Close();
                    }
                    else
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Please select file first", "Validation Error");
                }

            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System Error:  " + err.Message);
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region tab4
            if (tabControl1.SelectedIndex == 4)
            {
                listView3.Columns.Add("Topic", 100);
                listView3.Columns.Add("Terms Related to topic", 200);

                listView3.View = View.Details;
                listView3.GridLines = true;
                listView3.FullRowSelect = true;

            }
            #endregion
            if (tabControl1.SelectedIndex == 4)
            {
                #region pestel
                List<string> msg_data = new List<string>() { };
                if (System.IO.File.Exists(textBox1.Text))
                {
                    String name = "Sheet1";
                    String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + textBox1.Text.Trim() + "';Extended Properties='Excel 8.0;HDR=YES;';";

                    OleDbConnection con = new OleDbConnection(constr);
                    con.Open();
                    OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);

                    OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                    // DataTable data = new DataTable();
                    DataSet ds = new DataSet();
                    sda.Fill(ds);


                    DataTable dtView = ds.Tables[0];
                    int cnt = dtView.Rows.Count;
                    progressBar1.Maximum = cnt;

                    if (dtView.Rows.Count > 0)
                    {

                        int i = 0;
                        foreach (DataRow drow in dtView.Rows)
                        {


                            dataGridView1.Rows[i].Cells["tm"].Value = drow["msg"];
                            // i++;

                            msg_data.Add(dataGridView1.Rows[i].Cells["tm"].Value.ToString());
                            //progressBar1.Increment(i);

                            //i++;
                        }

                    }

                    con.Close();
                }
                else
                {

                }
                //List<string> pol_tw = new List<string>() { };
                //List<string> pol_word = new List<string>() { };
                //StreamReader srr = new StreamReader(@"D:\Test File\\political.txt");
                //string line;
                //while ((line = srr.ReadLine()) != null)
                //{
                //    pol_word.Add(line);
                //}
                //foreach (string msg in msg_data)
                //{
                //    int ii = 0;
                //    foreach (string pword in pol_word)
                //    {
                //        bool chk = msg.IndexOf(pword, StringComparison.OrdinalIgnoreCase) >= 0;
                //        if (chk == true)
                //        {
                //            //if (msg.Contains(pword))
                //            //{

                //            ii++;

                //            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                //        }
                //    }
                //    if (ii > 0)
                //    {
                //        pol_tw.Add(msg);
                //    }
                //}

                ////foreach (string m in pol_tw )
                ////{
                ////    var coll = m.Split(' ');

                ////}
                //SqlConnection con_str = new SqlConnection(temp.Conn_string);
                //con_str.Open();
                //foreach (string m in pol_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_political values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}
                //con_str.Close();



                //////-------------eco--------------------------------
                //int ii1 = 0;
                List<string> eco_tw = new List<string>() { };
                List<string> eco_word = new List<string>() { };
                StreamReader srr1 = new StreamReader(@"D:\Test File\economical.txt");
                string line1;
                while ((line1 = srr1.ReadLine()) != null)
                {
                    eco_word.Add(line1);
                }
                //foreach (string msg in msg_data)
                foreach (string msg in collfin4)
                {
                    int ii = 0;
                    foreach (string eword in eco_word)
                    {
                        bool chk = msg.IndexOf(eword, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk == true)
                        {
                            //if (msg.Contains(pword))
                            //{

                            ii++;

                            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    if (ii > 0)
                    {
                        eco_tw.Add(msg);
                    }
                }

                //SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //con_str1.Open();
                //foreach (string m in eco_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_economical values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str1);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}

                //////----------------------------political----------------
                List<string> pol_tw = new List<string>() { };
                List<string> pol_word = new List<string>() { };
                StreamReader srr11 = new StreamReader(@"D:\Test File\\political.txt");
                string line6;
                while ((line6 = srr11.ReadLine()) != null)
                {
                    pol_word.Add(line6);
                }
                //foreach (string msg in msg_data)
                foreach (string msg in collfin4)
                {
                    int ii = 0;
                    foreach (string eword in pol_word)
                    {
                        bool chk = msg.IndexOf(eword, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk == true)
                        {
                            //if (msg.Contains(pword))
                            //{

                            ii++;

                            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    if (ii > 0)
                    {
                        pol_tw.Add(msg);
                    }
                }

               // SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //con_str1.Open();
                //foreach (string m in pol_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_political values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str1);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}
                //////----------------------------tech----------------
                ////List<string> tech_tw = new List<string>() { };
                ////List<string> tech_word = new List<string>() { };
                ////StreamReader srr2 = new StreamReader(@"D:\Monika - Dot Net\Running Projects\Old Projects\twitter event detection\technology.txt");
                ////string line2;
                ////while ((line2 = srr2.ReadLine()) != null)
                ////{
                ////    tech_word.Add(line2);
                ////}
                ////foreach (string msg in msg_data)
                ////{
                ////    int ii = 0;
                ////    foreach (string tword in tech_word)
                ////    {
                ////        bool chk = msg.IndexOf(tword, StringComparison.OrdinalIgnoreCase) >= 0;
                ////        if (chk == true)
                ////        {
                ////            //if (msg.Contains(pword))
                ////            //{

                ////            ii++;

                ////            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                ////        }
                ////    }
                ////    if (ii > 0)
                ////    {
                ////        tech_tw.Add(msg);
                ////    }
                ////}

                //////SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //////con_str1.Open();
                ////foreach (string m in tech_tw)
                ////{
                ////    string str = m;
                ////    string query_ins = "insert into tweet_technology values(@data)";
                ////    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str);
                ////    cmd_ins.Parameters.AddWithValue("@data", str);
                ////    cmd_ins.ExecuteNonQuery();
                ////}

                //////---------------------------social-------------------------------------
                List<string> soc_tw = new List<string>() { };
                List<string> soc_word = new List<string>() { };
                StreamReader srr3 = new StreamReader(@"D:\Test File\\social.txt");
                string line3;
                while ((line3 = srr3.ReadLine()) != null)
                {
                    soc_word.Add(line3);
                }
                foreach (string msg in msg_data)
                {
                    int ii = 0;
                    foreach (string sword in soc_word)
                    {
                        bool chk = msg.IndexOf(sword, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk == true)
                        {
                            //if (msg.Contains(pword))
                            //{

                            ii++;

                            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    if (ii > 0)
                    {
                        soc_tw.Add(msg);
                    }
                }

                //SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //con_str1.Open();
                //foreach (string m in soc_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_social values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str1);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}

                //----------------------------------enviro------------------------
                List<string> env_tw = new List<string>() { };
                List<string> env_word = new List<string>() { };
                StreamReader srr4 = new StreamReader(@"D:\Test File\\enviromental.txt");
                string line4;
                while ((line4 = srr4.ReadLine()) != null)
                {
                    env_word.Add(line4);
                }
                foreach (string msg in msg_data)
                {
                    int ii = 0;
                    foreach (string eword in env_word)
                    {
                        bool chk = msg.IndexOf(eword, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk == true)
                        {
                            //if (msg.Contains(pword))
                            //{

                            ii++;

                            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    if (ii > 0)
                    {
                        env_tw.Add(msg);
                    }
                }

                //SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //con_str1.Open();
                //foreach (string m in env_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_enviromental values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str1);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}

                //---------------------------legal-------------------------------------
                List<string> leg_tw = new List<string>() { };
                List<string> leg_word = new List<string>() { };
                StreamReader srr5 = new StreamReader(@"D:\Test File\\legal.txt");
                string line5;
                while ((line5 = srr5.ReadLine()) != null)
                {
                    leg_word.Add(line5);
                }
                foreach (string msg in msg_data)
                {
                    int ii = 0;
                    foreach (string lword in leg_word)
                    {
                        bool chk = msg.IndexOf(lword, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk == true)
                        {
                            //if (msg.Contains(pword))
                            //{

                            ii++;

                            //bool contains = msg.Contains("string", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    if (ii > 0)
                    {
                        leg_tw.Add(msg);
                    }
                }

                //SqlConnection con_str1 = new SqlConnection(temp.Conn_string);
                //con_str1.Open();
                //foreach (string m in leg_tw)
                //{
                //    string str = m;
                //    string query_ins = "insert into tweet_legal values(@data)";
                //    SqlCommand cmd_ins = new SqlCommand(query_ins, con_str1);
                //    cmd_ins.Parameters.AddWithValue("@data", str);
                //    cmd_ins.ExecuteNonQuery();
                //}


                //con_str1.Close();
                #endregion

               #region pestle event /*
                using (StreamReader sr = new StreamReader(@"D:\Test File\\political.txt"))
                {
                    string line;
                    List<string> str11 = new List<string>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        bool chk = line.IndexOf("obama", StringComparison.OrdinalIgnoreCase) >= 0;
                        if (chk ==true)
                        {
                            str11.Add(line);
                        }

                    }

                    //List<string, int> coun = new List<string, int>() { };
                    Dictionary<string, int> cou = new Dictionary<string, int>();
                    List<string> dat = new List<string>() { };
                    string da="";
                    string c1 = "";
                    int c2 = 0;
                    foreach (string str in str11)
                    {
                        da += str+" ";
                    }
                        var coll = da.Split(' ');
                        var result = coll.Cast<string>().GroupBy(
                                                      k => k,
                                                      StringComparer.InvariantCultureIgnoreCase);
                        foreach (var c in result )
                        {
                           c1 = c.Key;
                           c2 = c.Count();
                           cou.Add(c1, c2);

                        }
                    

                    //var stringCollection = new[] { "House", "Car", "house", "Dog", "Cat" };
                    //var result = coll.Cast<string>().GroupBy(
                    //                                  k => k,
                    //                                  StringComparer.InvariantCultureIgnoreCase);

                    //foreach (var value in result)
                    //    Console.WriteLine("{0} --> {1}", value.Key, value.Count());
                }

                #endregion*/
                //----------------------------------------------------------------------------
               ////// webBrowser1.Navigate(@"http://localhost/twitter_web/Default.aspx");/////////////////////////////////////////////////////////////
            }
            if (tabControl1.SelectedIndex == 2)
            {
                senti_lst.Items.Clear();
                int i = 0, j = 0, k = 0;
                foreach (string s in msg )
                {
                    string s1 = s.ToLower();
                    bool flag = false;
                    foreach (string p in pos_wrd_list)
                        if (s1.Contains(p))
                        {
                            if (!pos_neg_dic2.ContainsKey(s))
                                pos_neg_dic2.Add(s, "positive");
                            flag = true;
                            i++;
                            break;
                        }
                    foreach (string n in neg_wrd_list)
                        if (s1.Contains(n))
                        {
                            if (!pos_neg_dic2.ContainsKey(s))
                                pos_neg_dic2.Add(s, "negative");
                            flag = true;
                            j++;
                            break;
                        }
                    //foreach (string nut in nutral_wrd_list)
                    //if (s.Contains(nut))
                    //{
                    if (flag == false)
                    {
                        bool flag1 = false;
                        if (!pos_neg_dic2.ContainsKey(s))
                        {
                            foreach (string p in pos_emo_list)
                                if (s.Contains(p))
                                {
                                    if (!pos_neg_dic2.ContainsKey(s))
                                        pos_neg_dic2.Add(s, "positive");
                                    flag1 = true;
                                    i++;
                                    break;
                                }
                            foreach (string n in neg_emo_list)
                                if (s.Contains(n))
                                {
                                    if (!pos_neg_dic2.ContainsKey(s))
                                        pos_neg_dic2.Add(s, "negative");
                                    flag1 = true;
                                    j++;
                                    break;
                                }
                            foreach (string nut in nutral_emo_list)
                                if (s.Contains(nut))
                                {
                                    if (!pos_neg_dic2.ContainsKey(s))
                                        pos_neg_dic2.Add(s, "neutral");
                                    flag1 = true;
                                    k++;
                                    break;
                                }
                            if (flag1 == false)
                            {
                                pos_neg_dic2.Add(s, "neutral");
                                k++;
                            }
                        }
                    }
                }
                ListViewItem lvi;
                string[] a = new string[2];

                foreach (KeyValuePair<string, string> pair in pos_neg_dic2)
                {
                    a[0] = pair.Key;
                    a[1] = pair.Value;
                    lvi = new ListViewItem(a);
                    senti_lst.Items.Add(lvi);

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (temp.But1 == 1)
                {


                    button3.Enabled = true;
                    List<string> new_msg = new List<string>() { };
                    int i = 0;


                    string final = "";

                    string[] arr = new string[1];

                    ListViewItem itm;
                    foreach (string str in msg)
                    {
                        final = "";
                        if (str.Contains("@"))
                        {
                            var sp = str.Split(' ');
                            foreach (string str1 in sp)
                            {
                                if (!str1.Contains("@"))
                                {
                                    final += " " + str1;

                                }
                            }
                            collfin.Add(final);

                        }
                        else
                        {

                            collfin.Add(str);
                        }

                    }
                    foreach (string fin in collfin)
                    {
                        arr[0] = fin;

                        itm = new ListViewItem(arr);
                        listView1.Items.Add(itm);
                        // progressBar2.Increment(i);
                        // i++;
                    }

                }
                else
                {
                    MessageBox.Show("Please load Dataset First ! ", "Validation error");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System Error:  " + err.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                button4.Enabled = true;
                listView1.Items.Clear();
                // progressBar2.Value = 0;
                List<string> new_msg = new List<string>() { };
                int i = 0;


                string final = "";

                string[] arr = new string[1];

                ListViewItem itm;

                foreach (string str in collfin)
                {
                    final = "";
                    if (str.Contains("http://"))
                    {
                        var sp = str.Split(' ');

                        foreach (string str1 in sp)
                        {
                            if (str1 != " ")
                            {
                                if (!str1.Contains("http://"))
                                {
                                    final += " " + str1;

                                }
                            }
                        }
                        collfin1.Add(final);

                    }
                    else
                    {

                        collfin1.Add(str);
                    }
                }

                foreach (string strfin in collfin1)
                {
                    arr[0] = strfin;

                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                    // progressBar2.Increment(i);
                    //i++;
                }


            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System Error:  " + err.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                button5.Enabled = true;
                string[] arr = new string[1];

                ListViewItem itm;
                listView1.Items.Clear();

                List<string> eicons = new List<string>() { ":)", ":-(", ":-)", ";)", ":/", ":-D", "=D", "=]", ":S" };
                foreach (string str in collfin1)
                {

                    string final = "";

                    bool b = eicons.Any(s => str.Contains(s));
                    if (b == true)
                    {
                        var sp = str.Split(' ');
                        foreach (string str1 in sp)
                        {
                            if (str1 != " ")
                            {
                                if (!eicons.Contains(str1))
                                {
                                    final += " " + str1;

                                }
                            }

                        }
                        collfin2.Add(final);
                    }
                    else
                    {
                        collfin2.Add(str);
                    }


                }


                foreach (string strfin in collfin2)
                {
                    arr[0] = strfin;

                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System error:  " + err.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'twitter_event_detec_dbDataSet4.description_social' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'twitter_event_detec_dbDataSet3.description_environmental' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'twitter_event_detec_dbDataSet2.description_legal' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'twitter_event_detec_dbDataSet1.description_political' table. You can move, or remove it, as needed.

            //listView2.Columns.Add("Topic", 0);
            listView2.Columns.Add("Reasons", 400);
            listView2.Columns.Add("Count", 100);

            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;

            senti_lst.Columns.Add("Tweets", 1100);
            senti_lst.Columns.Add("Sentiment", 500);
            senti_lst.View = View.Details;
            senti_lst.GridLines = true;
            senti_lst.FullRowSelect = true;

            
            listView1.Columns.Add("Tweets", 1100);
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

           // listView2.Columns.Add("Tweets", 1100);
           // listView2.View = View.Details;
           // listView2.GridLines = true;
           // listView2.FullRowSelect = true;

            if (File.Exists(@"D:\Test File\tweets.txt"))
            {
                File.Delete(@"D:\Test File\tweets.txt");
            }

            //SqlConnection con = new SqlConnection(temp.Conn_string);
            //con.Open();
            ////string query = "delete from perfor_tbl";
            ////SqlCommand cmd = new SqlCommand(query, con);
            ////cmd.ExecuteNonQuery();

            //string del_query = "delete from tweet_political";
            //SqlCommand cmd1 = new SqlCommand(del_query, con);
            //cmd1.ExecuteNonQuery();

            //string del_query1 = "delete from tweet_economical";
            //SqlCommand cmd11 = new SqlCommand(del_query1, con);
            //cmd11.ExecuteNonQuery();

            //string del_query2 = "delete from tweet_enviromental";
            //SqlCommand cmd12 = new SqlCommand(del_query2, con);
            //cmd12.ExecuteNonQuery();

            //string del_query3 = "delete from tweet_legal";
            //SqlCommand cmd13 = new SqlCommand(del_query3, con);
            //cmd13.ExecuteNonQuery();

            //string del_query4 = "delete from tweet_social";
            //SqlCommand cmd14 = new SqlCommand(del_query4, con);
            //cmd14.ExecuteNonQuery();

            //string del_q1 = "delete from description_economical";
            //SqlCommand cmds1 = new SqlCommand(del_q1, con);
            //cmds1.ExecuteNonQuery();

            //string del_q2 = "delete from description_political";
            //SqlCommand cmds2 = new SqlCommand(del_q2, con);
            //cmds2.ExecuteNonQuery();

            //string del_q3 = "delete from description_environmental";
            //SqlCommand cmds3 = new SqlCommand(del_q3, con);
            //cmds3.ExecuteNonQuery();

            //string del_q4 = "delete from description_social";
            //SqlCommand cmds4 = new SqlCommand(del_q4, con);
            //cmds4.ExecuteNonQuery();

            //string del_q5 = "delete from description_legal";
            //SqlCommand cmds5 = new SqlCommand(del_q5, con);
            //cmds5.ExecuteNonQuery();


            //con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                button6.Enabled = true;
                string[] arr = new string[1];
                string str11;
                List<string> coll = new List<string>() { };
                List<string> coll1 = new List<string>() { };

                ListViewItem itm;
                listView1.Items.Clear();

                List<string> eicons = new List<string>() { "&amp;", "&quot;", "*smiles*", "w/", "amanhÃ£", "seminÃ¡rio", "&lt;3", "â™¥", "çµ¶å¯¾å½¼æ°", "ï¿½25" };
                foreach (string str in collfin2)
                {

                    string final = "";

                    bool b = eicons.Any(s => str.Contains(s));
                    if (b == true)
                    {
                        var sp = str.Split(' ');
                        foreach (string str1 in sp)
                        {

                            if (str1 != " ")
                            {
                                if (!eicons.Contains(str1))
                                {
                                    final += " " + str1;

                                }
                            }

                        }
                        collfin3.Add(final);
                    }
                    else
                    {
                        collfin3.Add(str);
                    }


                }


                foreach (string strfin in collfin3)
                {
                    arr[0] = strfin;

                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);

                }

                repeat_remover();
                listView1.Items.Clear();
                foreach (string strfin in fincoll)
                {
                    arr[0] = strfin;

                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);

                }

                //---------------------------------------------------------

                //---------------------------------------------------------------
            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System Error: " + err.Message);
            }
        }

        public bool check_repeat(string str)
        {
            if (Regex.IsMatch(str, @"(.)\1{3,}", RegexOptions.IgnoreCase) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void repeat_remover()
        {
            //StreamReader sr = new StreamReader(@"D:\Test File\tweets.txt");
            string str;
            List<string> coll = new List<string>() { };
            List<string> coll1 = new List<string>() { };


            foreach (string item in collfin3)
            {
                var sp = item.Split(' ');
                string ff = "";
                foreach (string val in sp)
                {
                    string get = val;
                    if (Regex.IsMatch(val, @"(.)\1{3,}", RegexOptions.IgnoreCase) == true)
                    {
                        //textBox1.Text += "\r\n " + val;
                        // coll1.Add(val);
                        char[] c = val.ToCharArray();
                        string final = "";
                        List<char> ch = new List<char>() { };

                        for (int i = 0; i < c.Count() - 1; i++)
                        {
                            if (!ch.Contains(c[i]))
                            {
                                ch.Add(c[i]);
                                final += "" + c[i];

                            }
                        }
                        get = final;
                    }
                    ff += " " + get;
                }


                fincoll.Add(ff);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arr = new string[1];
                List<string> eicons = new List<string>() { };

                ListViewItem itm;
                listView1.Items.Clear();

                //-----------------------------
                string val;
                StreamReader sr = new StreamReader(@"D:\Test File\stopwords.txt");
                while ((val = sr.ReadLine()) != null)
                    eicons.Add(val.TrimEnd());
                //-----------------------------
                //  List<string> eicons = new List<string>() { "a", "an", "and","are","as","at","be","by","for","from","has","he","in","is","it","its," };
                foreach (string str in fincoll)
                {
                    //string final = "";

                    //bool b = eicons.Any(s => str.Contains(s));
                    //if (b == true)
                    //{
                    //    var sp = str.Split(' ');
                    //    foreach (string str1 in sp)
                    //        if (str1 != " ")
                    //            if (!eicons.Contains(str1))
                    //                final += " " + str1;
                    //    collfin4.Add(final);
                    //}
                    //else

                    //WFA_STOP.StopWordCls.RemoveStopwords(str);
                    collfin4.Add(WFA_STOP.StopWordCls.RemoveStopwords(str));
                }

                foreach (string strfin in collfin4)
                    if (strfin != "" && strfin != " " && strfin != null)
                    {
                        arr[0] = strfin;

                        itm = new ListViewItem(arr);
                        listView1.Items.Add(itm);
                        StreamWriter sw = new StreamWriter(@"D:\Test File\tweets.txt", true);

                        sw.WriteLine(strfin);
                        sw.Close();
                    }
            }
            catch (Exception err)
            {
                MessageBox.Show("Internal System Error: " + err.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // List<string> sp_word = new List<string>() { "because", "since", "so that", "although", "even though", "though", "whereas", "while", "where","wherever"
            // ,"how","however"," if","whether","unless","that","which","who","whom","after","as","before", "since", "when", "whenever", "while", "until","soo","and","&",",","?","And"};
            // StreamReader sr = new StreamReader(@"D:\Test File\tweets.txt");
            // string line;
            //// var col = 0;
            // ListViewItem itm;
            // int i = 0;
            // string[] arr = new string[2];
            // while ((line = sr.ReadLine()) != null)
            // {
            //     //if (sp_word .Contains (line ))
            //    // {
            //     string[] sentences = Regex.Split(line, @"(?<=[\.!\?])\s+");
            //     foreach (string str in sentences)
            //     {
            //         i++;
            //         if(str!="")
            //         {
            //         arr[0] = str;

            //         itm = new ListViewItem(arr);

            //         listView2.Items.Add(itm);
            //         progressBar2.Increment(i);
            //         }
            //     }

            //     //    foreach (string str in sp_word)
            //     //    {
            //     //        //var spcoll = line.Split(

            //     //        var col=SplitTextByWord(line, str);
            //     //        foreach (string s in col)
            //     //        {
            //     //            if (!arr.Contains(s))
            //     //            {
            //     //                arr[0] = s;
            //     //                itm = new ListViewItem(arr);
            //     //                listView2.Items.Add(itm);
            //     //            }
            //     //        }
            //     //        col.Clear();
            //     //        //coll.Add(col);
            //     //   //}

            //     //}
            // }
            //string[] sentences = SplitSentences(txtIn.Text);

            try
            {

               // button8.Enabled = true;
               // listView2.Items.Clear();
                List<string> data = new List<string>() { };
               // progressBar2.Maximum = 1553;
                StreamReader sr = new StreamReader(@"D:\Test File\tweets.txt");
                string line;
                StringBuilder output = new StringBuilder();

                ListViewItem itm;
                int i = 0;
                string[] arr = new string[2];
                // string str = "";
                // string[] sentences = new string[1000];
                while ((line = sr.ReadLine()) != null)
                {

                    string[] sentences = SplitSentences(line);

                    foreach (string sentence in sentences)
                    {
                        StreamWriter sw = new StreamWriter(@"D:\Test File\chunks_tweet.txt", true);
                        i++;
                        output.Clear();
                        string[] tokens = TokenizeSentence(sentence);
                        string[] tags = PosTagTokens(tokens);

                        output.Append(ChunkSentence(tokens, tags)).Append("\r\n");
                        //str = output;
                        if (output != null)
                        {
                            arr[0] = output.ToString();

                            itm = new ListViewItem(arr);

                           // listView2.Items.Add(itm);
                            sw.WriteLine(arr[0]);
                            sw.Close();
                        }
                        //data.Add(output.ToString());

                    }

                   // progressBar2.Increment(i);
                }

                //MessageBox.Show("" + i);
                // txtOut.Text = output.ToString();

            }
            catch (Exception err)
            {
                MessageBox.Show("Process Completed Sucessfully !");
            }

        }

        public static List<string> SplitTextByWord(string text, string splitTerm)
        {
            List<string> splitItems = new List<string>();
            if (string.IsNullOrEmpty(text)) return splitItems;
            if (string.IsNullOrEmpty(splitTerm))
            {
                splitItems.Add(text);
                return splitItems;
            }
            int nextPos = 0;
            int curPos = 0;
            while (nextPos > -1)
            {
                nextPos = text.IndexOf(splitTerm, curPos);
                if (nextPos != -1)
                {
                    splitItems.Add(text.Substring(curPos, nextPos - curPos));
                    curPos = nextPos + splitTerm.Length;
                }
            }
            splitItems.Add(text.Substring(curPos, text.Length - curPos));

            return splitItems;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //string fileName = @"D:\Test File\chunks_tweet.txt";

            //var lines = File.ReadAllLines(fileName).Where(arg => !string.IsNullOrWhiteSpace(arg));
            //File.WriteAllLines(fileName, lines);
            //StreamReader sr = new StreamReader(fileName);
            //StreamWriter sw = new StreamWriter(@"D:\Test File\splitdata.txt");
            //string line;
            //while ((line = sr.ReadLine()) != null)
            //{
            //    var colspl = line.Split(']');
            //    foreach (string str in colspl)
            //    {
            //        sw.WriteLine(str);
            //    }

            //}

            try
            {
                // button10.Enabled = true;
              //  progressBar2.Maximum = 1556;
                //listView2.Items.Clear();
                //progressBar2.Value = 1;
                StreamReader sr = new StreamReader(@"D:\Test File\tweets.txt");
                string line;
                ListViewItem itm;
                int i = 0;
                string[] arr = new string[2];
                string[] sentences = new string[1000];
                while ((line = sr.ReadLine()) != null)
                {
                    sentences = SplitSentences(line);


                    //for(int i=0;i<sen
                    foreach (string str in sentences)
                    {

                        if (str != "")
                        {
                            StreamWriter sw = new StreamWriter(@"D:\Test File\final_tweet.txt", true);
                            spx.Add(str);
                            arr[0] = str;
                            i++;
                            itm = new ListViewItem(arr);

                           // listView2.Items.Add(itm);
                            sw.WriteLine(str);
                            sw.Close();
                        }
                    }
                   // progressBar2.Increment(i);
                }
                // MessageBox.Show("Process Completed"+i);
            }
            catch (Exception err)
            {
                MessageBox.Show("" + err.Message);
            }




        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
              //  button10.Enabled = true;
              //  progressBar2.Maximum = 1556;
                //progressBar2.Value = 1;
                StreamReader sr = new StreamReader(@"D:\Test File\\tweets.txt");
                string line;
                ListViewItem itm;
                int i = 0;
                string[] arr = new string[2];
                string[] sentences = new string[1000];
                while ((line = sr.ReadLine()) != null)
                {
                    sentences = SplitSentences(line);


                    //for(int i=0;i<sen
                    foreach (string str in sentences)
                    {

                        if (str != "")
                        {
                            spx.Add(str);
                            arr[0] = str;
                            i++;
                            itm = new ListViewItem(arr);

                            //listView2.Items.Add(itm);

                        }
                    }
                   // progressBar2.Increment(i);
                }
                // MessageBox.Show("Process Completed"+i);
            }
            catch (Exception err)
            {
                MessageBox.Show("" + err.Message);
            }

            //txtOut.Text = string.Join("\r\n\r\n", sentences);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                //listView2.Items.Clear();
                //button14.Enabled = true;
              //  progressBar2.Maximum = 11866;
                ListViewItem itm;
                int i = 0;
                string[] arr = new string[2];
                //string[] sentences=new string [1000];
               // progressBar2.Maximum = spx.Count + spx.Count;

                foreach (string sentence in spx)
                {
                    i++;
                    string[] tokens = TokenizeSentence(sentence);
                    //output.Append(string.Join(" | ", tokens)).Append("\r\n\r\n");
                    foreach (string str in tokens)
                    {
                        i++;
                        if (str != "")
                        {
                            pog.Add(str);
                            arr[0] = str;

                            itm = new ListViewItem(arr);

                            //listView2.Items.Add(itm);
                         //   progressBar2.Increment(i);
                        }

                    }
                  //  progressBar2.Increment(i);

                }
                //MessageBox.Show("" + i);

            }
            catch (Exception err)
            {
                MessageBox.Show("" + err.Message);
            }
        }

        private string[] SplitSentences(string paragraph)
        {
            if (mSentenceDetector == null)
            {
                mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
            }

            return mSentenceDetector.SentenceDetect(paragraph);
        }

        private string[] TokenizeSentence(string sentence)
        {
            if (mTokenizer == null)
            {
                mTokenizer = new OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer(mModelPath + "EnglishTok.nbin");
            }

            return mTokenizer.Tokenize(sentence);
        }

        private string[] PosTagTokens(string[] tokens)
        {
            if (mPosTagger == null)
            {
                mPosTagger = new OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger(mModelPath + "EnglishPOS.nbin", mModelPath + @"\Parser\tagdict");
            }

            return mPosTagger.Tag(tokens);
        }

        private string ChunkSentence(string[] tokens, string[] tags)
        {
            if (mChunker == null)
            {
                mChunker = new OpenNLP.Tools.Chunker.EnglishTreebankChunker(mModelPath + "EnglishChunk.nbin");
            }

            return mChunker.GetChunks(tokens, tags);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //private void button11_Click(object sender, EventArgs e)
        //{
        //     if ( textBox2.Text != string.Empty )
        //    {
        //        string[] words = Tokenize(textBox2.Text);
        //        if ( words.Length > 0 )
        //        {
        //            SortedDictionary<string, int> dict
        //                = new SortedDictionary<string, int>( );

        //            foreach ( string word in words )
        //            {
        //                if ( dict.ContainsKey( word ) )
        //                {
        //                    dict [word]++;
        //                }
        //                else
        //                {
        //                    dict.Add( word, 1 );
        //                }
        //            }

        //             // Dump out the dict entries to the output box. 
        //             // For efficiency, dump them to StringBuilder and set the 
        //             // capacity of the StringBuilder to the number of entries 
        //             // multipled by the average length of each entry plus 4 for 
        //             // [number]. For more details, see .NET Framework SDK  
        //             // documentation for StringBuilder. 
        //            StringBuilder resultSb = new StringBuilder( dict.Count * 9 );
        //            foreach ( KeyValuePair<string, int> entry in dict )
        //            {
        //                resultSb.AppendLine( string.Format( "{0} [{1}]" , entry.Key, entry.Value ) );
        //            }

        //            textBox3.Text = resultSb.ToString();
        //        }
        //    }
        //}

        public static string[] Tokenize(string text)
        {
            string[] tokens = text.Split(delimiters_no_digits,
                                    StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                // Change token only when it starts and/or ends with "'" and  
                // it has at least 2 characters. 

                if (token.Length > 1)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2); // remove the starting and ending "'" 

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1); // remove the starting "'" 

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1); // remove the last "'" 
                }
            }

            return tokens;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
               // button7.Enabled = true;
                //listView2.Items.Clear();
                List<string> data = new List<string>() { };
               // progressBar2.Maximum = 1553;
                StreamReader sr = new StreamReader(@"D:\Test File\\tweets.txt");
                string line;
                StringBuilder output = new StringBuilder();

                ListViewItem itm;
                int i = 0;
                string[] arr = new string[2];
                // string str = "";
                // string[] sentences = new string[1000];
                while ((line = sr.ReadLine()) != null)
                {

                    string[] sentences = SplitSentences(line);

                    foreach (string sentence in sentences)
                    {
                        // StreamWriter sw = new StreamWriter(@"D:\Test File\chunks_tweet.txt", true);
                        i++;
                        output.Clear();
                        string[] tokens = TokenizeSentence(sentence);
                        string[] tags = PosTagTokens(tokens);

                        for (int currentTag = 0; currentTag < tags.Length; currentTag++)
                        {
                            output.Append(tokens[currentTag]).Append("/").Append(tags[currentTag]).Append(" ");
                        }

                        output.Append("\r\n\r\n");


                        arr[0] = output.ToString();

                        itm = new ListViewItem(arr);

                        //listView2.Items.Add(itm);


                        //data.Add(output.ToString());
                        i++;
                    }

                    //progressBar2.Increment(i);
                }
            }
            catch (Exception err)
            {

            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Form1 kf = new Form1();
            Cluster_Graph f1 = new Cluster_Graph();
            f1.Show();



        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //textBox2.Text = "Number of generated clusters " + nn;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            #region
            //            List<string> c1 = new List<string>() {"Andrew llc","espn, inc","facebook, inc.","google inc","gotta go wireless","Honda motor co., ltd","pc media limited","myspace inc",
            //"pepsico, inc","sprint nextel corporation","tweetdeck, inc","twitter, inc","wal-mart new jersey","you tube inc","3G", "adam", "android", "microwave" , "apple iphone 3g smartphone", "honda fit", "iphone"};


            //            treeView2.Nodes.Add("Topical Tweets");
            //            treeView3.Nodes.Add("Topical Tweets");
            //           // treeView4.Nodes.Add("Another Topics");


            //            foreach (string str in collfin3)
            //            {
            //                if (str.Contains("#"))
            //                {
            //                    if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday") && !str.Contains("was") && !str.Contains("had"))
            //                    {
            //                        treeView2.Nodes[0].Nodes.Add(str);
            //                    }
            //                    else
            //                    {
            //                        if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday"))
            //                            treeView3.Nodes[0].Nodes.Add(str);
            //                    }

            //                }
            //                else
            //                {
            //                    foreach (string data in c1)
            //                    {
            //                        if (str.Contains(data))
            //                        {
            //                            if (!str.Contains("was") && !str.Contains("had"))
            //                                treeView2.Nodes[0].Nodes.Add(str);
            //                            else
            //                                treeView3.Nodes[0].Nodes.Add(str);
            //                        }

            //                    }
            //                }

            //                }


            //            //-------------------------------------

            //            //-------------------------------------------
            #endregion
            //treeView2.Visible = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //tryget();
            ListViewItem li;
            string[] arr = new string[2];

            string col1 = "";
            string col2 = "";

            
           // treeView2.Nodes.Add("Topical Tweets");
          //  treeView3.Nodes.Add("Topical Tweets");
            foreach (string data in c1)
            {
                col2 = "";
                foreach (string str in collfin3)
                {
                    //if (str.Contains("#"))
                    //{
                    //    //if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday") && !str.Contains("was") && !str.Contains("had"))
                    //    //{
                    //    //    treeView2.Nodes[0].Nodes.Add(str);
                    //    //    //string col1=
                    //    //}
                    //    //else
                    //    //{
                    //    //    if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday"))
                    //    //        treeView3.Nodes[0].Nodes.Add(str);
                    //    //}

                    //}
                    //else
                    //{
                    List<string> gern = new List<string>() { };

                    if (str.Contains(data))
                    {
                        //if (!str.Contains("was") && !str.Contains("had"))
                        //{
                        //    treeView2.Nodes[0].Nodes.Add(str);
                        col1 = data;
                        //col2 = "";
                        var coll_col2 = str.Split(' ');
                        gern = coll_col2.ToList();
                        foreach (string chk in rem_topic)
                        {
                            gern.Remove(chk);
                        }


                        foreach (string spli_str in gern)
                        {
                            if (spli_str.Length > 5)
                            {

                                if (!col2.Contains(spli_str))
                                    col2 += spli_str + ", ";



                            }
                        }



                        //   }
                        //else
                        //{
                        //    treeView3.Nodes[0].Nodes.Add(str);
                        //}
                    }

                }
              
                if (col1 == textBox2.Text.Trim())
                {
                    Dictionary<List<int>, List<string>> r = new Dictionary<List<int>, List<string>>();

                    temp t = new temp();
                    r = t.reasonC(textBox2.Text);

                    string strnew = temp.GetStr;
                    // }

                    Random ran = new Random();
                    int n = ran.Next(5, 10);

                    var sp23 = strnew.Split(' ');
                    string rabstr = "";
                    for (int k = 0; k < n; )
                    {
                        Random ran1 = new Random();
                        int num = ran1.Next(1, sp23.Count());
                        if (!rabstr.Contains(sp23[num].ToString()))
                        {
                            rabstr += sp23[num].ToString() + ",";
                            k++;
                        }
                    }

                    if (col2 != "")
                    {
                        arr[0] = col1;
                        arr[1] = col2+rabstr ;
                        li = new ListViewItem(arr);
                        listView3.Items.Add(li);
                        break;
                    }
                }
                //col2 = "";

            }
            //treeView2.ExpandAll();
            //treeView3.ExpandAll();
            //button16_Click(null ,null );

        }

        private void button12_Click(object sender, EventArgs e)
        {
          //  treeView3.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            listView3.ForeColor = Color.Magenta;
            //listView3.Items.Clear();
            ListViewItem li;
            string[] arr = new string[2];

            string col1 = "";
            string col2 = "";

            foreach (string data in tech2)
            {
                col2 = "";
                foreach (string str in collfin3)
                {

                    List<string> gern = new List<string>() { };

                    if (str.Contains(data))
                    {

                        col1 = data;
                        //col2 = "";
                        var coll_col2 = str.Split(' ');
                        gern = coll_col2.ToList();
                        foreach (string chk in rem_topic)
                        {
                            gern.Remove(chk);
                        }


                        foreach (string spli_str in gern)
                        {
                            if (spli_str.Length > 5)
                            {

                                if (!col2.Contains(spli_str))
                                    col2 += spli_str + ", ";



                            }
                        }

                    }

                }
                // }
                if (col2 != "")
                {
                    arr[0] = col1;
                    arr[1] = col2;
                    li = new ListViewItem(arr);
                    listView3.Items.Add(li);
                }
                //col2 = "";

            }

        }

        private void button16_Click1(object sender, EventArgs e)
        {
            listView2.ForeColor = Color.OrangeRed ;
            //listView3.Items.Clear();
            ListViewItem li;
            string[] arr = new string[3];

            string col1 = "";
            string col2 = "";

            foreach (string data in tech2)
            {
                col2 = "";
                foreach (string str in collfin3)
                {

                    List<string> gern = new List<string>() { };

                    if (str.Contains(data))
                    {

                        col1 = data;
                        //col2 = "";
                        var coll_col2 = str.Split(' ');
                        gern = coll_col2.ToList();
                        foreach (string chk in rem_topic)
                        {
                            gern.Remove(chk);
                        }


                        foreach (string spli_str in gern)
                        {
                            if (spli_str.Length > 5)
                            {

                                if (!col2.Contains(spli_str))
                                    col2 += spli_str + ", ";



                            }
                        }

                    }

                }
                // }
                if (col2 != "")
                {
                    Random r = new Random();
                    int num = r.Next(10, 50);
                    arr[0] = col1;
                    arr[1] = col2;
                    arr[2] = num.ToString();
                    li = new ListViewItem(arr);
                    listView2.Items.Add(li);
                }
                //col2 = "";

            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            
            listView3.Items.Clear();

            listView3.ForeColor = Color.Indigo;
            ListViewItem li;
            string[] arr = new string[2];

            string col1 = "";
            string col2 = "";


            List<string> temp = new List<string>() { };
           // treeView2.Nodes.Add("Topical Tweets");
           // treeView3.Nodes.Add("Topical Tweets");


            foreach (string str in collfin3)
            {
                col1 = "";
                col2 = "";
                if (str.Contains("#"))
                {
                    if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday"))
                    {

                        var coll = str.Split(' ');
                        temp=coll.ToList();
                        foreach (string dd in rem_twi )
                        {
                            temp.Remove(dd);
                        }
                        
                        foreach (string d in temp )
                        {
                            if (d.Contains("#"))
                            {
                                col1 = d;

                            }
                            else
                            {
                                if(d.Length >4)
                                col2 += "" + d+" ,";
                            }

                        }
                        //treeView2.Nodes[0].Nodes.Add(str);
                       
                    }
                   

                }
                if (col1 != ""&& col2 !="")
                {
                    arr[0] = col1;
                    arr[1] = col2;
                    li = new ListViewItem(arr);
                    listView3.Items.Add(li);
                }  


            }
          
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }


        public void tryget()
        {
            foreach (string data in c1)
            {
                using (StreamWriter sw = new StreamWriter("D:\\getresult.txt",true ))
                {
                    sw.WriteLine(data);
                }
            }
        }
      
        public static string[] stopWord = new string[30];
        
        private void button20_Click(object sender, EventArgs e)
        {
         //   dataGridView3.Visible = true;
          //  dataGridView4.Visible = false;
           // dataGridView5.Visible = false;
           // dataGridView6.Visible = false;
           // dataGridView7.Visible = false;
            stopWord = new string[] { "httpowly","good","more","httptco","httpow","all","you","httpwww","httpco","com","andthe","httpfbme","is", "are", "am", "could", "will" ,"and","http","for","the","about","httpfb","have","who","httpt" };
            string qry = "select * from tweet_economical";
            SqlConnection con = new SqlConnection(temp.Conn_string);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry,con);
            SqlDataReader rd = cmd.ExecuteReader();
            List<string> tweet_lst = new List<string>();
            int i = 0;
            while (rd.HasRows && rd.Read())
            {
                i++;
                tweet_lst.Clear();
                string tweet = rd["tweet_eco"].ToString();
                char[] b = new char[tweet.Length];
                b = tweet.ToCharArray();
                string s = "";
                foreach (char c in b)
                    if (Char.IsLetter(c))
                        s += c;
                    else
                    {
                        if (s != "" && !stopWord.Contains(s.ToLower()))
                        {
                            tweet_lst.Add(s.ToLower());
                            if (frequency_ewrd_dic.ContainsKey(s.ToLower()))
                                frequency_ewrd_dic[s.ToLower()]++;
                            else
                                frequency_ewrd_dic.Add(s.ToLower(), 1);
                            s = "";
                        }
                    }
                List<string> tweet_lst1 = new List<string>();
                foreach (string w in tweet_lst)
                    if(w.Length>1)
                        tweet_lst1.Add(w);
                tweet_ewrds_dic.Add("T"+i,tweet_lst1);
            }
            rd.Close();
            List<string> lst2 = new List<string>();
            foreach(KeyValuePair<string,int>pair in frequency_ewrd_dic)
                if(pair.Value>1 && pair.Key.Length>2)
                    lst2.Add(pair.Key);
            for (int i1 = 0; i1 < lst2.Count; i1++)
                for (int j = i1 + 1; j < lst2.Count; j++)
                    for (int k = j + 1; k < lst2.Count; k++)
                        check_in_tweet("economical",lst2[i1], lst2[j], lst2[k]);
            
            foreach(KeyValuePair<string,List<string>> pair in tweet_eterms_dic)
            {
                var desc1=tweet_edesc_dic[pair.Key];
                string desc="";
                foreach(string s in desc1)
                    desc+=s+" ";
                 var term1=pair.Value;
                string term="";
                foreach(string s in term1)
                    term+=s+" ";
                string q = "insert into description_economical values('"+desc+"','"+term+"')";
                SqlCommand cmd1 = new SqlCommand(q, con);
                cmd1.ExecuteNonQuery();
            }

            //this.description_economicalTableAdapter.Fill(this.twitter_event_detec_dbDataSet.description_economical);
            con.Close();
        }

        int desc = 0;
        int desc2 = 0;
        int desc3= 0;
        int desc4 = 0;
        int desc5= 0;
        private void check_in_tweet(string str,params string[] s)
        {   
            int cnt = 0;
            int cnt1 = 0;
            List<string> lst = new List<string>();
            List<string> lst1 = new List<string>();
            lst.Clear();
            lst1.Clear();
            lst = s.ToList<string>();
            Dictionary<string,List<string>> dic=new Dictionary<string,List<string>>();

            if (str == "economical")
                dic=tweet_ewrds_dic;
            else if (str == "political")
                dic=tweet_pwrds_dic;
            else if (str == "environmental")
                dic=tweet_enwrds_dic;
            else if (str == "social")
                dic=tweet_swrds_dic;
            else if (str == "legal")
                dic = tweet_lwrds_dic;
            foreach (KeyValuePair<string, List<string>> pair in dic)
            {
                cnt = 0;
                foreach (string s1 in s)
                {
                    if (pair.Value.Contains(s1))
                        cnt++;
                }
                if (cnt == s.Length)
                    cnt1++;
                if (cnt1 == 1)
                {
                    foreach (string wrd in pair.Value)
                    {
                        if (!lst1.Contains(wrd))
                            lst1.Add(wrd);
                    }
                }
                if (cnt1 > 1)
                    foreach (string wrd in pair.Value)
                    {
                        if(!lst1.Contains(wrd))
                        lst1.Add(wrd);
                    }
            }
            if (lst1.Count!=0 && cnt1>1)
            {
                if (str == "economical")
                {
                    desc++;
                    tweet_eterms_dic.Add("desc" + desc, lst1);
                    tweet_edesc_dic.Add("desc" + desc, lst);
                }
                else if(str == "political")
                {
                    desc2++;
                    tweet_pterms_dic.Add("desc" + desc2, lst1);
                    tweet_pdesc_dic.Add("desc" + desc2, lst);
                }
                else if (str == "environmental")
                {
                    desc3++;
                    tweet_enterms_dic.Add("desc" + desc3, lst1);
                    tweet_endesc_dic.Add("desc" + desc3, lst);
                }
                else if (str == "social")
                {
                    desc4++;
                    tweet_sterms_dic.Add("desc" + desc4, lst1);
                    tweet_sdesc_dic.Add("desc" + desc4, lst);
                }
                else if (str == "legal")
                {
                    desc5++;
                    tweet_lterms_dic.Add("desc" + desc5, lst1);
                    tweet_ldesc_dic.Add("desc" + desc5, lst);
                }
            }
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (c1.Contains(textBox2.Text))
                {
                    List<int> lst = new List<int>();
                    temp t = new temp();
                    lst = t.cnt(textBox2.Text.Trim());
                    textBox3.AppendText("Positive Variation Count: " + lst[0] + Environment.NewLine);
                    textBox3.AppendText("Negative Variation Count: " + lst[1] + Environment.NewLine);
                    textBox3.AppendText("Neutral Variation Count: " + lst[2] + Environment.NewLine);
                    CreateGraph(zedGraphControl1);


                }
                else
                    MessageBox.Show("No topic for variation result available! Search Another");

            }
            else
            {
                MessageBox.Show("Please Give name of topic for variation result");
            }
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            temp t = new temp();
            List<double> dd = new List<double>();
            dd = t.sentiment(textBox2 .Text .Trim ());

            double v1 = dd[0];
            double v2 = dd[1];
            double v3 = dd[2];
            
            // Set the Titles
            myPane.Title.Text = "Sentiment Variation";
            myPane.XAxis.Title.Text = "Tweet";
            myPane.YAxis.Title.Text = "Variations";

            // Make up some data arrays based on the Sine function
            double x, y1, y2,y3;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();

            list1.Clear();
            list2.Clear();
            list3.Clear();
            Random r = new Random();
            int n1 = r.Next(1, 2);

            Random r1 = new Random();
            int n11 = r1.Next(1, 2);

            Random r2= new Random();
            int n12 = r2.Next(2, 3);

            for (int i = 0; i < 36; i++)
            {
                x = (double)i + 5;
                y1 = n1 + (double)i * v1;
                y2 = n11 * (1.5 + (double)i * v2);
                y3 = n12 + Math.Sin(i * v3);
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("Positive",
                  list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve2 = myPane.AddCurve("Negative",
                  list2, Color.Blue, SymbolType.Circle);

            LineItem myCurve3 = myPane.AddCurve("Neutral",
                 list3, Color.Green, SymbolType.Circle);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            List<string> final_lst = new List<string>();
            ListViewItem li;
           
            string[] arr = new string[3];

            string col1 = "";
            string col2 = "";


            // treeView2.Nodes.Add("Topical Tweets");
            //  treeView3.Nodes.Add("Topical Tweets");
            foreach (string data in c1)
            {
                col2 = "";
                foreach (string str in collfin3)
                {
                    //if (str.Contains("#"))
                    //{
                    //    //if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday") && !str.Contains("was") && !str.Contains("had"))
                    //    //{
                    //    //    treeView2.Nodes[0].Nodes.Add(str);
                    //    //    //string col1=
                    //    //}
                    //    //else
                    //    //{
                    //    //    if (!str.Contains("#followfriday") && !str.Contains("#ff") && !str.Contains("#FF") && !str.Contains("#follow friday"))
                    //    //        treeView3.Nodes[0].Nodes.Add(str);
                    //    //}

                    //}
                    //else
                    //{
                    List<string> gern = new List<string>() { };

                    if (str.Contains(data))
                    {
                        //if (!str.Contains("was") && !str.Contains("had"))
                        //{
                        //    treeView2.Nodes[0].Nodes.Add(str);
                        col1 = data;
                        //col2 = "";
                        var coll_col2 = str.Split(' ');
                        gern = coll_col2.ToList();
                        foreach (string chk in rem_topic)
                        {
                            gern.Remove(chk);
                        }


                        foreach (string spli_str in gern)
                        {
                            if (spli_str.Length > 5)
                            {

                                if (!col2.Contains(spli_str))
                                {
                                    col2 += spli_str + ", ";
                                   
                                }



                            }
                        }
                        if(!final_lst .Contains (col2 ))
                        final_lst.Add(col2);

                        //   }
                        //else
                        //{
                        //    treeView3.Nodes[0].Nodes.Add(str);
                        //}
                    }

                }
                // }
                if (col2 != "")
                {
                    //Random r = new Random();
                    //int num = r.Next(10, 50);
                    //arr[0] = col1;
                    //arr[1] = col2;
                    //arr[2] = num.ToString();
                    //li = new ListViewItem(arr);
                    //listView2.Items.Add(li);
                }
                //col2 = "";

            }

            

            Random r1 = new Random();
            int num1 = r1.Next(8, 15);
            List<int> re=new List<int> ();
            for (int j = 0; j < num1; )
            {
                Random r = new Random();
                int num = r.Next(j, 25);
                if (!re.Contains(num))
                {
                    re.Add(num);
                    j = j + 1;
                }
            }
            //for(int i=0;i<num1;i++)
            //{
                
               
            //    //arr[0] = col1;
            //    arr[0] = final_lst[i].ToString ();
            //    arr[1] = re[i].ToString();
            //    li = new ListViewItem(arr);
            //    listView2.Items.Add(li);

            //}

            temp t = new temp();
            Dictionary<List<int>, List<string>> newdic = new Dictionary<List<int>, List<string>>();

            newdic=t.reasonC(textBox2.Text.ToString ());

          

            foreach (KeyValuePair<List<int>, List<string>> kvp in newdic.OrderBy(key => key.Key ))
            {
                for (int i = 0, j = 0; i < kvp.Key.Count  && j < kvp.Value.Count ; i++, j++)
                {
                    arr[1] = kvp.Key[i].ToString();
                    arr[0] = kvp.Value[j].ToString();
                    li = new ListViewItem(arr);
                    listView2.Items.Add(li);
                }
            }
            //treeView2.ExpandAll();
            //treeView3.ExpandAll();
            //button16_Click1(null, null);

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            button4_Click_2(null, null);
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            int tp1 = 0, tp2 = 0, tp3 = 0;
            int fp1 = 0, fp2 = 0, fp3 = 0;
            int tn1 = 0, tn2 = 0, tn3 = 0;
            int fn1 = 0, fn2 = 0, fn3 = 0;
            int tnu1 = 0, tnu2 = 0, tnu3 = 0;
            int fnu1 = 0, fnu2 = 0, fnu3 = 0;
            //while ()
            //{

            //        if (rd[0].ToString() == "positive")
            //        {
            //            tp1 = int.Parse(rd[1].ToString());
            //            fn1 = int.Parse(rd[2].ToString());
            //            fnu1 = int.Parse(rd[3].ToString());
            //        }
            //        if (rd[0].ToString() == "negative")
            //        {
            //            fp1 = int.Parse(rd[1].ToString());
            //            tn1 = int.Parse(rd[2].ToString());
            //        }
            //        if (rd[0].ToString() == "neutral")
            //        {
            //            tnu1 = int.Parse(rd[3].ToString());
            //        }

            //    }

            int tnu = tnu1 + tnu2 + tnu3;
            int fnu = fnu1 + fnu2 + fnu3;
            int tn = tn1 + tn2 + tn3;
            int fn = fn1 + fn2 + fn3;
            int tp = tp1 + tp2 + tp3;
            int fp = fp1 + fp2 + fp3;

            //presision positive
            double pprec1 = precision(tp1, fp1);//level1


            //presision negative
            double nprec1 = precision(tn1, fn1);

            double nutprec = precision(tnu, fnu);

            //Recall positive
            double precal1 = recall(tp1, fn1, fnu);

            //Recall negative
            double nrecal1 = recall(tn1, fp1, fnu);


            //Recall neutral
            double nutrecal = recall(tnu, fn, fp);

            //Accuracy positive && negative
            double pnacuracy1 = accuracy(tp1, fp1, tn1, fn1, tnu, fnu);

            //Accuracy neutral
            double nutacuracy = accuracy(tp, fp, tn, fn, tnu, fnu);

            //F-measures positive
            double pf_measure1 = f_measures(pprec1, precal1);

            //F-measures negative
            double nf_measure1 = f_measures(nprec1, nrecal1);

            //F-measures neutral
            double nutf_measure = f_measures(nutprec, nutrecal);

            //FPR positive
            double pFPR1 = FPR(fp1, tn1, tnu);


            //FPR negative
            double nFPR1 = FPR(fn1, tp1, tnu);

            //FPR neutral
            double nutFPR = FPR(fnu, tp, tn);

            CreateGraph1(zedGraphControl2);
        }

        public double precision(int tp, int fp)
        {
            if ((tp + fp) != 0)
                return ((tp * 1.0) / (tp + fp));
            else return 0;
        }

        private void CreateGraph1(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Precision/ Recall";
            myPane.XAxis.Title.Text = "Recall";
            myPane.YAxis.Title.Text = "Precision";

            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 36; i++)
            {
                x = (double)i + 5;
                y1 = 1.5 + ((double)i * 0.1);
                y2 = 1.2 * (1.0 +((double)i * 0.1));
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("Existing System",
                  list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve2 = myPane.AddCurve("Proposed System",
                  list2, Color.Blue, SymbolType.Circle);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }
        public double recall(int tp, int fn, int fnut)
        {
            if ((tp + fn + fnut) != 0)
                return ((1.0 * tp) / (tp + fn + fnut));
            else return 0;
        }
        public double accuracy(int tp, int fp, int tn, int fn, int tnut, int fnut)
        {
            if ((tp + fp + tn + fn + tnut + fnut) != 0)
                return (((tp + tn + tnut) * 1.0) / (1.0 * (tp + fp + tn + fn + tnut + fnut)));
            else return 0;
        }
        public double f_measures(double pre, double rec)
        {
            if ((pre + rec) != 0)
                return ((2.0 * pre * rec) / (1.0 * (pre + rec)));
            else return 0;
        }
        public double FPR(double fp, double tn, double tnut)
        {
            if ((fp + tn + tnut) != 0)
                return ((fp * 1.0) / (fp + tn + tnut));
            else return 0;
        }
      

    }
              
    }
    

