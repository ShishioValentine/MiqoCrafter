using System.IO;

namespace MiqoCraftCore
{
    public class FFXIVAetheryte
    {
        public FFXIVPosition Position;
        public double OffsetX = 21.4;
        public double OffsetY = 21.4;

        public string Region = "Unknown";
        public string Zone = "Unknown";
        public string Name = "Unknown";
        public FileInfo BackgroundPicture = null;

        public override string ToString()
        {
            string position = "unknown";
            if (null != Position) position = Position.ToString();
            return Region + " - " + Zone + " - " + Name + " - " + position;
        }

        public string GetMapName()
        {
            return Zone + "-" + Name + ".jpg";
        }
    }
}
