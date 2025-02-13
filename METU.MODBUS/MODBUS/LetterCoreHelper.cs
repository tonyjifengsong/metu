namespace METU.MODBUS.MODBUS
{
    internal class LetterCoreHelper : CoreHelper
    {
        public override Byte[] GetBytes(short value)
        {
            return this.Reverse(BitConverter.GetBytes(value));
        }

        public override Byte[] GetBytes(int value)
        {
            return this.Reverse(BitConverter.GetBytes(value));
        }

        public override Byte[] GetBytes(float value)
        {
            return this.Reverse(BitConverter.GetBytes(value));
        }

        public override Byte[] GetBytes(double value)
        {
            return this.Reverse(BitConverter.GetBytes(value));
        }

        public override short GetShort(byte[] data)
        {
            return BitConverter.ToInt16(this.Reverse(data), 0);
        }

        public override int GetInt(byte[] data)
        {
            return BitConverter.ToInt32(this.Reverse(data), 0);
        }

        public override float GetFloat(byte[] data)
        {
            return BitConverter.ToSingle(this.Reverse(data), 0);
        }

        public override double GetDouble(byte[] data)
        {
            return BitConverter.ToDouble(this.Reverse(data), 0);
        }

        private Byte[] Reverse(Byte[] data)
        {
            Array.Reverse(data);
            return data;
        }
    }
}
