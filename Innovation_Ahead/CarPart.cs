//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Innovation_Ahead
{
    using System;
    using System.Collections.Generic;
    
    public partial class CarPart
    {
        public int sno { get; set; }
        public string carName { get; set; }
        public string makeyear { get; set; }
        public string sparePart { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
    
        public virtual UserRegister UserRegister { get; set; }
    }
}
