//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntitiesObject.Entities.TanTamEntities
{
    using System;
    
    public partial class Ins_CompanyDepartment_GetAll_Result
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> SortIndex { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsHead { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Color { get; set; }
    }
}
