using System;
using System.Collections.Generic;
using System.Text;

namespace UDPGolf
{
    public class SwingData
    {
        public double SwingSpeed { get; set; }

        public SwingData(double swingSpeed)
        {
            SwingSpeed = swingSpeed;
        }

        public SwingData()
        {

        }

        public override string ToString()
        {
            return $"{nameof(SwingSpeed)}: {SwingSpeed}";
        }
        
    }
}
