//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirTicket.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class LOAIHANHKHACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAIHANHKHACH()
        {
            this.QUYDINHGIAVEs = new HashSet<QUYDINHGIAVE>();
            this.VECHUYENBAYs = new HashSet<VECHUYENBAY>();
        }
    
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string PhotoUrl { get; set; }
        public Nullable<int> TuoiMin { get; set; }
        public Nullable<int> TuoiMax { get; set; }
        public Nullable<int> SoLuongMin { get; set; }
        public Nullable<int> SoLuongMax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUYDINHGIAVE> QUYDINHGIAVEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VECHUYENBAY> VECHUYENBAYs { get; set; }
    }
}
