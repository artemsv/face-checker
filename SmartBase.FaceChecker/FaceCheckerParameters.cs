namespace SmartBase.FaceChecker
{
    public class FaceCheckerParameters : FaceCapturerParameters
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int CloseTimeoutInMs { get; set; }
    }
}
