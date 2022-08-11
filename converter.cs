using System.Text;
using System.IO;

namespace smf
{
    public class converter : hendler
    {
        private string _file;


        public override void Init(string directory, string file)
        {
            _file = file;
            base.Init(directory);
        }
        
        public void serialize()
        {
            using (FileStream xmlFile = File.OpenRead(_file))
            {
                byte[] buffer = new byte[xmlFile.Length];
                xmlFile.Read(buffer, 0, buffer.Length);
                base._notdeserialize = Encoding.Default.GetString(buffer);
            }
            _notdeserialize = UnSerialize(-10, _notdeserialize);
            using (BinaryWriter binFile = new BinaryWriter(File.Open(base._directory, FileMode.OpenOrCreate)))
            {
                byte[] buffer = Encoding.Default.GetBytes(base._notdeserialize);
                binFile.Write(buffer);
            }
        } 
    }
}