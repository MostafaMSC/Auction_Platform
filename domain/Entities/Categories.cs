using AuctionSystem.Domain.Entities;

// كلاس يمثل فئة (تصنيف) للمشاريع
public class Category
{
    public int Id { get; set; } // معرف الفئة (Primary Key)
    public string CategoryName { get; set; } = string.Empty; // اسم الفئة
    public string CategoryDescription { get; set; } = string.Empty; // وصف الفئة

    // قائمة المشاريع المرتبطة بهذه الفئة
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
