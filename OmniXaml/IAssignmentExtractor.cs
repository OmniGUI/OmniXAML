namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IAssignmentExtractor
    {
        IEnumerable<MemberAssignment> GetAssignments(Type owner, XElement element, IPrefixAnnotator annotator);
    }
}