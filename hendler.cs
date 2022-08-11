using System;
using System.Text;

namespace smf
{
    public class hendler
    {
        protected string _directory;
        protected string _notdeserialize;
        protected state _main;


        public virtual void Init(string directory) 
        {
            _directory = directory;
        }

        public virtual void Init(string directory, string file)
        {
            _directory = directory;
        }


        protected string UnSerialize(int key, string oldValue)
        {
            string newValue = "";
            for (int i = 0; i < oldValue.Length; i++)
            {
                byte[] buffer = Encoding.Default.GetBytes(Convert.ToString(oldValue[i]));
                for (int j = 0; j < buffer.Length; j++)
                {
                    if (buffer[j] + key > 255)
                        buffer[j] = Convert.ToByte(buffer[j] + key - 256);
                    else if (buffer[j] + key < 0)
                        buffer[j] = Convert.ToByte(buffer[j] + key + 256);
                    else
                        buffer[j] = Convert.ToByte(buffer[j] + key);
                }
                newValue += Encoding.Default.GetString(buffer);
            }
            return newValue;
        }
    }
}
