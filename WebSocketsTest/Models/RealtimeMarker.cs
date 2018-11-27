using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models
{
    public class RealtimeMarker : IRealtimeModel
    {
        public string PrimaryKey {
            get { return Id; }
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

    }
}
