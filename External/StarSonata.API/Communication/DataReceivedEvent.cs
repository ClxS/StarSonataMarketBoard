namespace StarSonata.API.Communication
{
    internal class DataReceivedEvent
    {
        public DataReceivedEvent(byte[] bytes)
        {
            this.Bytes = bytes;
        }

        public byte[] Bytes { get; }
    }
}
