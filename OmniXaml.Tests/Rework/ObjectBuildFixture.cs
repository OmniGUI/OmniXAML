namespace OmniXaml.Tests.Rework
{
    using OmniXaml.Rework;

    internal class ObjectBuildFixture
    {
        public ObjectBuildFixture()
        {
            Creator = new SmartInstanceCreatorMock();
            Converter = new SmartConverterMock();
            ObjectBuilder = new NewObjectBuilder(Creator, Converter);
        }

        public SmartConverterMock Converter { get; set; }

        public SmartInstanceCreatorMock Creator { get; }

        public NewObjectBuilder ObjectBuilder { get; set; }
    }
}