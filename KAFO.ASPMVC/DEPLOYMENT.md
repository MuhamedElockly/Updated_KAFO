# KAFO Application Deployment Guide

## 🚀 Production Hosting Setup

### Prerequisites
- .NET 6.0 Runtime (or higher)
- IIS with ASP.NET Core Hosting Bundle
- SQL Server Database (Remote or Local)

### 1. Database Configuration

The application is configured to use the remote database by default:
```
Server=db24116.public.databaseasp.net
Database=db24116
User Id=db24116
Password=i-2X7eM#@Q6x
```

### 2. Environment Configuration

#### For IIS Hosting:
1. Set the `ASPNETCORE_ENVIRONMENT` environment variable to `Production`
2. Ensure the application pool is configured for .NET Core
3. Set appropriate permissions for the application pool identity

#### For Azure App Service:
1. Set the `ASPNETCORE_ENVIRONMENT` application setting to `Production`
2. Configure connection strings in Application Settings

#### For Docker:
```dockerfile
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
```

### 3. Connection String Configuration

#### Option A: Use Remote Database (Recommended for Production)
The application is already configured to use the remote database connection string in `appsettings.Production.json`.

#### Option B: Use Local Database
Update the connection string in `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "RemoteConnection": "Your_Local_Connection_String_Here"
  }
}
```

### 4. Security Configuration

The production configuration includes:
- ✅ HTTPS Redirection
- ✅ HSTS (HTTP Strict Transport Security)
- ✅ Secure Cookie Policy
- ✅ Security Headers
- ✅ Content Compression

### 5. Performance Optimizations

- ✅ Response Compression enabled
- ✅ Static file caching (7 days)
- ✅ Database connection retry logic
- ✅ Optimized logging levels

### 6. Deployment Steps

#### For IIS:
1. Publish the application:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. Copy the published files to your IIS website directory

3. Configure the application pool:
   - .NET CLR Version: "No Managed Code"
   - Managed Pipeline Mode: "Integrated"

4. Set the physical path to the published folder

#### For Azure App Service:
1. Use Azure CLI or Visual Studio to publish
2. Configure application settings in Azure Portal
3. Set the connection string in Application Settings

#### For Docker:
1. Build the Docker image:
   ```bash
   docker build -t kafo-app .
   ```

2. Run the container:
   ```bash
   docker run -d -p 80:80 -e ASPNETCORE_ENVIRONMENT=Production kafo-app
   ```

### 7. Environment Variables

Set these environment variables for production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80;https://+:443
```

### 8. SSL Certificate

For production, ensure you have a valid SSL certificate:
- For IIS: Install the certificate in the certificate store
- For Azure: Use Azure's built-in SSL certificates
- For Docker: Mount the certificate files

### 9. Monitoring and Logging

The application is configured with:
- Structured logging
- Error handling middleware
- Performance monitoring capabilities

### 10. Backup Strategy

Ensure regular backups of:
- Database (SQL Server)
- Application files
- Configuration files

### 11. Troubleshooting

#### Common Issues:

1. **Database Connection Errors**
   - Verify the connection string
   - Check firewall settings
   - Ensure database server is accessible

2. **Authentication Issues**
   - Verify cookie settings
   - Check HTTPS configuration
   - Ensure proper user permissions

3. **Performance Issues**
   - Enable response compression
   - Configure caching
   - Monitor database performance

### 12. Maintenance

Regular maintenance tasks:
- Update .NET Core runtime
- Monitor application logs
- Review security settings
- Backup database regularly

## 🔧 Configuration Files

### appsettings.json
- Base configuration for all environments
- Connection strings
- Security settings

### appsettings.Production.json
- Production-specific settings
- Optimized logging
- Enhanced security

### web.config
- IIS configuration
- Security headers
- Compression settings

## 📞 Support

For deployment issues:
1. Check application logs
2. Verify configuration settings
3. Test database connectivity
4. Review security requirements

---

**Note:** Always test the deployment in a staging environment before going live with production. 