using Microsoft.ML.Data;

namespace E_Commerce_VS.Models.Database.Entidades
{
    public class ModelOutput
    {
        [ColumnName(@"text")]
        public string Text { get; set; }

        [ColumnName(@"label")]
        public uint Label { get; set; }

        [ColumnName(@"PredictedLabel")]
        public float PredictedLabel { get; set; }

        [ColumnName(@"Score")]
        public float[] Score {  get; set; }
    }
}
