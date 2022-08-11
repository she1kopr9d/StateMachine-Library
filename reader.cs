using System.IO;
using System.Text;
using UnityEngine;

namespace smf
{
    public enum ReadState {start, header, footer, end}

    public enum WriteState { none, name, dTag, iTag, oTag}


    public class reader : hendler
    {
        private state _main;


        public override void Init(string directory)
        {
            base.Init(directory);
        }

        public state deserialize()
        {
            _notdeserialize = fileFluid();
            return objectCreation(_notdeserialize);
        }


        private state objectCreation(string value)
        {
            state main = new state();
            ReadState RState = new ReadState();
            RState = ReadState.start;
            bool isLoop = true;
            while (isLoop == true)
            {
                switch (RState)
                {
                    case ReadState.start:
                        Tags dio;
                        (main._stateName, value, dio) = searchHeader(value);
                        main._doubleTag = dio._dTags;
                        main._inputTag = dio._iTags;
                        main._outputTag = dio._oTags;
                        RState = ReadState.footer;
                        break;
                    case ReadState.header:
                        state child = new state();
                        child._parent = main;
                        main._children.Add(child);
                        main = child;
                        RState = ReadState.start;
                        break;
                    case ReadState.footer:
                        bool isEnding = true;
                        (isEnding, value) = searchFooter(value);
                        if(isEnding == true)
                        {
                            if (main._isMain == true)
                            {
                                isLoop = false;
                                break;
                            }
                            else
                            {
                                main = main._parent;
                                break;
                            }
                        }
                        RState = ReadState.header;
                        break;
                }
            }
            return main;
        }

        private (string, string, Tags) searchHeader(string value)
        {
            WriteState writeStatus = new WriteState();
            writeStatus = WriteState.none;
            string name = "";
            Tags dio = new Tags();
            bool isStart = true;
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '<':
                        writeStatus = WriteState.name;
                        break;
                    case '>':
                        return (name, value.Substring(i + 1, value.Length - (i + 1)), dio);
                    default:
                        if (value[i] == ' ' && writeStatus == WriteState.name)
                        {
                            writeStatus = WriteState.none;
                            isStart = !isStart;
                        }   
                        if(writeStatus == WriteState.none)
                        {
                            switch (value[i])
                            {
                                case 'd':
                                    writeStatus = WriteState.dTag;
                                    break;
                                case 'i':
                                    writeStatus = WriteState.iTag;
                                    break;
                                case 'o':
                                    writeStatus = WriteState.oTag;
                                    break;
                            }
                        }
                        else
                        {
                            if (writeStatus != WriteState.name)
                            {
                                if(value[i] == ',')
                                    dio.adder(writeStatus);
                                if(value[i] == '"')
                                {
                                    isStart = !isStart;
                                    if(isStart == false)
                                    {
                                        writeStatus = WriteState.none;
                                    }
                                }
                            }
                        }
                        if(value[i] != ':' && value[i] != '"' && value[i] != ' ' && value[i] != ',' && isStart == true)
                        {
                            switch (writeStatus)
                            {
                                case WriteState.name:
                                    name += value[i];
                                    break;
                                case WriteState.dTag:
                                    dio._dTags[dio._IndexDTag] += value[i];
                                    break;
                                case WriteState.iTag:
                                    dio._iTags[dio._IndexITag] += value[i];
                                    break;
                                case WriteState.oTag:
                                    dio._oTags[dio._IndexOTag] += value[i];
                                    break;
                            }
                        }  
                        break;
                }
            }
            return ("error", "error", null);
        }

        private (bool, string) searchFooter(string value)
        {
            int start = -1, len = 0;
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '<':
                        if (i != value.Length - 1)
                            if (value[i + 1] != '/')
                                return (false, value);
                        start = i;
                        break;
                    case '>':
                        return (true,
                                value.Substring(start + 2 + len,
                                value.Length - (start + 2 + len)));
                    default:
                        if (start != -1)
                            len++;
                        break;
                }
            }
            return (false, "error");
        }

        private string fileFluid()
        {
            string file = "";
            using (FileStream smf = File.OpenRead(_directory))
            {
                byte[] buffer = new byte[smf.Length];
                smf.Read(buffer, 0, buffer.Length);
                file = Encoding.Default.GetString(buffer);
            }

            file = UnSerialize(10, file);
            file = nonDesCutter(file);
            return file;
        }

        private string nonDesCutter(string oldValue)
        {
            string newValue = "";
            for(int i = 0; i < oldValue.Length; i++)
            {
                if (oldValue[i] == '.')
                    break;
                else
                    newValue += oldValue[i];
            }
            return newValue;
        }
    }
}
