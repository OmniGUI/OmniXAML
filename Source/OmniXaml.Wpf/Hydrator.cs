namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Parsers.ProtoParser;
    using Parsers.XamlInstructions;
    using Services.DotNetFx;
    using Typing;

    internal class Hydrator
    {
        private readonly IEnumerable<Type> inflatables;
        private readonly IRuntimeTypeSource typeContext;
        private readonly XamlInstructionBuilder instructionBuilder;

        public Hydrator(IEnumerable<Type> inflatables, IRuntimeTypeSource typeContext)
        {
            this.inflatables = inflatables;
            this.typeContext = typeContext;
            instructionBuilder = new XamlInstructionBuilder(typeContext);
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
                    var croppedNodes = Crop(toAdd, xamlNode.XamlType, TypeContext.GetByType((matchedInflatable)));

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

        private IRuntimeTypeSource TypeContext => typeContext;

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
                var runtimeTypeContext = new WpfRuntimeTypeSource();
                var loader = new XamlInstructionParser(runtimeTypeContext);
                var protoParser = new XamlProtoInstructionParser(runtimeTypeContext);

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