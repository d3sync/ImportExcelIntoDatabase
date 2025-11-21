# Quick Start Guide

## 5-Minute Setup

### Prerequisites
- SQL Server installed (Express, Developer, or any edition)
- An Excel file with data to import

### Step-by-Step

#### 1. Prepare Your Excel File (1 minute)

Create an Excel file with these columns:
- FirstName
- LastName
- Email
- Department
- HireDate

Add a few sample rows of data.

#### 2. Run the Application (4 minutes)

**Tab 1 - Excel File:**
1. Click "Browse..." and select your Excel file
2. Verify the preview looks correct
3. Click "Next >"

**Tab 2 - Database:**
1. Enter server name:
   - For local: `localhost` or `.\SQLEXPRESS`
2. Check "Use Windows Authentication"
3. Click "Test Connection"
4. Select a database (e.g., "TestImport")
5. **Choose one of two options:**
   
   **Option A - Use Existing Table:**
   - Type to search for a table OR select from dropdown
   - Click "Next >"
   
   **Option B - Create New Table (Recommended for first time):**
   - Click "? Create New Table" button
   - Enter table name (e.g., "Employees")
   - Review auto-detected column names and data types
   - Modify if needed
   - Click "Create"
   - Click "Next >"

**Tab 3 - Column Mapping:**
1. Verify columns are auto-mapped correctly
2. All columns should be checked
3. Click "Next >"

**Tab 4 - Import:**
1. Review the summary
2. Click "Start Import"
3. Wait for completion
4. Done! ?

#### 3. Verify the Import

Back in SQL Server Management Studio:

```sql
USE [YourDatabase];
GO

SELECT * FROM [YourTableName];
```

You should see your imported data!

## ?? Quick Start with Auto-Table Creation

The **fastest way** to get started (3 minutes):

1. **Load Excel**: Browse ? Select file ? Next
2. **Connect**: Enter server ? Test Connection
3. **Create Table**: 
   - Select database
   - Click "? Create New Table"
   - Enter "MyData" as table name
   - Click "Create"
   - Click "Next"
4. **Import**: Next ? Start Import ? Done!

The app will:
- ? Analyze your Excel data
- ? Suggest appropriate data types
- ? Create the table automatically
- ? Map all columns
- ? Import your data

## Common Quick Scenarios

### Scenario 1: Quick Import with New Table

**You have:** Customer data in Excel  
**You need:** Get it into SQL Server fast

```
1. Load Excel file
2. Connect to SQL Server
3. Click "Create New Table"
4. Name it "Customers"
5. Review & Create
6. Import ? Done!
```

**Time:** ~2 minutes

### Scenario 2: Import to Existing Table

**You have:** More data to add to existing table  
**You need:** Import without recreating table

```
1. Load Excel file
2. Connect to SQL Server
3. Type table name (e.g., "Cust" to search)
4. Select "Customers"
5. Map columns (auto-mapped if names match)
6. Import ? Done!
```

**Time:** ~3 minutes

### Scenario 3: Selective Column Import

**You have:** Excel with 20 columns  
**You need:** Import only 5 columns

```
1. Load Excel file
2. Connect & select/create table
3. In Column Mapping:
   - Click "Deselect All"
   - Check only needed columns
   - Map them to SQL columns
4. Import ? Done!
```

## ?? Create New Table - Feature Walkthrough

### What Happens When You Click "Create New Table"

1. **Data Analysis**:
   ```
   Excel Column    Sample Data         Suggested Type
   FirstName       "John"           ?  NVARCHAR(100)
   Age             25               ?  INT
   Salary          50000.50         ?  DECIMAL(18,2)
   HireDate        "2024-01-15"     ?  DATE
   IsActive        TRUE             ?  BIT
   ```

2. **Dialog Shows**:
   - Table name field (you enter "Employees")
   - Column grid with all Excel columns
   - Suggested data types (editable)

3. **After Creating**:
   - Table exists in database with ID column
   - Table is auto-selected
   - Ready to map and import

### Data Type Selection Guide

