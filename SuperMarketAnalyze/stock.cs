//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperMarketAnalyze
{
    using System;
    using System.Collections.Generic;
    
    public partial class stock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public stock()
        {
            this.OrderProductRelationship = new HashSet<OrderProductRelationship>();
        }
    
        public int id { get; set; }
        public string product { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public string brand { get; set; }
        public double sale_price { get; set; }
        public double market_price { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProductRelationship> OrderProductRelationship { get; set; }
    }
}
