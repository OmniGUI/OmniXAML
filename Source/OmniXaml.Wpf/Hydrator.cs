namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Parsers.Parser;
    using Parsers.ProtoParser;
    using Services.DotNetFx;
    using Typing;

    internal class Hydrator
    {
        private readonly IEnumerable<Type> inflatables;
        private readonly IRuntimeTypeSource typeSource;
        private readonly XamlInstructionBuilder instructionBuilder;

        public Hydrator(IEnumerable<Type> inflatables, IRuntimeTypeSource typeSource)
        {
            this.inflatables = inflatables;
            this.typeSource = typeSource;
            instructionBuilder = new XamlInstructionBuilder(typeSource);
        }

        public IEnumerable<Instruction> Hydrate(IEnumerable<Instruction> nodes)
        {
            var processedNodes = new Collection<Instruction>();
            var skipNext = false;
            foreach (var xamlNode in nodes)
            {
                var matchedInflatable = GetMatchedInflatable(xamlNode);

                if (matchedInflatable != null)
                {
                    var toAdd = ReadNodes(xamlNode.XamlType.UnderlyingType);
                    var croppedNodes = Crop(toAdd, xamlNode.XamlType, TypeSource.GetByType((matchedInflatable)));

                    foreach (var croppedNode in croppedNodes)
                    {
                        processedNodes.Add(croppedNode);
                        skipNext = true;
                    }
                }
                else
                {
                    if (skipNext)
                    {
                        skipNext = false;
                    }
                    else
                    {
                        processedNodes.Add(xamlNode);
                    }
                }
            }

            return processedNodes;
        }

        private IRuntimeTypeSource TypeSource => typeSource;

        private IEnumerable<Instruction> Crop(IEnumerable<Instruction> original, XamlType newType, XamlType oldType)
        {
            var list = original.ToList();
            var nodeToReplace = list.First(node => NodeHasSameType(oldType, node));
            var id = list.IndexOf(nodeToReplace);
            list[id] = instructionBuilder.StartObject(newType.UnderlyingType);
            return list;
        }

        private static bool NodeHasSameType(XamlType oldType, Instruction instruction)
        {
            var xamlType = instruction.XamlType;
            if (xamlType != null)
            {
                var nodeHasSameType = xamlType.Equals(oldType);
                return nodeHasSameType;
            }

            return false;
        }

        private static IEnumerable<Instruction> ReadNodes(Type underlyingType)
        {
            var resourceProvider = new InflatableTranslator();

            using (var stream = resourceProvider.GetInflationSourceStream(underlyingType))
            {
                var reader = new XmlCompatibilityReader(stream);
                var runtimeTypeSource = new WpfRuntimeTypeSource();
                var loader = new InstructionParser(runtimeTypeSource);
                var protoParser = new ProtoInstructionParser(runtimeTypeSource);

                return loader.Parse(protoParser.Parse(reader));
            }
        }
      
        private Type GetMatchedInflatable(Instruction instruction)
        {
            if (instruction.XamlType != null)
            {
                var matches = from inflatable in inflatables
                              where inflatable.IsAssignableFrom(instruction.XamlType.UnderlyingType)
                              select inflatable;

                return matches.FirstOrDefault();
            }

            return null;
        }
    }
}