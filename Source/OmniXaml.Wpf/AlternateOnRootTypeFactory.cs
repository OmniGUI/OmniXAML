namespace OmniXaml.Wpf
{
    using System;

    public class AlternateOnRootTypeFactory : ITypeFactory
    {
        private readonly ITypeFactory forRoot;
        private readonly ITypeFactory forNonRoot;

        protected AlternateOnRootTypeFactory(ITypeFactory forRoot, ITypeFactory forNonRoot)
        {
            this.forRoot = forRoot;
            this.forNonRoot = forNonRoot;
        }

        private bool RootIsRead { get; set; }

        public object Create(Type type, params object[] args)
        {
            if (RootIsRead)
            {
                return forNonRoot.Create(type, args);
            }

            RootIsRead = true;
            return forRoot.Create(type, args);
        }

        public bool CanCreate(Type type)
        {
            return forNonRoot.CanCreate(type);
        }
    }
}