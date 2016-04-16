namespace OmniXaml.ObjectAssembler
{
    using System;
    using System.Collections;
    using Commands;
    using Glass.Core;
    using TypeConversion;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly object rootInstance;
        private readonly XamlType rootInstanceXamlType;
        private readonly IValueContext valueContext;

        public ObjectAssembler(IRuntimeTypeSource typeSource, IValueContext valueContext, Settings settings = null)
            : this(new StackingLinkedList<Level>(), typeSource, GetLifecycleListener(settings), valueContext)
        {
            this.valueContext = valueContext;
            Guard.ThrowIfNull(typeSource, nameof(typeSource));

            TypeSource = typeSource;
            StateCommuter.RaiseLevel();

            rootInstance = settings?.RootInstance;
            var rootInstanceType = rootInstance?.GetType();
            rootInstanceXamlType = rootInstanceType != null ? TypeSource.GetByType(rootInstanceType) : null;
        }

        public ObjectAssembler(StackingLinkedList<Level> state,
            IRuntimeTypeSource typeSource,
            IInstanceLifeCycleListener listener,
            IValueContext context)
        {
            StateCommuter = new StateCommuter(state, typeSource, listener, context);
            LifecycleListener = listener;
        }

        public StateCommuter StateCommuter { get; }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;

        public IInstanceLifeCycleListener LifecycleListener { get; set; }
        public ITopDownValueContext TopDownValueContext => valueContext.TopDownValueContext;

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public IRuntimeTypeSource TypeSource { get; }

        public void Process(Instruction instruction)
        {
            Command command;

            switch (instruction.InstructionType)
            {
                case InstructionType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(instruction.NamespaceDeclaration, StateCommuter);
                    break;
                case InstructionType.StartObject:
                    command = new StartObjectCommand(StateCommuter, TypeSource, instruction.XamlType, rootInstance);
                    break;
                case InstructionType.StartMember:
                    command = new StartMemberCommand(StateCommuter, GetActualMemberFromMemberSpecifiedInInstruction(instruction.Member));
                    break;
                case InstructionType.Value:
                    command = new ValueCommand(StateCommuter, valueContext, (string) instruction.Value);
                    break;
                case InstructionType.EndObject:
                    command = new EndObjectCommand(StateCommuter, stateCommuter => Result = stateCommuter.Current.Instance, LifecycleListener);
                    break;
                case InstructionType.EndMember:
                    command = new EndMemberCommand(TypeSource, StateCommuter, valueContext);
                    break;
                case InstructionType.GetObject:
                    command = new GetObjectCommand(StateCommuter);
                    break;
                default:
                    throw new ParseException($"The XamlInstructionType {instruction.InstructionType} has an unexpected value");
            }

            command.Execute();
        }

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            StateCommuter.Current.Instance = instance;

            var collection = instance as ICollection;
            if (collection != null)
            {
                StateCommuter.Current.Collection = collection;
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