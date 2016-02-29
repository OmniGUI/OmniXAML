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
                    return visualizationNode.Instruction.XamlType.Name;

                case NodeType.GetObject:
                    return "Collection";

                case NodeType.NamespaceDeclaration:
                    return "Namespace Declaration: Mapping " + visualizationNode.Instruction.NamespaceDeclaration;

                case NodeType.Value:
                    return $"\"{visualizationNode.Instruction.Value}\"";

                case NodeType.Root:
                    return "Root";
            }

            throw new InvalidOperationException("The instruction type {NodeType} cannot be handled.");
        }

        private static string GetMemberName(VisualizationNode visualizationNode)
        {
            var mutableXamlMember = visualizationNode.Instruction.Member as MutableMember;

            if (mutableXamlMember != null)
            {
                if (!mutableXamlMember.IsAttachable)
                {
                    return mutableXamlMember.Name;
                }

                return mutableXamlMember.DeclaringType.Name + "." + mutableXamlMember.Name;
            }

            var member = visualizationNode.Instruction.Member;

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

        private static NodeType GetNodeType(Instruction current)
        {
            switch (current.InstructionType)
            {
                case InstructionType.StartMember:
                case InstructionType.EndMember:
                    return NodeType.Member;

                case InstructionType.StartObject:
                case InstructionType.EndObject:
                    return NodeType.Object;

                case InstructionType.GetObject:
                    return NodeType.GetObject;

                case InstructionType.NamespaceDeclaration:
                    return NodeType.NamespaceDeclaration;

                case InstructionType.Value:
                    return NodeType.Value;

                case InstructionType.None:
                    return NodeType.Root;
            }

            throw new InvalidOperationException("Cannot translate the type");
        }

        public NodeType NodeType => GetNodeType(model.Instruction);

        public ICommand CollapseBranchCommand { get; private set; }
        public ICommand ExpandBranchCommand { get; private set; }
    }
}