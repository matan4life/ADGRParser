using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADGRClusteriser
{
    class PlayerData
    {
        [LoadColumn(0)]
        public float Sex;

        [LoadColumn(1)]
        public float Age;

        [LoadColumn(2)]
        public float Country;

        [LoadColumn(3)]
        public float Profit;

        [LoadColumn(4)]
        public float Cheats;

        [LoadColumn(5)]
        public string UserId;
    }
}
