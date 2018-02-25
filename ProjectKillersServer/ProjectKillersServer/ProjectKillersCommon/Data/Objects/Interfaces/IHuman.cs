using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectKillersCommon.Data.Objects {
    public interface IHuman {
        int Health { get; set; }
        int MaxHealth { get; set; }
    }
}
