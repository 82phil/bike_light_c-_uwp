using Microsoft.Maker.Firmata;

namespace BikeLight.Lighting
{
    class bike01
    {
        public Segment frontRight;
        public Segment frontSideRight;

        public Segment frontLeft;
        public Segment frontSideLeft;

        public Segment SideRight;
        public Segment SideLeft;

        public Segment Rear;

        public bike01(UwpFirmata Firmata)
        {
            frontRight = new Segment(Firmata, 0x08);
            frontSideRight = new Segment(Firmata, 0x09);

            frontLeft = new Segment(Firmata, 0x0A);
            frontSideLeft = new Segment(Firmata, 0x0B);

            SideRight = new Segment(Firmata, 0x0C);
            SideLeft = new Segment(Firmata, 0x0D);

            Rear = new Segment(Firmata, 0x0E);
        }
    }
}