| Excel Data | Suggested Type | When to Change |
|------------|---------------|----------------|
| Short text | NVARCHAR(50) | If text might be longer |
| Medium text | NVARCHAR(100) | ? Good default |
| Long text | NVARCHAR(255) | For descriptions |
| Very long | NVARCHAR(MAX) | For articles/notes |
| Whole numbers | INT | If > 2 billion, use BIGINT |
| Decimals | DECIMAL(18,2) | Good for money |
| Dates | DATE | If time needed, use DATETIME |
| Yes/No | BIT | ? Perfect for boolean |

## Tips for First-Time Users

? **DO:**
- **Use "Create New Table" for first import** (easiest)
- Start with a small test file (< 100 rows)
- Use Windows Authentication if possible (easier)
- Let the app suggest data types (usually correct)
- Review suggested types before creating

? **DON'T:**
- Import into production without testing
- Use special characters in table/column names
- Skip the data preview check
- Ignore data type suggestions (they're smart!)

## ?? Searchable Table Dropdown

### How to Use

**Before:**
- Scroll through 100+ tables
- Hard to find specific table
- Time consuming

**Now:**
- Just start typing: "cust..."
- Auto-complete shows matches: "Customers", "CustomerOrders"
- Select or keep typing
- Much faster!

**Pro Tips:**
- Type middle part of name: "Order" finds "CustomerOrders"
- Case doesn't matter: "CUST" = "cust" = "Cust"
- Works with partial matches

## Keyboard Shortcuts

- `Alt + N` - Next button
- `Alt + P` - Previous button
- `Alt + B` - Browse button
- `Alt + T` - Test Connection
- `Ctrl + F` - Focus on table search (in combo box)

## Need Help?

### "Create New Table" Issues
```
Problem: "Invalid table name"
Solution: 
1. Start with a letter
2. Use only letters, numbers, underscores
3. Avoid spaces (use underscores instead)
4. Example: "Customer_Data" ? "Customer Data" ?
```

### Connection Issues
```
Problem: "Cannot connect to server"
Solution: 
1. Verify SQL Server is running
2. Check server name (try localhost or .\SQLEXPRESS)
3. Try Windows Authentication first
```

### Import Issues
```
Problem: "Import failed"
Solution:
1. Check column mappings
2. Verify data types match
3. Review error message carefully
4. Try creating new table instead
```

### ?? Data Type Issues
```
Problem: "Wrong data type suggested"
Solution:
1. In Create Table dialog, click on data type
2. Select from dropdown:
   - Text: NVARCHAR(50/100/255/MAX)
   - Numbers: INT, BIGINT, DECIMAL
   - Dates: DATE, DATETIME
   - Boolean: BIT
3. Create table
```

## Success Checklist

Before clicking "Start Import", verify:

- [ ] Excel file loaded successfully
- [ ] Data preview looks correct
- [ ] Database connection tested
- [ ] Correct database selected
- [ ] Table selected OR created
- [ ] ?? If new table: reviewed data types
- [ ] Columns are properly mapped
- [ ] At least one column is checked
- [ ] Summary information is correct

## What's Next?

After your first successful import:

1. **Explore "Create New Table":**
   - Try with different Excel structures
   - Experiment with data type changes
   - See how it handles different data

2. **Try Searchable Tables:**
   - Type partial table names
   - Notice how fast you can find tables
   - No more endless scrolling

3. **Advanced Usage:**
   - Import larger datasets
   - Use SQL Authentication
   - Connect to remote servers
   - Work with complex data types

4. **Best Practices:**
   - Always test with sample data first
   - Document your table structures
   - Create reusable Excel templates
   - Backup before large imports

---

**Congratulations!** ?? You're now ready to import Excel data efficiently.

**New Features Highlight:**
- ? Auto-create tables from Excel
- ?? Search tables instantly
- ?? Smart data type detection

For detailed documentation, see:
- [README.md](README.md) - Full documentation
- [CONFIGURATION.md](CONFIGURATION.md) - Advanced configuration
