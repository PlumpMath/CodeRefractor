#region Uses

using System.Text;

#endregion

namespace CodeRefractor.DataNode
{
    class Cursor
    {
        public string ReadString()
        {
            var arrayLength = ReadInt();
            var result = Encoding.UTF8.GetString(ArrayData, _position, arrayLength);
            _position += arrayLength;
            return result;
        }

        public int ReadInt()
        {
            var start = ReadByte();
            var moreBytessCount = start%4;
            if (moreBytessCount == 0)
                return start/4;
            var value = (int) start;
            var pow = 1;
            for (var i = 0; i < moreBytessCount; i++)
            {
                pow *= 256;
                value += pow*ReadByte();
            }
            return value/4;
        }

        public ExiLikeEvent Next()
        {
            var current = CurrentEvent;
            _position++;
            return current;
        }

        #region Fields

        public byte[] ArrayData;
        int _position;

        #endregion

        #region Private

        byte CurrentByte => ArrayData[_position];

        ExiLikeEvent CurrentEvent => (ExiLikeEvent)CurrentByte;

        byte ReadByte()
        {
            var current = CurrentByte;
            _position++;
            return current;
        }

        #endregion
    }
}