using Microsoft.Maker.Firmata;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BikeLight.Lighting
{
    public class Segment
    {
        private UwpFirmata _Firmata;
        private byte _sysExCode;

        private byte _red;
        private byte _green;
        private byte _blue;
        
        public Segment(UwpFirmata Firmata, byte sysExCode)
        {
            _Firmata = Firmata;
            _sysExCode = sysExCode;
        }

        public void segment_color(byte red, byte green, byte blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }

        public void update()
        {
            App.Firmata.sendSysex(_sysExCode, new byte[] { _red, _green, _blue }.AsBuffer());
            App.Firmata.flush();
        }
    }
}
