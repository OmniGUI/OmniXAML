namespace AvaloniaApp.Adapters
{
    using System;
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using Avalonia.Metadata;

    public class OmniDataTemplate : IDataTemplate
    {
        public IControl Build(object param)
        {
            return (IControl) Content.Load();
        }

        public bool Match(object data)
        {
            return true;
        }

        public Type TargetType { get; set; }

        public bool SupportsRecycling { get; } = false;

        [Content]
        public TemplateContent Content { get; set; }
    }
}