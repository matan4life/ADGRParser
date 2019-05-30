using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADGRClusteriser
{
    class ClusteringResult
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}
