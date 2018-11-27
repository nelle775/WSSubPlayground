using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models
{
    public interface IRealtimeModel
    {
        string PrimaryKey { get; }
    }
}
