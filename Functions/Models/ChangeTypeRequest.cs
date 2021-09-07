using System;
using System.Collections.Generic;

using Core.KenticoKontent.Models.Management.Types;

namespace Functions.Models
{
    public class ChangeTypeRequest
    {
        public IDictionary<string, string>? ElementMappings { get; set; }

        public ContentType? SelectedType { get; set; }

        internal void Deconstruct(
            out IDictionary<string, string> elementMappings,
            out ContentType selectedType
            )
        {
            elementMappings = ElementMappings ?? throw new ArgumentNullException(nameof(ElementMappings));
            selectedType = SelectedType ?? throw new ArgumentNullException(nameof(SelectedType));
        }
    }
}