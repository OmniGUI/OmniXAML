namespace OmniXaml.ObjectAssembler
{
    using System;
    using System.Collections;
    using Commands;
    using Glass;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly object rootInstance;
        private readonly XamlType rootInstanceXamlType;

        public ObjectAssembler(IRuntimeTypeSource typeSource, ITopDownValueContext topDownValueContext, Settings settings = null)
            : this(new StackingLinkedList<Level>(), typeSource, topDownValueContext, GetLifecycleListener(settings))
        {
            Guard.ThrowIfNull(typeSource, nameof(typeSource));
            Guard.ThrowIfNull(topDownValueContext, nameof(topDownValueContext));

            TypeSource = typeSource;
            TopDownValueContext = topDownValueContext;
            StateCommuter.RaiseLevel();

            rootInstance = settings?.RootInstance;
            var rootInstanceType = rootInstance?.GetType();
            rootInstanceXamlType = rootInstanceType != null ? TypeSource.GetByType(rootInstanceType) : null;
        }

        public ObjectAssembler(StackingLinkedList<Level> state,
            IRuntimeTypeSource typeSource,
            ITopDownValueContext topDownValueContext,
            IInstanceLifeCycleListener listener)
        {
            StateCommuter = new StateCommuter(state, typeSource, topDownValueContext, listener);
            LifecycleListener = listener;
        }

        public IInstanceLifeCycleListener LifecycleListener { get; set; }

        public StateCommuter StateCommuter { get; }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;
        public ITopDownValueContext TopDownValueContext { get; }

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public IRuntimeTypeSource TypeSource { get; }

        public void Process(Instruction instruction)
        {
            Command command;

            switch (instruction.InstructionType)
            {
                case InstructionType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(this, instruction.NamespaceDeclaration);
                    break;
                case InstructionType.StartObject:
                    command = new StartObjectCommand(this, instruction.XamlType, rootInstance);
                    break;
                case InstructionType.StartMember:
                    command = new StartMemberCommand(this, GetActualMemberFromMemberSpecifiedInInstruction(instruction.Member));
                    break;
                case InstructionType.Value:
                    command = new ValueCommand(this, TopDownValueContext, (string) instruction.Value);
                    break;
                case InstructionType.EndObject:
                    command = new EndObjectCommand(this);
                    break;
                case InstructionType.EndMember:
                    command = new EndMemberCommand(this, TopDownValueContext);
                    break;
                case InstructionType.GetObject:
                    command = new GetObjectCommand(this);
                    break;
                default:
                    throw new ParseException($"The XamlInstructionType {instruction.InstructionType} has an unexpected value");
            }

            command.Execute();
        }

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            var tempQualifier = StateCommuter;
            tempQualifier.Current.Instance = instance;

            var collection = instance as ICollection;
            if (collection != null)
            {
                tempQualifier.Current.Collection = collection;
            }
        }

        private static IInstanceLifeCycleListener GetLifecycleListener(Settings settings)
        {
            if (settings?.InstanceLifeCycleListener != null)
            {
                return settings.InstanceLifeCycleListener;
            }

            return new NullLifecycleListener();
        }

        private MemberBase GetActualMemberFromMemberSpecifiedInInstruction(MemberBase specifiedMember)
        {
            if (IsLevelOneAndThereIsRootInstance && !specifiedMember.IsDirective && rootInstanceXamlType != null)
            {
                var attachable = specifiedMember as AttachableMember;

                MemberBase actualMember;

                if (attachable != null)
                {
                    actualMember = attachable.DeclaringType.GetAttachableMember(specifiedMember.Name);
                }
                else
                {
                    actualMember = rootInstanceXamlType.GetMember(specifiedMember.Name);
                }

                return actualMember;
            }

            return specifiedMember;
        }
    }
}