namespace WebApi.Core.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SplitTableSetAttribute : Attribute
    {
        public int SplitTypeSet { get; set; }

        public SplitTableSetAttribute(int splitTypeset)
        {
            SplitTypeSet = splitTypeset;
        }
    }
}
