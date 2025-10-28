# KAFO - Point of Sale & Inventory Management System

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-green)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-orange)
![SignalR](https://img.shields.io/badge/SignalR-Real--time-purple)

A comprehensive Point of Sale (POS) and inventory management system built with ASP.NET Core MVC, featuring real-time statistics, multi-role access, and complete business operations management.

## 🚀 Features

### 📊 **Real-time Statistics Dashboard**
- Live profit tracking with SignalR integration
- Daily sales monitoring
- Product sales analytics
- Real-time notifications for all connected users
- Interactive dashboard with instant updates

### 🧾 **Advanced Invoice Management**
- **Cash Invoices**: Traditional cash transactions
- **Credit Invoices**: Deferred payment with customer account management
- **Purchasing Invoices**: Supplier purchase tracking
- **Return Invoices**: Support for cash, credit, and purchasing returns
- Invoice image attachment support
- Automatic total calculation and inventory updates

### 📦 **Comprehensive Inventory Management**
- Product catalog with barcode support
- Category-based organization
- Stock quantity tracking
- Purchase price vs. selling price management
- Average purchase price calculation
- Low stock alerts and monitoring
- Product image management

### 👥 **Multi-Role User System**
- **Admin Role**: Full system access and management
- **Seller Role**: POS operations and sales
- Secure authentication with role-based routing
- User profile management
- Password security and validation

### 💳 **Credit Customer Management**
- Customer account creation and management
- Credit limit tracking
- Payment history
- Debt management
- Balance tracking with automatic calculations

### 📈 **Advanced Reporting System**
- Daily, weekly, and monthly profit reports
- Sales analytics and trends
- Top-selling products analysis
- Most profitable products tracking
- Seller performance metrics
- Stock value calculations
- Expected profit projections

### 🖥️ **Multiple Interface Options**
- **Web Application**: Modern responsive web interface
- **Desktop Application**: Windows Forms desktop client
- Cross-platform compatibility
- Mobile-responsive design

## 🏗️ **Architecture**

The project follows a clean architecture pattern with clear separation of concerns:

```
KAFO.sln
├── KAFO.ASPMVC/          # Web Application Layer
├── KAFO.BLL/             # Business Logic Layer
├── KAFO.DAL/             # Data Access Layer
├── KAFO.Domain/          # Domain Models & Entities
├── KAFO.Utility/         # Utility Classes
└── KAFO.WinForms/        # Desktop Application
```

### **Key Components:**

- **ASP.NET Core MVC**: Web application with areas for Admin, Seller, and Identity
- **Entity Framework Core**: Database operations with SQL Server
- **SignalR**: Real-time communication for live statistics
- **Repository Pattern**: Clean data access abstraction
- **Unit of Work**: Transaction management
- **Custom Middleware**: Exception handling and image processing

## 🛠️ **Technology Stack**

- **Backend**: ASP.NET Core 8.0, Entity Framework Core
- **Frontend**: Razor Views, JavaScript, CSS3
- **Database**: SQL Server
- **Real-time**: SignalR
- **Desktop**: Windows Forms
- **Authentication**: Custom Cookie Authentication
- **Architecture**: Clean Architecture, Repository Pattern

## 🚀 **Getting Started**

### Prerequisites
- .NET 8.0 SDK
- SQL Server (Local or Remote)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/KAFO.git
   cd KAFO
   ```

2. **Configure Database**
   - Update connection string in `appsettings.json`
   - Run Entity Framework migrations:
   ```bash
   dotnet ef database update --project KAFO.DAL --startup-project KAFO.ASPMVC
   ```

3. **Run the Application**
   ```bash
   dotnet run --project KAFO.ASPMVC
   ```

4. **Access the Application**
   - Web: `https://localhost:5001`
   - Default admin credentials: (Configure during first setup)

## 📱 **Usage**

### **For Administrators:**
- Access comprehensive dashboard with real-time statistics
- Manage products, categories, and inventory
- Handle user accounts and permissions
- Generate detailed reports and analytics
- Monitor system performance and sales trends

### **For Sellers:**
- Use the POS interface for quick transactions
- Process cash and credit sales
- Manage customer accounts
- View product availability and pricing
- Handle returns and exchanges

## 🔧 **Configuration**

### **Database Configuration**
The application supports both local and remote database configurations:

```json
{
  "ConnectionStrings": {
    "RemoteConnection": "Server=your-server;Database=your-db;User Id=user;Password=pass;"
  }
}
```

### **Production Settings**
- HTTPS redirection enabled
- HSTS security headers
- Secure cookie policies
- Response compression
- Static file caching

## 📊 **Key Features in Detail**

### **Invoice Types:**
- **Cash Invoice**: Immediate payment transactions
- **Credit Invoice**: Deferred payment with customer account tracking
- **Purchasing Invoice**: Supplier purchase management
- **Return Invoices**: Support for all return scenarios

### **Real-time Statistics:**
- Live profit updates
- Daily sales monitoring
- Product movement tracking
- Instant notifications to all users

### **Inventory Management:**
- Automatic stock updates on sales/purchases
- Average cost calculation
- Barcode support
- Category organization
- Image management

## 🤝 **Contributing**

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 **Support**

For support and questions, please open an issue in the GitHub repository.

## 🎯 **Roadmap**

- [ ] Mobile application (React Native/Xamarin)
- [ ] Advanced analytics dashboard
- [ ] Multi-language support
- [ ] API for third-party integrations
- [ ] Cloud deployment options
- [ ] Advanced reporting features

---

**KAFO** - Streamlining your business operations with modern technology and real-time insights.