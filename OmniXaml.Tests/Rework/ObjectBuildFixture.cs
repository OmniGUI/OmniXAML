namespace OmniXaml.Tests.Rework
{
    internal class ObjectBuildFixture
    {
        public ObjectBuildFixture()
        {
            Creator = new InstanceCreatorMock();
            ObjectBuilder = new NewObjectBuilder(Creator);
        }

        public InstanceCreatorMock Creator { get; }

        public NewObjectBuilder ObjectBuilder { get; set; }
    }
}