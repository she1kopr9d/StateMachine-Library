using System.Collections.Generic;
using nonBinDll.Single;

namespace smf
{
    public class state
    {
        public bool _isMain => _parent == null;
        public string _stateName;
        public state _parent;
        public List<state> _children;

        public List<string> _doubleTag;
        public List<string> _inputTag;
        public List<string> _outputTag;
             
        public state()
        {
            _children = new List<state>();
            _doubleTag = new List<string>();
            _inputTag = new List<string>();
            _outputTag = new List<string>();
        }
    }
}
