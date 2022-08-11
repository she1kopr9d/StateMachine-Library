using System.Collections.Generic;
using nonBinDll.Single;

namespace smf
{
    public class Tags
    {

        private byte _indexDTag = 0;
        private byte _indexITag = 0;
        private byte _indexOTag = 0;

        public byte _IndexDTag 
        {
            get
            {
                return _indexDTag;
            }
            set
            {
                _indexDTag = value;
                _dTags.Add("");
            } 
        }
        public byte _IndexITag
        {
            get
            {
                return _indexITag;
            }
            set
            {
                _indexITag = value;
                _iTags.Add("");
            }
        }
        public byte _IndexOTag
        {
            get
            {
                return _indexOTag;
            }
            set
            {
                _indexOTag = value;
                _oTags.Add("");
            }
        }

        public List<string> _dTags = new List<string>();
        public List<string> _iTags = new List<string>();
        public List<string> _oTags = new List<string>();

        public Tags()
        {
            _dTags.Add("");
            _iTags.Add("");
            _oTags.Add("");
        }

        public void adder(WriteState state)
        {
            switch (state)
            {
                case WriteState.dTag:
                    _IndexDTag++;
                    break;
                case WriteState.iTag:
                    _IndexITag++;
                    break;
                case WriteState.oTag:
                    _IndexOTag++;
                    break;
            }
        }
    }
}
