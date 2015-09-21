using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitter_Event_Detection;
using Microsoft.Glee.Drawing;

namespace Twitter_Event_Detection
{
    public partial class Cluster_Graph : Form
    {
        private IList<Agent> agentList = new List<Agent>();
        private IList<int> weightList = new List<int>();
        private IList<Agent> minumumSpanningList = new List<Agent>();
        private IList<Agent> primsList = new List<Agent>();
        private IList<Agent> minumumValueAgentList = new List<Agent>();


        public Cluster_Graph()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            get_node();

        }

        private int getIndexIfInAgentList(IList<Agent> list, string agentName)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name == agentName)
                    return i;
            }
            return -1;
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            //addButton.Enabled = false;
            //drawButton.Enabled = true;
            //comboBox1.Enabled = true;
            if (comboBox1.SelectedIndex == 0)
            {
                #region comment
                //                //------------------------------------------
//                gViewer.Visible = true;
//                Graph g = new Graph("incremental Clustering");

//                List<string> c = new List<string>() { "Her bro sis-in law hospital", "WHO" };
//            List<string> c1 = new List<string>() {"Andrew llc","espn, inc","facebook, inc.","google inc","gotta go wireless","Honda motor co., ltd","pc media limited","myspace inc",
//"pepsico, inc","sprint nextel corporation","tweetdeck, inc","twitter, inc","wal-mart new jersey","you tube inc"};
//            List<string> cc = new List<string>() { "Austin", "Bet Final", "Cape down", "Glasgow", "Jac", "Munich", "Newcastle", "ottawa", "paris", "fan diego", "sydney" };
//            List<string> c2 = new List<string>() { "Australia", "Costa Rica", "New Zealand", "Philippines", "poland", "United Kingdom", "United States" };

//            List<string> tr = new List<string>() {"organization" };
           
//                foreach (string str in tr)
//                {
                
//                    foreach (string record in c)
//                    {
                        
//                g.GraphAttr.NodeAttr.Padding = 3;
//                Edge edge = (Edge)g.AddEdge(str, record);
//                    }
//                }
//                    gViewer.Graph = g;
                //---------------------------------------------------
                #endregion
                drawGraph(minumumSpanningList);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                drawGraph(agentList);
            }
            //else
            //    MessageBox.Show("Lütfen çizilecek graph türünü seçin!");
            //firstAgent.Enabled = false;
            //secondAgent.Enabled = false;
            //textBox2.Enabled = false;
        }

        private void writeGraphInfo(Graphics g , IList<Agent> list)
        {
            int linkWeight = 0;
            int linkCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                linkCount += list[i].links.Count;
                for (int j = 0; j < list[i].linkWeight.Count; j++)
                {
                    linkWeight += list[i].linkWeight[j];
                    
                }
            }
          //  g.DrawString( "Total Link Count : " + (linkCount/2).ToString(), new Font("Times New Roman", 10), Brushes.Blue, new Point(350,460));
          //  g.DrawString("Total Link Weight : " + (linkWeight/2).ToString(), new Font("Times New Roman", 10), Brushes.Blue, new Point(350, 480));
            //g.DrawString("Total Node Count : " + list.Count, new Font("Times New Roman", 10), Brushes.Blue, new Point(350, 500));
        }

        private void refreshUnionOtherLinkedAgents(string oldName, string newName)
        {
            string[] splitString = oldName.Split(',');
            for (int i = 0; i < agentList.Count; i++)
            {
                if ((agentList[i].vertices.CompareTo(splitString[0]) == 0) || (agentList[i].vertices.CompareTo(splitString[1]) == 0))
                    agentList[i].vertices = newName;
            }
        }

        private void addIfNotExistInList(IList<Agent> list, string agent1Name, string agent2Name, int weight)
        {
            if (getIndexIfInAgentList(list, agent1Name) != -1)
            {
                // İlk Agent Liste içerisinde bulunuyorsa...
                if (getIndexIfInAgentList(list, agent2Name) == -1)
                {
                    // İkinci Agent Liste içerisinde bulunmuyorsa...
                    Agent temp = new Agent(agent2Name);
                    list.Add(temp);    // Listeye Eklenir.
                }
            }
            else
            {
                // İlk Agent Liste içerisinde bulunmuyorsa...
                Agent temp = new Agent(agent1Name);
                list.Add(temp);    // Listeye Eklenir.
                if (getIndexIfInAgentList(list, agent2Name) == -1)
                {
                    // İkinci Agent Liste içerisinde bulunmuyorsa...
                    temp = new Agent(agent2Name);
                    list.Add(temp);    // Listeye Eklenir.
                }
            }
            if (!list[getIndexIfInAgentList(list, agent1Name)].links.Contains(list[getIndexIfInAgentList(list, agent2Name)]))
            {
                list[getIndexIfInAgentList(list, agent1Name)].links.Add(list[getIndexIfInAgentList(list, agent2Name)]);
                list[getIndexIfInAgentList(list, agent1Name)].linkWeight.Add(weight);
            }
            if (!list[getIndexIfInAgentList(list, agent2Name)].links.Contains(list[getIndexIfInAgentList(list, agent1Name)]))
            {
                list[getIndexIfInAgentList(list, agent2Name)].links.Add(list[getIndexIfInAgentList(list, agent1Name)]);
                list[getIndexIfInAgentList(list, agent2Name)].linkWeight.Add(weight);
            }
            //list[getIndexIfInAgentList(list, agent1Name)].links.Add(list[getIndexIfInAgentList(list, agent2Name)]);
            //list[getIndexIfInAgentList(list, agent2Name)].links.Add(list[getIndexIfInAgentList(list, agent1Name)]);
            //list[getIndexIfInAgentList(list, agent1Name)].linkWeight.Add(weight);
            //list[getIndexIfInAgentList(list, agent2Name)].linkWeight.Add(weight);
        }

        private void drawGraph(IList<Agent> list)
        {
            Point referancePoint = new Point(600, 280);
            Graphics g = this.CreateGraphics();
            g.Clear(System.Drawing.Color.White);
            writeGraphInfo(g, list);
            for (int i = 0; i < list.Count; i++)
            {
                Point realAgentLocation = new Point(referancePoint.X + list[i].agentLocation.X, referancePoint.Y + list[i].agentLocation.Y);
                Point realAgentNameLocation = new Point(referancePoint.X + list[i].agentNameLocation.X, referancePoint.Y + list[i].agentNameLocation.Y);
                g.DrawEllipse(Pens.Black, new Rectangle(realAgentLocation, new System.Drawing.Size(10, 10)));
                g.DrawString(list[i].name, new Font("Times New Roman", 10), Brushes.Blue, realAgentNameLocation);
                for (int j = 0; j < list[i].links.Count; j++)
                {
                    Point tempAgentLinkPoint = new Point(referancePoint.X + list[i].links[j].agentLocation.X, referancePoint.Y + list[i].links[j].agentLocation.Y);
                    g.DrawLine(Pens.Red, realAgentLocation, tempAgentLinkPoint);
                    Point p = new Point((int)(((tempAgentLinkPoint.X-realAgentLocation.X)/5)+realAgentLocation.X),(int)(((realAgentLocation.Y - tempAgentLinkPoint.Y)/5))*4+tempAgentLinkPoint.Y);
                    g.DrawString(list[i].linkWeight[j].ToString(), new Font("Times New Roman", 10), Brushes.Blue, p);
                }
            }
            //var th = 1.0f;
            //var picLander = new PointF(200, 600);


            //Graphics g1 = this.CreateGraphics();
            //g1.PageUnit = GraphicsUnit.Inch;
            //Pen MyPen = new Pen(Color.Black, th / g1.DpiX);
            //g1.DrawLine(MyPen, Convert.ToInt32(picLander.X), Convert.ToInt32(picLander.Y), 1, 1);
            //g1.DrawEllipse(MyPen, Convert.ToInt32(picLander.X), Convert.ToInt32(picLander.Y), 3, 5);
    
           // g.DrawEllipse(Pens.Black, new Rectangle(, new Size(10, 10)));
        }

        private void setAgentPoints(IList<Agent> list)
        {
            double agentGraphRadius = 250;
            double agentNameGraphRadius = 270;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].agentLocation = new Point((int)(agentGraphRadius * Math.Cos(i * (2 * Math.PI) / list.Count)), (int)(agentGraphRadius * Math.Sin(i * (2 * Math.PI) / list.Count)));
                list[i].agentNameLocation = new Point((int)(agentNameGraphRadius * Math.Cos(i * (2 * Math.PI) / list.Count)), (int)(agentNameGraphRadius * Math.Sin(i * (2 * Math.PI) / list.Count)));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawButton.Enabled = true;
            if (comboBox1.SelectedIndex == 0)
            {
                // Minumum Spanning Tree çizilir.
                IEnumerable<int> sortedWeight = weightList.OrderBy(f => f);
                weightList = sortedWeight.ToList();     // Sorting Edges Weight

                
                for (int i = 0; i < weightList.Count; i++)
                {
                    int tempWeight = weightList[i];
                    for (int j = 0; j < agentList.Count; j++)
                    {
                        Agent tempAgent = agentList[j];
                        for (int k = 0; k < tempAgent.linkWeight.Count; k++)
                        {
                            int tempAgentWeight = tempAgent.linkWeight[k];
                            Agent tempAgentLink = tempAgent.links[k];
                            if ((tempAgentWeight == tempWeight) && (tempAgent.vertices.CompareTo(tempAgentLink.vertices) != 0))
                          // if ((tempAgentWeight == tempWeight))
                            {
                                // Burada bulunulan yol alınabilir. Kümeleri farklıdır. Circle oluşturmaz.
                                string otherLinkedGroupName = tempAgent.vertices + "," + tempAgentLink.vertices;
                                tempAgent.vertices += tempAgentLink.vertices;
                                tempAgentLink.vertices = tempAgent.vertices;
                                refreshUnionOtherLinkedAgents(otherLinkedGroupName, tempAgent.vertices);
                                addIfNotExistInList(minumumSpanningList, tempAgent.name, tempAgentLink.name, tempAgentWeight);
                            }
                        }
                    }
                }
                setAgentPoints(minumumSpanningList);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                // Orjinal Graph çizilir.
                setAgentPoints(agentList);
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                //string root;
                //int rootIndex = -1;
                //chooseRootForm n = new chooseRootForm(getAllAgentName(agentList));
                //n.ShowDialog();
                //root = n.choiceRoot;
                //n.Close();
                //rootIndex = getIndexIfInAgentList(agentList, root);

                //agentList[rootIndex].key = 0;
                //minumumValueAgentList.Add(agentList[rootIndex]);
                //agentList[rootIndex].isInQueue = true;

                //while (minumumValueAgentList.Count != 0)
                //{
                //    string currentAgentName = findMinumumKeyAgentIndex(minumumValueAgentList);
                //    int index = getIndexIfInAgentList(agentList,currentAgentName);
                //    Agent currentAgent = agentList[index];
                //    for (int i = 0; i < currentAgent.links.Count; i++)
                //    {
                //        if (currentAgent.links[i].isInQueue == false && currentAgent.links[i].key > currentAgent.linkWeight[i])
                //        {
                //            currentAgent.links[i].key = currentAgent.linkWeight[i];
                //            currentAgent.links[i].parent = currentAgent;
                //            if ( getIndexIfInAgentList(minumumValueAgentList,currentAgent.links[i].name) == -1 )
                //                minumumValueAgentList.Add(currentAgent.links[i]);
                //        }
                //    }
                //    minumumValueAgentList.RemoveAt(getIndexIfInAgentList(minumumValueAgentList, currentAgentName));
                //    agentList[index].isInQueue = true;
                //}

                //for (int i = 0; i < agentList.Count; i++)
                //{
                //    if (agentList[i].parent == null)
                //    {
                //        addIfNotExistInList(primsList, agentList[i].name, findChildAgent(agentList[i].name).name, findChildAgent(agentList[i].name).key);
                //    }
                //    else
                //    {
                //        addIfNotExistInList(primsList, agentList[i].name, agentList[i].parent.name, agentList[i].key);
                //    }
                //}
                //setAgentPoints(primsList);
                //drawGraph(primsList);
            }
        }

        private void drawGraphForPrim(IList<Agent> list)
        {
            Point referancePoint = new Point(750, 300);
            Graphics g = this.CreateGraphics();
            g.Clear(System.Drawing.Color.White);
            writeGraphInfo(g, list);
            for (int i = 0; i < list.Count; i++)
            {
                Point realAgentLocation = new Point(referancePoint.X + list[i].agentLocation.X, referancePoint.Y + list[i].agentLocation.Y);
                Point realAgentNameLocation = new Point(referancePoint.X + list[i].agentNameLocation.X, referancePoint.Y + list[i].agentNameLocation.Y);
                g.DrawEllipse(Pens.Black, new Rectangle(realAgentLocation, new System.Drawing.Size(10, 10)));
                g.DrawString(list[i].name, new Font("Times New Roman", 10), Brushes.Blue, realAgentNameLocation);

                if (list[i].parent != null)
                {
                    Point tempAgentLinkPoint = new Point(referancePoint.X + list[i].parent.agentLocation.X, referancePoint.Y + list[i].parent.agentLocation.Y);
                    g.DrawLine(Pens.Red, realAgentLocation, tempAgentLinkPoint);
                    Point p = new Point((int)(((tempAgentLinkPoint.X - realAgentLocation.X) / 5) + realAgentLocation.X), (int)(((realAgentLocation.Y - tempAgentLinkPoint.Y) / 5)) * 4 + tempAgentLinkPoint.Y);
                    g.DrawString(findLinkWeight(list[i].name,list[i].parent.name).ToString(), new Font("Times New Roman", 10), Brushes.Blue, p);
                }
            }
        }

        private Agent findChildAgent(string p)
        {
            for (int i = 0; i < agentList.Count; i++)
            {
                if (agentList[i].parent == null)
                    continue;
                if (agentList[i].parent.name == p)
                    return agentList[i];
            }
            return agentList[getIndexIfInAgentList(agentList,p)].parent;
        }

        private int findLinkWeight(string first, string second)
        {
            int index1 = getIndexIfInAgentList(agentList, first);
            return agentList[index1].linkWeight[getIndexIfInAgentList(agentList[index1].links, second)];
        }

        private string findMinumumKeyAgentIndex(IList<Agent> list)
        {
            string name = "";
            int minKey = Int32.MaxValue;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].key < minKey)
                {
                    minKey = list[i].key;
                    name = list[i].name;
                }
            }
            return name;
        }



        //private void secondAgent_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13)
        //    {
        //        if (!secondAgent.AcceptsReturn)
        //        {
        //            addButton.PerformClick();
        //        }
        //    }
        //}

        public void get_node()
        {
            # region
            List<string> c = new List<string>() { "Her bro sis-in law hospital", "WHO" };
            List<string> c1 = new List<string>() {"Andrew llc","espn, inc","facebook, inc.","google inc","gotta go wireless","Honda motor co., ltd","pc media limited","myspace inc",
"pepsico, inc","sprint nextel corporation","tweetdeck, inc","twitter, inc","wal-mart new jersey","you tube inc"};
            List<string> cc = new List<string>() { "Austin", "Bet Final", "Cape down", "Glasgow", "Jac", "Munich", "Newcastle", "ottawa", "paris", "fan diego", "sydney" };
            List<string> c2 = new List<string>() { "Australia", "Costa Rica", "New Zealand", "Philippines", "poland", "United Kingdom", "United States" };

            List<string> c3 = new List<string>() { "pence", "thb", "usd" };
            List<string> c4 = new List<string>() {"armored car","back car sun","blue car thanks","car bit","car scooter","font site","less greasy hot food city","positive bank balance","smart car","sorry car",
"sum 1 plz give internet connection bak","tutorial cooking food","wanna paint","web design studio hk","web hosting"};

            for (int i = 0; i < cc.Count(); i++)
            {
                textBox4.Text += "city" + " " + "1" + " " + cc[i] + " = " + cc[i] + " " + "1" + " " + "city" + "\r\n";
                addIfNotExistInList(agentList, "city", cc[i], 1);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }
            for (int J = 0; J < c.Count(); J++)
            {
                textBox4.Text += "organization" + " " + "2" + " " + c[J] + " = " + c[J] + " " + "2" + " " + "organization" + "\r\n";
                addIfNotExistInList(agentList, "organization", c[J], 2);
                weightList.Add(2);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }
            textBox4.Text += "entertainment awards" + " " + "3" + " " + "CMT awards" + " = " + "CMT awards" + " " + "3" + " " + "entertainment awards" + "\r\n";
            addIfNotExistInList(agentList, "entertainment awards", "CMT awards", 3);
            weightList.Add(2);
            comboBox1.Enabled = true;
            //firstAgent.Text = "";
            //secondAgent.Text = "";
            // textBox2.Text = "";
            addButton.Enabled = false;
            // this.ActiveControl = firstAgent;

            for (int i = 0; i < c1.Count(); i++)
            {
                textBox4.Text += "company" + " " + "4" + " " + c1[i] + " = " + c1[i] + " " + "4" + " " + "company" + "\r\n";
                addIfNotExistInList(agentList, "company", c1[i], 4);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //  this.ActiveControl = firstAgent;
            }
            for (int i = 0; i < c2.Count(); i++)
            {
                textBox4.Text += "country" + " " + "5" + " " + c2[i] + " = " + c2[i] + " " + "5" + " " + "country" + "\r\n";
                addIfNotExistInList(agentList, "country", c2[i], 5);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                // this.ActiveControl = firstAgent;
            }

            for (int i = 0; i < c3.Count(); i++)
            {
                textBox4.Text += "currency" + " " + "6" + " " + c3[i] + " = " + c3[i] + " " + "6" + " " + "currency" + "\r\n";
                addIfNotExistInList(agentList, "currency", c3[i], 6);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //  this.ActiveControl = firstAgent;
            }

            for (int i = 0; i < c4.Count(); i++)
            {
                textBox4.Text += "Industry Term" + " " + "7" + " " + c4[i] + " = " + c4[i] + " " + "7" + " " + "Industry Term" + "\r\n";
                addIfNotExistInList(agentList, "Industry Term", c4[i], 7);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                // this.ActiveControl = firstAgent;
            }

            // textBox4.Text += "movie" + " " + "8" + " " + "CMT awards" + " = " + "CMT awards" + " " + "8" + " " + "movie" + "\r\n";
            addIfNotExistInList(agentList, "movie", "The little mermaid up", 8);
            addIfNotExistInList(agentList, "sport", "FA Cup", 10);
            weightList.Add(2);
            comboBox1.Enabled = true;
            //firstAgent.Text = "";
            //secondAgent.Text = "";
            // textBox2.Text = "";
            addButton.Enabled = false;
            // this.ActiveControl = firstAgent;

            List<string> tech = new List<string>() { "3G", "adam", "android", "microwave" };
            for (int i = 0; i < tech.Count(); i++)
            {
                // textBox4.Text += "Industry Term" + " " + "7" + " " + c4[i] + " = " + c4[i] + " " + "7" + " " + "Industry Term" + "\r\n";
                addIfNotExistInList(agentList, "Technology", tech[i], 9);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }
            List<string> tech1 = new List<string>() { "california, united states", "McDowell Country", "West Bank" };
            for (int i = 0; i < tech1.Count(); i++)
            {
                // textBox4.Text += "Industry Term" + " " + "7" + " " + c4[i] + " = " + c4[i] + " " + "7" + " " + "Industry Term" + "\r\n";
                addIfNotExistInList(agentList, "State", tech1[i], 10);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }

            List<string> tech2 = new List<string>(){"alexander ryback","ashley","cale eric clapton","chris brown","clacton","darling harbour","david moody","dew throwback","gary","gilbert sullivan"
,"haha","jason marz.son","joanna it","john im","kelly cartman","kevin smith","lady gaga","martin weiss","mary grace","maybel",
"michelle pfeiffer","nika","ricky dicky dickers","sam dave","says friendster","soulja boy","toby keith","tori dean really"};

            for (int i = 0; i < tech2.Count(); i++)
            {
                // textBox4.Text += "Industry Term" + " " + "7" + " " + c4[i] + " = " + c4[i] + " " + "7" + " " + "Industry Term" + "\r\n";
                addIfNotExistInList(agentList, "Person", tech2[i], 11);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }

            List<string> tech3 = new List<string>() { "air conditioner", "apple iphone 3g smartphone", "honda fit", "iphone" };
            for (int i = 0; i < tech3.Count(); i++)
            {
                // textBox4.Text += "Industry Term" + " " + "7" + " " + c4[i] + " = " + c4[i] + " " + "7" + " " + "Industry Term" + "\r\n";
                addIfNotExistInList(agentList, "Products", tech3[i], 12);
                weightList.Add(1);
                comboBox1.Enabled = true;
                //firstAgent.Text = "";
                //secondAgent.Text = "";
                // textBox2.Text = "";
                addButton.Enabled = false;
                //this.ActiveControl = firstAgent;
            }
            #endregion

        }
       
        

       

        private string[] getAllAgentName(IList<Agent> list)
        {
            string[] s = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                s[i] = list[i].name;
            }
            return s;
        }

       
    }
}
