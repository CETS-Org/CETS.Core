# Migration Instructions: Add Priority to RPT_Report

## Bước 1: Tạo Migration

Chạy lệnh sau trong Package Manager Console hoặc Terminal:

```bash
# Trong thư mục CETS.Core/Infrastructure
dotnet ef migrations add AddPriorityToReport --startup-project ../CETS.API/CETS.API.Web
```

## Bước 2: Migration sẽ tự động tạo code tương tự như sau:

```csharp
public partial class AddPriorityToReport : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Priority",
            table: "RPT_Reports",
            type: "varchar(50)",
            unicode: false,
            maxLength: 50,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Priority",
            table: "RPT_Reports");
    }
}
```

## Bước 3: Apply Migration

```bash
dotnet ef database update --startup-project ../CETS.API/CETS.API.Web
```

## Bước 4: Không cần Seed Data

✅ **Priority được lưu dưới dạng varchar/string**, không cần tạo LookUp data.
Sau khi apply migration, column Priority sẽ có sẵn và có thể nhận giá trị string trực tiếp.

**Gợi ý giá trị Priority:**
- "High"
- "Medium"
- "Low"
- "Urgent"

Hoặc bất kỳ string nào khác tùy theo nhu cầu.

