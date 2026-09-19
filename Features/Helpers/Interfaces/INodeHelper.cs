using System;
using System.Collections.Generic;
using System.Text;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Helpers.Interfaces
{
    public interface INodeHelper
    {
        string GetNodeText(Node node, byte[] fileBytes);
    }
}
