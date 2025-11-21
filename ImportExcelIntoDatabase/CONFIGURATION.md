# Configuration Guide

## Database Setup

Before using the application, ensure your SQL Server is properly configured:

### 1. SQL Server Configuration

#### Enable TCP/IP (for remote connections):
1. Open SQL Server Configuration Manager
2. Navigate to SQL Server Network Configuration > Protocols for [Your Instance]
3. Enable TCP/IP protocol
4. Restart SQL Server service

#### Enable SQL Authentication (if not using Windows Auth):
1. Open SQL Server Management Studio (SSMS)
2. Right-click on the server ? Properties
3. Select "Security" page
4. Choose "SQL Server and Windows Authentication mode"
5. Restart SQL Server service

### 2. Create Test Database and Table

```sql
-- Create a test database
CREATE DATABASE TestImport;
GO

USE TestImport;
GO

-- Create a sample table for importing customer data
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    City NVARCHAR(50),
    Country NVARCHAR(50),
    DateJoined DATE,
    IsActive BIT DEFAULT 1
);
GO

-- Create a sample table for importing product data
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50),
    Price DECIMAL(10,2),
    Stock INT,
    Supplier NVARCHAR(100),
    LastUpdated DATETIME DEFAULT GETDATE()
);
GO

-- Create a sample table for importing sales data
CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATE NOT NULL,
    CustomerName NVARCHAR(100),
    Product NVARCHAR(100),
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    TotalAmount DECIMAL(10,2),
    Region NVARCHAR(50)
);
GO
```

## Excel File Preparation

### Best Practices

1. **Headers**: Use clear, descriptive headers in the first row
2. **Data Types**: Ensure data types are consistent in each column
3. **Date Format**: Use standard date formats (YYYY-MM-DD or MM/DD/YYYY)
4. **Empty Cells**: Empty cells will be imported as NULL
5. **Text**: Remove leading/trailing spaces
6. **Numbers**: Ensure numeric columns contain only numbers

### Sample Excel Structure

#### Customers.xlsx
```
| FirstName | LastName | Email                | Phone          | City       | Country | DateJoined | IsActive |
|-----------|----------|----------------------|----------------|------------|---------|------------|----------|
| John      | Doe      | john.doe@email.com   | +1-555-0101   | New York   | USA     | 2024-01-15 | 1        |
| Jane      | Smith    | jane.smith@email.com | +1-555-0102   | London     | UK      | 2024-01-16 | 1        |
| Bob       | Johnson  | bob.j@email.com      | +1-555-0103   | Toronto    | Canada  | 2024-01-17 | 1        |
```

#### Products.xlsx
```
| ProductName      | Category    | Price  | Stock | Supplier         | LastUpdated |
|------------------|-------------|--------|-------|------------------|-------------|
| Laptop Pro 15    | Electronics | 1299.99| 50    | TechSupply Inc   | 2024-01-20  |
| Wireless Mouse   | Electronics | 29.99  | 200   | TechSupply Inc   | 2024-01-20  |
| Office Desk      | Furniture   | 399.99 | 30    | Furniture Co     | 2024-01-20  |
```

## Connection String Examples

### Windows Authentication
```
Server: localhost
or
Server: .\SQLEXPRESS
or
Server: DESKTOP-ABC123\SQLEXPRESS
```

### SQL Authentication
```
Server: localhost
Username: sa
Password: YourPassword

or

Server: 192.168.1.100
Username: dbuser
Password: SecurePassword123
```

## Common Connection Strings by Scenario

### Local SQL Server Express
- **Server**: `.\SQLEXPRESS` or `localhost\SQLEXPRESS`
- **Authentication**: Windows Authentication

### Local SQL Server (Default Instance)
- **Server**: `localhost` or `.` or `(local)`
- **Authentication**: Windows Authentication

### Remote SQL Server
- **Server**: `192.168.1.100` or `server.domain.com`
- **Authentication**: SQL Authentication (usually)

### Azure SQL Database
- **Server**: `yourserver.database.windows.net`
- **Database**: Your database name
- **Authentication**: SQL Authentication
- **Username**: yourusername@yourserver
- **Password**: Your password

## Column Mapping Tips

### Automatic Mapping
The application will automatically map columns when:
- Excel column name matches SQL column name exactly
- Excel column name matches SQL column name (case-insensitive)

Example:
```
Excel: FirstName ? SQL: FirstName ? (Auto-mapped)
Excel: first_name ? SQL: FirstName ? (Needs manual mapping)
```

### Manual Mapping
1. Uncheck auto-mapped columns you don't want to import
2. Use the dropdown to select different SQL columns
3. You can map multiple Excel columns to the same SQL column (last one wins)

### Data Type Compatibility

| Excel Type | Compatible SQL Types |
|------------|---------------------|
| Text       | VARCHAR, NVARCHAR, CHAR, NCHAR, TEXT |
| Number     | INT, BIGINT, SMALLINT, DECIMAL, FLOAT, MONEY |
| Date       | DATE, DATETIME, DATETIME2, SMALLDATETIME |
| Boolean    | BIT, TINYINT |

## Import Strategies

### Initial Import
1. Create table with appropriate schema
2. Import all columns
3. Verify data integrity

### Incremental Import
1. Import only new records
2. Use WHERE clause in SQL to avoid duplicates (manual step after import)
3. Or use IDENTITY columns and let SQL Server handle IDs

### Update Existing Data
1. First import to a staging table
2. Use MERGE statement to update (manual SQL step)
```sql
MERGE INTO Customers AS target
USING StagingCustomers AS source
ON target.Email = source.Email
WHEN MATCHED THEN UPDATE SET ...
WHEN NOT MATCHED THEN INSERT ...;
```

## Performance Tips

1. **Large Files**: 
   - Split into smaller files (< 10,000 rows each)
   - Import during off-peak hours

2. **Indexes**: 
   - Drop indexes before import
   - Recreate indexes after import

3. **Constraints**:
   - Disable constraints before import
   - Re-enable and validate after import

4. **Transaction Log**:
   - Ensure adequate transaction log space
   - Use SIMPLE recovery model for large imports (test databases)

## Security Considerations

1. **Credentials**: Never share or commit connection credentials
2. **Least Privilege**: Use database accounts with minimal required permissions
3. **SQL Injection**: The application uses parameterized queries (safe)
4. **Audit Trail**: Consider logging import activities
5. **Backup**: Always backup before large imports

## Troubleshooting Guide

### Error: "Cannot open database"
- **Solution**: Verify database name, check permissions

### Error: "Invalid column name"
- **Solution**: Ensure column mappings are correct

### Error: "String or binary data would be truncated"
- **Solution**: Check SQL column sizes vs. Excel data length

### Error: "Cannot insert NULL"
- **Solution**: Map required columns or provide default values

### Error: "Violation of PRIMARY KEY constraint"
- **Solution**: Don't map IDENTITY columns, or ensure unique values

### Error: "Login failed for user"
- **Solution**: Check credentials, server name, and authentication mode

## Support

For issues or questions:
1. Check this configuration guide
2. Review error messages carefully
3. Test with sample data first
4. Verify database and table structure
