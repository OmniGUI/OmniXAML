namespace OmniXaml
{
    public class ConversionRequest
    {
        public ConversionRequest(Member member, object value)
        {
            Member = member;
            Value = value;
        }

        public Member Member { get; set; }
        public object Value { get; set; }
    }
}