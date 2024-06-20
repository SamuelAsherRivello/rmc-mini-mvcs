using System;

namespace RMC.Core.Attributes
{
    //TODO: Replace this concept with something like 'filename' for use in Resources.Load()?
    //Which one is more brittle? Guid or filename? Maybe guid is indeed best and keep as is.
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ReferenceByGuidAttribute : Attribute
    {
        public string Guid = "";
    }
}
