using System.Collections.Generic;
using UnityEngine;
using System;
using nonBinDll.Single;

namespace smf.Unity
{
    [Serializable]
    public class StateMachine
    {
        public state _Now => _now;

        public string _nowAddres 
        {
            private set;
            get;
        }

        private state _mainParent;
        private state _now;

        private const char _betwenSumbol = '.';
        

        public StateMachine() { }

        public StateMachine(state smf) => Init(smf);

        public void Init(state smf)
        {
            _now = smf;
            _nowAddres = _now._stateName;
            _mainParent = _now;
            Debug.Log("State Machine Create");
        }

        public bool Move(string msg)
        {
            if (msg == _nowAddres)
                return true;
            if (TagCheaker(msg) == true)
                return true;
            int newCount = sumbolCounter(msg, _betwenSumbol);
            int oldCount = sumbolCounter(_nowAddres, _betwenSumbol);
            if (newCount > oldCount)
            {
                if (childCutter(msg) == _nowAddres)
                {
                    string newStateName = parentCutter(msg);
                    bool _isChildTrue = false;
                    _now._children.ForEach(child =>
                    {
                        if (child._stateName == newStateName)
                        {
                            _now = child;
                            _nowAddres = msg;
                            _isChildTrue = true;
                        }
                    });
                    return _isChildTrue;
                }
                else
                {
                    return false;
                }
            }
            else if (newCount < oldCount)
            {
                if (childCutter(_nowAddres) == msg)
                {
                    _now = _now._parent;
                    _nowAddres = msg;
                    return true;
                }
            }
            return false;
        }


        private bool TagCheaker(string link)
        {
            state newState = searchStateFromLink(link, _mainParent);
            if (newState != null)
            {
                if (CompairsLists(newState._doubleTag, _now._doubleTag) == true || 
                    CompairsLists(newState._inputTag, _now._outputTag) == true)
                {

                    _nowAddres = link;  
                    _now = newState;
                    return true;
                }
            }
            return false;
        }

        private bool CompairsLists(List<string> values1, List<string> values2)
        {
            foreach(string value1 in values1)
            {
                foreach (string value2 in values2)
                {
                    if (value1 == "" || value2 == "")
                        continue;
                    if (value1 == value2 && value1 != null)
                        return true;
                }
            }
            return false;
        }

        private state searchStateFromLink(string link, state main)
        {
            state newState = new state();
            newState = main;
            Stack<string> stackLink = new Stack<string>();
            string mainName = "";
            (mainName, stackLink) = linkCutter(link);
            if (newState._stateName != mainName)
                return null;
            else if (newState._stateName == mainName && stackLink.Count < 1)
                return newState;
            for (int i = 0; i < stackLink.Count + 1; i++)
            {
                string stateName = stackLink.Pop();
                bool tryAcept = false;
                foreach (state child in newState._children)
                {
                    if (child._stateName == stateName)
                    {
                        newState = child;
                        tryAcept = true;
                        break;
                    }
                }
                if (tryAcept == false)
                    return null;
                else
                    tryAcept = false;
            }
            return newState;
        }

        private (string, Stack<string>) linkCutter(string link)
        {
            string cellLink = "";
            Stack<string> stackLink = new Stack<string>();
            for(int i = link.Length-1; i >= 0; i--)
            {
                if (link[i] != _betwenSumbol)
                {
                    cellLink = link[i] + cellLink;
                }
                else
                {
                    stackLink.Push(cellLink);
                    cellLink = "";
                }
            }
            return (cellLink, stackLink);
        }

        private string childCutter(string msg)
        {
            for (int i = msg.Length-1; i > 0; i--)
                if (msg[i] == _betwenSumbol)
                    return msg.Substring(0, i);
            return "error";
        }

        private string parentCutter(string msg)
        {
            string parent = childCutter(msg);
            return msg.Substring(parent.Length + 1, msg.Length - parent.Length - 1);
        }

        private int sumbolCounter(string msg, char sumbol)
        {
            int count = 0;
            for (int i = 0; i < msg.Length; i++)
                if(msg[i] == sumbol)
                    count++;
            return count;
        }
    }
}