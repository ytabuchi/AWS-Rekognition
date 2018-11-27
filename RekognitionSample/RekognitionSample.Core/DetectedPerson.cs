using System;
using System.Collections.Generic;
using System.Text;

namespace RekognitionSample.Core
{
    public class DetectedFaceDetail
    {
        public string Gender { get; set; }
        public float GenderConfidence { get; set; }
        public float HappinessConfidence { get; set; }
        public int AgeRangeHigh { get; set; }
        public int AgeRangeLow { get; set; }
    }
}
