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
    
    public partial class UserRegister
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRegister()
        {
            this.CarParts = new HashSet<CarPart>();
        }
    
        public string Username { get; set; }
        public string Password { get; set; }
        public string mobileNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarPart> CarParts { get; set; }
    }
}
