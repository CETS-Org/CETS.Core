# Migration Instructions: Add AdminResponse to RPT_Report

## Bước 1: Tạo Migration

Chạy lệnh sau trong Package Manager Console hoặc Terminal:

```bash
# Trong thư mục CETS.Core/Infrastructure
dotnet ef migrations add AddAdminResponseToReport --startup-project ../CETS.API/CETS.API.Web
```

## Bước 2: Migration sẽ tự động tạo code tương tự như sau:

```csharp
public partial class AddAdminResponseToReport : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AdminResponse",
            table: "RPT_Reports",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AdminResponse",
            table: "RPT_Reports");
    }
}
```

## Bước 3: Apply Migration

Sau khi generate migration, apply nó vào database:

```bash
dotnet ef database update --startup-project ../CETS.API/CETS.API.Web
```

## Lưu ý

- `AdminResponse` là **nullable string** (optional), nên reports cũ không có admin response vẫn hoạt động bình thường
- Admin response được lưu khi admin thay đổi status của complaint
- Response được hiển thị trong complaint detail dialog sau khi đã được xử lý

