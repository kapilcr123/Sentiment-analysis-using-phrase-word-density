using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Twitter_Event_Detection
{
    class temp
    {
        private static int but1;
          Dictionary<string, List<double>> dic = new Dictionary<string, List<double>>();
        Dictionary<string, List<int>> dic1 = new Dictionary<string, List<int>>();

        Dictionary<string, List<string >> reason = new Dictionary<string, List<string>>();
        
        List<string> earth = new List<string>() { };

        private static string getStr;

        public static string GetStr
        {
            get { return temp.getStr; }
            set { temp.getStr = value; }
        }
       

      public void add()
      {
          #region declaration
          dic.Add("Earthquake", new List<double>() { 0.1, 0.4, 0.2 });
          dic.Add("Olympics", new List<double>() { 0.3, 0.1, 0.1 });
          dic.Add("Mars-planet", new List<double>() { 0.4, 0.1, 0.2 });
          dic.Add("facebook", new List<double>() { 0.1, 0.4, 0.1 });
          dic.Add("google", new List<double>() { 0.5, 0.1, 0.2 });
          dic.Add("3G", new List<double>() { 0.1, 0.4, 0.2 });
          dic.Add("android", new List<double>() { 0.3, 0.1, 0.2 });
          dic.Add("FA Cup", new List<double>() { 0.1, 0.1, 0.2 });
          dic.Add("iphone", new List<double>() { 0.1, 0.4, 0.2 });


          dic1.Add("Earthquake", new List<int>() { 6, 18, 4 });
          dic1.Add("Olympics", new List<int>() { 13, 6, 2 });
          dic1.Add("Mars-planet", new List<int>() { 14, 6, 4 });
          dic1.Add("facebook", new List<int>() { 4, 19, 2 });
          dic1.Add("google", new List<int>() { 21, 7, 6 });
          dic1.Add("3G", new List<int>() { 19, 7, 6 });
          dic1.Add("android", new List<int>() { 13, 6, 3 });
          dic1.Add("FA Cup", new List<int>() { 5, 6, 3 });
          dic1.Add("iphone", new List<int>() { 4, 17, 3 });


          #endregion
      }

      public void readf(string word)
      {

                 


      }

      public Dictionary<List<int>, List<string >> reasonC(string word)
      {
          Dictionary<List <int >, List<string>> r = new Dictionary<List<int>, List<string>>();
          List<int> cnt = new List<int>();

          using (StreamReader sr = new StreamReader("ForeG.txt"))
          {
              int i = 0;
              string line;
              while ((line = sr.ReadLine()) != null)
              {
                  i++;
                  if (word == "Earthquake")
                  {
                      if (i < 7)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                    
                          cnt.Add(int.Parse (sp[1].ToString().Trim ()));
                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }
                  else if (word == "Olympics")
                  {
                      if (i > 6 && i<11)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                      
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));


                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }
                  else if (word == "Mars-planet")
                  {
                      if (i > 10 && i < 17)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                        
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));

                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }

                  else if (word == "facebook")
                  {
                      if (i > 16 && i < 25)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                    
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));

                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }
                  else if (word == "google")
                  {
                      if (i > 24 && i < 30)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                        
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));

                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }

                  else if (word == "3G")
                  {
                      if (i > 29 && i < 34)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                 
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));

                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }

                  else if (word == "android")
                  {
                      if (i > 33 && i < 39)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                  
                          cnt.Add(int.Parse(sp[1].ToString().Trim()));

                          earth.Add(sp[0].ToString());
                          getStr += " " + sp[0];
                      }
                  }

                  else if (word == "iphone")
                  {
                      if (i > 38 && i < 53)
                      {
                          line = WFA_STOP.StopWordCls.RemoveStopwords(line);
                          string[] sp = line.Split('=');
                          if (sp.Count() >1)
                          {
                              cnt.Add(int.Parse(sp[1].ToString().Trim()));


                              earth.Add(sp[0].ToString());
                              getStr += " " + sp[0];
                          }
                      }
                  }
              }
          }
          cnt.Sort();
          cnt.Reverse();
          r.Add(cnt, earth);

          return r;
      }

        public static int But1
        {
            get { return temp.but1; }
            set { temp.but1 = value; }
        }

        private static string conn_string;

        public static string Conn_string
        {
            get { return @"Data Source=(local);Initial Catalog=twitter_db;Integrated Security=True"; }
            set { temp.conn_string = @"Data Source=(local);Initial Catalog=twitter_db;Integrated Security=True"; }
        }

        public List<double> sentiment(string word)
        {
            add();
            List<double> senti = new List<double>() { };
          

            foreach (KeyValuePair <string ,List <double >> kvp in dic)
            {
                if (kvp.Key.Contains(word))
                {
                    senti = kvp.Value;
                }
            }

            return senti;
        }

        public List<int> cnt(string word)
        {
            add();
            List<int> cnt = new List<int>();
            foreach (KeyValuePair<string, List<int>> kvp in dic1)
            {
                if (kvp.Key.Contains(word))
                {
                    cnt = kvp.Value;
                }
            }

            return cnt;
           

        }
    }
}
