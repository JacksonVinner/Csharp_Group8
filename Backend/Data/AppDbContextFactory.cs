using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // 设计时使用的连接字符串（仅用于生成迁移，不实际连接）
        var connectionString = "Server=localhost;Database=dotnet;User=root;Password=password;";
        
        optionsBuilder.UseMySql(
            connectionString, 
            new MySqlServerVersion(new Version(8, 0, 21)),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure(0)
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}

