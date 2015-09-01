namespace XamlViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Services.Mvvm;
    using OmniXaml.Typing;
    using OmniXaml.Visualization;

    public class VisualizerNodeViewModel : ViewModel
    {
        private readonly VisualizationNode model;
        private bool isExpanded;
        private readonly List<VisualizerNodeViewModel> children;

        public VisualizerNodeViewModel(VisualizationNode model)
        {
            this.model = model;
            this.IsExpanded = true;
            children = model.Children.Select(node => new VisualizerNodeViewModel(node)).ToList();

            CollapseBranchCommand = new RelayCommand(o => CollapseChildren());
            ExpandBranchCommand = new RelayCommand(o => ExpandChildren());
        }

        private void ExpandChildren()
        {
            SetExpanded(true);
        }

        private void CollapseChildren()
        {
            SetExpanded(false);
        }

        private void SetExpanded(bool value)
        {
            IsExpanded = value;
            foreach (var visualizerNodeViewModel in Children)
            {
                visualizerNodeViewModel.SetExpanded(value);
            }
        }

        public string Name => GetName(model);

        private string GetName(VisualizationNode visualizationNode)
        {
            switch (NodeType)
            {
                case NodeType.Member:
                    return GetMemberName(visualizationNode);

                case NodeType.Object:
                    return visualizationNode.XamlInstruction.XamlType.Name;

                case NodeType.GetObject:
                    return "Collection";

                case NodeType.NamespaceDeclaration:
                    return "Namespace Declaration: Mapping " + visualizationNode.XamlInstruction.NamespaceDeclaration;

                case NodeType.Value:
                    return $"\"{visualizationNode.XamlInstruction.Value}\"";

                case NodeType.Root:
                    return "Root";
            }

            throw new InvalidOperationException("The instruction type {NodeType} cannot be handled.");
        }

        private static string GetMemberName(VisualizationNode visualizationNode)
        {
            var mutableXamlMember = visualizationNode.XamlInstruction.Member as MutableXamlMember;

            if (mutableXamlMember != null)
            {
                if (!mutableXamlMember.IsAttachable)
                {
                    return mutableXamlMember.Name;
                }

                return mutableXamlMember.DeclaringType.Name + "." + mutableXamlMember.Name;
            }

            var member = visualizationNode.XamlInstruction.Member;

            return member.IsDirective ? "[(" + member.Name + ") Directive]": member.Name;
        }

        public IEnumerable<VisualizerNodeViewModel> Children => children;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        private static NodeType GetNodeType(XamlInstruction current)
        {
            switch (current.InstructionType)
            {
                case XamlInstructionType.StartMember:
                case XamlInstructionType.EndMember:
                    return NodeType.Member;

                case XamlInstructionType.StartObject:
                case XamlInstructionType.EndObject:
                    return NodeType.Object;

                case XamlInstructionType.GetObject:
                    return NodeType.GetObject;

                case XamlInstructionType.NamespaceDeclaration:
                    return NodeType.NamespaceDeclaration;

                case XamlInstructionType.Value:
                    return NodeType.Value;

                case XamlInstructionType.None:
                    return NodeType.Root;
            }

            throw new InvalidOperationException("Cannot translate the type");
        }

        public NodeType NodeType => GetNodeType(model.XamlInstruction);

        public ICommand CollapseBranchCommand { get; private set; }
        public ICommand ExpandBranchCommand { get; private set; }
    }
}