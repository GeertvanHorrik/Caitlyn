// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleSerializerModifier.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2014 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Caitlyn.Runtime.Serialization
{
    using Catel.Runtime.Serialization;

    public class RuleSerializerModifier : SerializerModifierBase
    {
        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "ProjectTypes"))
            {
                // TODO: Match the list with WP8 => WP80 somehow?
            }

            base.DeserializeMember(context, memberValue);
        }
    }
}