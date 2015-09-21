using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Twitter_Event_Detection
{
    class Agent
    {
        public string name;
        public IList<Agent> links = new List<Agent>();
        public IList<int> linkWeight = new List<int>();
        public Point agentLocation = new Point();
        public Point agentNameLocation = new Point();
        public string vertices;

        public Agent parent;    // Used at Prim's Algorithm
        public int key;         // Used at Prim's Algorithm
        public bool isInQueue;

        public Agent(string name)
        {
            this.name = name;
            vertices = name;
            parent = null;
            key = Int32.MaxValue;
            isInQueue = false;
        }
    }
}
