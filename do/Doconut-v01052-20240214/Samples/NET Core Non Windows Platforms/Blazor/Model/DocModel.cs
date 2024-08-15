
namespace DoconutBlazor.Model
{
    public class DocModel
    {
        public bool ShowThumbs { get; set; }
        public bool AutoFocus { get; set; }
        public bool AutoPageFocus { get; set; }
        public int PageZoom { get; set; }
        public int ZoomStep { get; set; }
        public int MaxZoom { get; set; }
        public bool ShowToolTip { get; set; }
        public bool AutoLoad { get; set; }
        public bool CacheEnabled { get; set; }
        public bool LargeDoc { get; set; }
        public bool ShowHyperlinks { get; set; }

        public bool FixedZoom { get; set; }
        public int FixedZoomPercent { get; set; }
        public int FixedZoomPercentMobile { get; set; }
        public string ToolTipPageText { get; set; }
        public string BasePath { get; set; }
        public string ResPath { get; set; }
        public string FitType { get; set; }
    }
}
