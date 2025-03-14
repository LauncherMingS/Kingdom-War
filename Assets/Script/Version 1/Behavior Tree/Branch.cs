using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripte.Behavior_Tree
{
    public abstract class Branch
    {
        private List<Node> childList = new List<Node>();
        public Branch(params Node[] children) 
        {
            if (children.Length == 0) return;      
        }

        public Branch OpenBranch(params Node[] children)
        {
            if (children.Length == 0)
            {
                Debug.Log("The params of OpenBranch is empty!");
                return this;
            }
            if (childList.Count != 0)
            {
                return NewBranch(children);
            }
            foreach (var child in children)
            {
                childList.Add(child);
            }
            OpenBranchGain();
            return this;
        }
        public Branch NewBranch(params Node[] children)
        {
            return ClearBranch().OpenBranch(children);
        }
        public Branch ClearBranch()
        {
            List<Node> childList = ChildList();
            childList.Clear();
            return this;
        }
        protected virtual void OpenBranchGain()
        {

        }
        public List<Node> ChildList()
        {
            return childList;
        }
    }
    
}
